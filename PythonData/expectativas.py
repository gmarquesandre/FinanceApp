import urllib.request, json 
import pandas as pd
# import numpy as np
from pandas.io.json import json_normalize
from datetime import datetime
import connectionSqlServer
import pypyodbc
# coleta dados api
with urllib.request.urlopen("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais?$top=1000&$orderby=Data%20desc&$format=json&$select=Indicador,Data,DataReferencia,Media,Mediana,Minimo,Maximo,numeroRespondentes,baseCalculo") as url:
    monthly_data = json.loads(url.read().decode())

#with urllib.request.urlopen("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoTrimestrais?$top=100&$orderby=Data%20desc&$format=json&$select=Indicador,Data,DataReferencia,Media,Mediana,Minimo,Maximo,numeroRespondentes") as url:
#    quarterly_data = json.loads(url.read().decode())
#
#with urllib.request.urlopen("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoInflacao12Meses?$top=100&$orderby=Data%20desc&$format=json&$select=Indicador,Data,Suavizada,Media,Mediana,Minimo,Maximo,numeroRespondentes") as url:
#    twelve_months_data = json.loads(url.read().decode())
#
#with urllib.request.urlopen("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoAnuais?$top=1000&$orderby=Data%20desc&$format=json&$select=Indicador,IndicadorDetalhe,Data,DataReferencia,Media,Mediana,Minimo,Maximo,numeroRespondentes") as url:
#    annual_data = json.loads(url.read().decode())


# transforma json para df
monthly_data_df = pd.DataFrame.from_dict(
    monthly_data['value'], orient='columns')

#quarterly_data_df = pd.DataFrame.from_dict(
#    quarterly_data['value'], orient='columns')
#print(quarterly_data_df.head)
#
#twelve_months_data_df = pd.DataFrame.from_dict(
#    twelve_months_data['value'], orient='columns')
#print(twelve_months_data_df.head)
#
#annual_data_df = pd.DataFrame.from_dict(
#    annual_data['value'], orient='columns')
#print(annual_data_df.head)
#
#monthly_data_df.head()



today = datetime.today()

#Data posterior à reunião do COPOM
        
copom_meetings = [
    "2021-08-05",
    "2021-09-23",
    "2021-10-28",
    "2021-12-09",
    "2022-02-03",
    "2022-03-17",
    "2022-05-05",
    "2022-06-16",
    "2022-08-04",
    "2022-09-22",
    "2022-10-27",
    "2022-12-08"
]

copom_meetings = [datetime.strptime(x,'%Y-%m-%d') for x in copom_meetings]

lists = {
            'Date': copom_meetings,
            'YearMonth': [x.strftime('%Y-%m') for x in copom_meetings]
        }

df_copom_meeting = pd.DataFrame(lists, columns = ['Date', 'YearMonth'])


#filtra apenas dados da ultima previsão
monthly_data_df = monthly_data_df[monthly_data_df["Data"] == monthly_data_df["Data"].max() ]

monthly_data_df["DateLastUpdate"] = today 

monthly_data_df = monthly_data_df.rename(columns={"Indicador":"IndexName",
                                   "Data":"DateResearch", 
                                   "DataReferencia": "DateStart",
                                   "Mediana": "Median",
                                   "Media": "Average",
                                   "Maximo": "Max",
                                   "Minimo":"Min",
                                   "numeroRespondentes": "ResearchAnswers",
                                   "baseCalculo":"BaseCalculo"
                                   })


for col in ["Average","Max","Min","Median"]:    
    monthly_data_df[col] = monthly_data_df[col].astype(float).round(2)

monthly_data_df[["Average","Max","Min","Median"]].head()

monthly_data_df['IndexName'] = monthly_data_df['IndexName'].str.upper()

monthly_data_df['DateStart'] = pd.to_datetime(monthly_data_df['DateStart'], format = "%m/%Y")

monthly_data_df = monthly_data_df.sort_values(by = ["IndexName","DateStart"])

#Ajusta tabela da Selic

monthly_data_selic = monthly_data_df[monthly_data_df["IndexName"] == 'SELIC' ].copy()

monthly_data_selic["YearMonth"] = monthly_data_selic["DateStart"].dt.to_period('M').astype(str)

monthly_data_selic = monthly_data_selic.merge(df_copom_meeting, on = "YearMonth", how = "left")

monthly_data_selic = monthly_data_selic[(monthly_data_selic["Date"].notnull()) | (monthly_data_selic["DateStart"] > max(copom_meetings))]

monthly_data_selic["DateStart"] = np.where(monthly_data_selic["Date"].notnull(), monthly_data_selic["Date"], monthly_data_selic["DateStart"])

del(monthly_data_selic["YearMonth"])
del(monthly_data_selic["Date"])

#Reduz 0.1 da Selic pois é mais ou menos o valor praticado em relação à meta
for col in ["Average","Max","Min","Median"]:    
    monthly_data_selic[col] = monthly_data_selic[col]
    
#Cria predição do CDI
monthly_data_cdi = monthly_data_selic.copy()
monthly_data_cdi["IndexName"] = "CDI"
    

monthly_data_df = monthly_data_df[monthly_data_df['IndexName'].isin(['IGP-M','IPCA'])]

monthly_data_df = pd.concat([monthly_data_cdi,monthly_data_df])    

monthly_data_df = pd.concat([monthly_data_selic,monthly_data_df ])

monthly_data_df = monthly_data_df.reset_index()

monthly_data_df["DateEnd"] = (monthly_data_df.sort_values(by=['DateStart'], ascending=False)
                       .groupby(['IndexName','BaseCalculo'])['DateStart'].shift(1)) - pd.to_timedelta(1, unit='d')

monthly_data_df["DateEnd"] = monthly_data_df["DateEnd"].fillna(datetime(today.year+100,1,1)) 

###############################################################################################################################

# import pypyodbc 
# import connectionSqlServer
# conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

# cursor = conn.cursor()

# cursor.execute('DELETE FROM dbo.ProspectIndexValues')
# conn.commit()

# for index, row in monthly_data_df.iterrows():

#     query = f'''INSERT INTO 
#                 dbo.ProspectIndexValues(IndexName, DateResearch, DateStart, DateEnd, Median, Average, Min, Max, ResearchAnswers, BaseCalculo, DateLastUpdate )
#                 VALUES ('{row['IndexName']}', '{row['DateResearch']}','{row['DateStart']}','{row['DateEnd']}',{row['Median']},{row['Average']},{row['Min']},{row['Max']},{row['ResearchAnswers']},{row['BaseCalculo']}, '{row['DateLastUpdate']}');'''

#     cursor.execute(query)

# conn.commit()
    

print("Fim atualização tabela de expectativas")