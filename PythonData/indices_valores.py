import pandas as pd
import pypyodbc 
import connectionSqlServer
from datetime import datetime, timedelta
# Consultar os dados do BC: 
# https://www3.bcb.gov.br/sgspub/localizarseries/localizarSeries.do?method=prepararTelaLocalizarSeries
#https://www.tesourodireto.com.br/titulos/calculadora.htm
#Formato API
#


indices = ['IPCA','IGPM','SELIC','CDI','Poupança'
                    , 'Taxa Referencial (TR)'
                    ]

dates = []

conn = pypyodbc.connect(connectionSqlServer.getConnectionString())



cursor = conn.cursor()


table = cursor.execute('''select dateadd(day, 1, MaxDate) as MaxDate, IndexName from (
SELECT  max(Date) as MaxDate, IndexName FROM dbo.IndexValues group by IndexName 
) t1''')
df_db = pd.DataFrame(tuple(t) for t in table.fetchall())


today = (datetime.today()- timedelta(days = 2)) 

if(df_db.shape[0] > 0):
    columns = [column[0] for column in table.description]
    df_db.columns = columns
    df_db['maxdate'] = pd.to_datetime(df_db['maxdate']) 
    dates = [df_db[df_db['indexname'] == indice].iloc[0]['maxdate'].strftime('%d/%m/%Y') for indice in indices]

    
else:
    dates = ['01/01/1900' for i in indices]


finalDates = ['01/01/2101' for i in indices]


lists = {
        'Indice': indices,
        'Link': ["http://api.bcb.gov.br/dados/serie/bcdata.sgs.433/dados?formato=csv",
                  "http://api.bcb.gov.br/dados/serie/bcdata.sgs.189/dados?formato=csv",
                  "https://api.bcb.gov.br/dados/serie/bcdata.sgs.11/dados?formato=csv",
                  "https://api.bcb.gov.br/dados/serie/bcdata.sgs.12/dados?formato=csv",
                  "http://api.bcb.gov.br/dados/serie/bcdata.sgs.196/dados?formato=csv"
                  ,"http://api.bcb.gov.br/dados/serie/bcdata.sgs.226/dados?formato=csv"
                  ],
        'InitialDate': dates,
       'FinalDate': finalDates,
        }

df = pd.DataFrame(lists, columns = ['Indice', 'Link','InitialDate','FinalDate'])

df_complete = pd.DataFrame()
for index, row in df.iterrows():
    print(row['Indice'])
    print(row['Link'])
#    http://api.bcb.gov.br/dados/serie/bcdata.sgs.{codigo_serie}/dados?formato=csv&dataInicial={dataInicial}&dataFinal={dataFinal}
    try:
        this_df = pd.read_csv(f"{row['Link']}&dataInicial={row['InitialDate']}&dataFinal={row['FinalDate']}", sep = ";", decimal = ",")
        
        this_df['data'] = pd.to_datetime(this_df['data'], format ='%d/%m/%Y')
        this_df['valor'] = (this_df['valor'].astype(float))/100

        this_df['indice'] = row['Indice']    
        this_df.head()
        df_complete = pd.concat([this_df, df_complete])
    except:
        print("Erro ao buscar")
df_complete.head()

df_complete= df_complete.rename(columns={"data":"Date", 
                                   "valor": "Value",
                                   "indice": "IndexName"
                                   })
    
df_complete = df_complete[['Date','Value','IndexName']]

df_complete['Date'] = pd.to_datetime(df_complete['Date'], format = '%Y-%M-%d') 


df_complete = df_complete.drop_duplicates()


for index,row in df_complete.iterrows():
    try:
        print(f"{row['Date']}, Indice {row['IndexName']}")
        query = f"INSERT INTO dbo.IndexValues(IndexName, Date, Value) VALUES ('{row['IndexName']}', '{row['Date']}',{row['Value']});"
        cursor.execute(query)
    except:
        print(f"Erro data {row['Date']}, Indice {row['IndexName']}")
        print(query)
    
conn.commit()

#Ultimo valor


table = cursor.execute('''select t2.* from 
                       (select max(Date) as Date, IndexName from dbo.IndexValues group by IndexName) t1
                       inner join dbo.IndexValues t2 
                       on t1.IndexName = t2.IndexName and t1.Date = t2.Date''')

df_db = pd.DataFrame(tuple(t) for t in table.fetchall())
columns = [column[0] for column in table.description]
df_db.columns = columns
df_db['date'] = pd.to_datetime(df_db['date']) 

# cursor.execute("DELETE FROM dbo.IndexLastValues")
# conn.commit()


for index,row in df_db.iterrows():
    try:
        cursor.execute(f'''INSERT INTO 
            dbo.IndexLastValues(IndexName, Date, Value) 
        VALUES ('{row['indexname']}', '{row['date']}', {row['value']});''')
        conn.commit()
        
        
    except:
        cursor.execute(f'''update 
            dbo.IndexLastValues
            set Date = '{row['date']}', Value =  {row['value']}
            where IndexName = '{row['indexname']}'
            ''')
        conn.commit()


conn.close()



print("Fim Atualização tabela de indices")


