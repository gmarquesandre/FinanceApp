import pandas as pd

import pypyodbc 
from datetime import datetime, timedelta
url_lista_fundos ="http://dados.cvm.gov.br/dados/FI/CAD/DADOS/cad_fi.csv"
    

df_lista_fundos = pd.read_csv(url_lista_fundos, sep = ";", encoding = 'Latin-1', low_memory = False)
try:
    anomes = (datetime.today()- timedelta(days = 0)).strftime('%Y%m') 

    url_posicao_diaria_fundo = f"http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_{anomes}.csv"
    df_valor_fundos = pd.read_csv(url_posicao_diaria_fundo, sep = ";", encoding = 'Latin-1', low_memory = False)
except:

    anomes = (datetime.today().replace(day=1) - timedelta(days=1)).strftime('%Y%m') 

    url_posicao_diaria_fundo = f"http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_{anomes}.csv"

    df_valor_fundos = pd.read_csv(url_posicao_diaria_fundo, sep = ";", encoding = 'Latin-1', low_memory = False)
    
df_lista_fundos['CLASSE'] = df_lista_fundos['CLASSE'].fillna('')

df_lista_fundos['TRIB_LPRAZO'] = df_lista_fundos['TRIB_LPRAZO'].fillna('N')



df_lista_fundos = df_lista_fundos[(df_lista_fundos['VL_PATRIM_LIQ'] > 0) & (df_lista_fundos['SIT'] != 'CANCELADA')]
df_lista_fundos = df_lista_fundos[["CNPJ_FUNDO","DENOM_SOCIAL","SIT","CLASSE","TRIB_LPRAZO","TAXA_PERFM", "INF_TAXA_PERFM", "TAXA_ADM", "INF_TAXA_ADM"]].drop_duplicates()



df_valor_fundos = df_valor_fundos[["CNPJ_FUNDO","DT_COMPTC","VL_QUOTA"]]



df = df_lista_fundos.merge(df_valor_fundos, on ='CNPJ_FUNDO', how = 'inner')

#Pega a ultima data
df = df[df['DT_COMPTC'] == df['DT_COMPTC'].max()]

df.head()

tax = {
       "S" : "1",
       "N" : "0",
}
       
df= df.rename(columns={"CNPJ_FUNDO":"CNPJ", 
                       "CLASSE":"FundTypeName",
                       "TRIB_LPRAZO": "TaxLongTerm",
                                   "DENOM_SOCIAL": "Name",
                                   "SIT": "Situation",
                                   "DT_COMPTC": "DateLastUpdate",
                                   "VL_QUOTA":"UnitPrice",
                                   "TAXA_PERFM": "PerformanceFee", 
                                   "INF_TAXA_PERFM": "PerformanceFeeInfo",
                                   "TAXA_ADM": "AdministrationFee",
                                   "INF_TAXA_ADM": "AdministrationFeeInfo",        
                                   })

df["BenchmarkIndex"] = ""                                   
df["AdministrationFeeInfo"] = df["AdministrationFeeInfo"].str.replace('\'','\'\'', regex = False).fillna("")
df["AdministrationFee"] = df["AdministrationFee"].fillna(0) 
df["AdministrationFee"] = df["AdministrationFee"]/100 

df["PerformanceFeeInfo"] =  df["PerformanceFeeInfo"].str.replace('\'','\'\'', regex = False).fillna("")

df["PerformanceFee"] = df["PerformanceFee"].fillna(0)
df["PerformanceFee"] = df["PerformanceFee"]/100

df['TaxLongTerm'] = df['TaxLongTerm'].replace(tax, regex = False).astype(int)


df['Name'] = df['Name'].str.replace('\'','\'\'')

first = [
			('CORPORATIVO' , 'CORP'),
			('CORPORATE' , 'CORP'),
			('PRIVADO' , 'PRIV'),
			('FUNDOS' , 'FDO'),
			('FUNDO', 'FDO'),
			('FDOS' , 'FDO'),
			('INVESTIMENTOS', 'INV'),
			('INVESTIMENTO','INV'),
			('INVESTIM', 'INV'),
			('INVEST', 'INV'),
			('QUOTAS', 'COTAS'),
			('ACOES', 'AÇÕES'),
			('PREVIDENCIA','PREV'),
			('PREVIDÊNCIA','PREV'),
			('PREVIDENCIÁRIO','PREV'),
			('PREVIDENCIÁR','PREV'),
			('MULTIMERCADOS' , 'MULT'),
			('MULTIMERCADO' , 'MULT'),
			('MULTIM' , 'MULT'),
			('REFERENCIADO','REF'),
			('INFRAESTRUTURA' , 'INFRA'),
			('INFRA ESTRUTURA' , 'INFRA'),
			('REFERENCIADA','REF'),
			('IMOBILIÁRIO' , 'IMOB'),
			('CRÉDITO', 'CRÉD'),
			('CREDITO', 'CRÉD'),
			('ABSOLUTO', 'ABS'),
			('IBOVESPA', 'IBOV')
]

second = [
        ('FDO DE INV' , 'FI')
        ]

di = [
    ('FI EM COTAS','FIC'),
    ('FI COTAS' , 'FI'),
    ('FI EM AÇÕES', 'FIA'),
    ('FI DE AÇÕES', 'FIA'),    
    ('FI MULT', 'FIM'),
    ('RENDA FIXA', 'RF'),
    ('CURTO PRAZO', 'CP'),
    ('LONGO PRAZO','LP'),
    ('ASSET MANAGEMENT','AM'),
]
df['NameShort'] = df['Name'].str.replace("."," ").replace("  "," ")
for k,v in first:
    df['NameShort'] = df['NameShort'].str.replace(k,v, regex = True)
for k,v in second:
    df['NameShort'] = df['NameShort'].str.replace(k,v, regex = True)
for k,v in di:
    df['NameShort'] = df['NameShort'].str.replace(k,v, regex = True)


df['DateLastUpdate'] = pd.to_datetime(df['DateLastUpdate'], format = '%Y-%M-%d') 





df['CompanyName'] = 'Undefined'


import connectionSqlServer
conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()

cursor.execute('DELETE FROM dbo.InvestmentFundValues')

# i = 0
# for index,row in df.iterrows():               
#     i += 1
#     print(f"restam {df.shape[0] - i}")
#     query = f'''
#         UPDATE dbo.InvestmentFundValues 
#             SET 
#                 DateLastUpdate = '{row['DateLastUpdate']}',
#                 AdministrationFeeInfo = '{row['AdministrationFeeInfo']}',
#                 AdministrationFee= '{row['AdministrationFee']}',
#                 PerformanceFeeInfo = '{row['PerformanceFeeInfo']}',
#                 BenchmarkIndex = '{row['BenchmarkIndex']}',
#                 PerformanceFee = '{row['PerformanceFee']}',
#                 TaxLongTerm = {row['TaxLongTerm']},
#                 FundTypeName = '{row['FundTypeName']}', 
#                 UnitPrice = {row['UnitPrice']}, 
#                 NameShort = '{row['NameShort']}', 
#                 Name = '{row['Name']}', 
#                 Situation = '{row['Situation']}' 
#             WHERE CNPJ = '{row['CNPJ']}'
#         IF @@ROWCOUNT = 0 
#             INSERT INTO 
#                 dbo.InvestmentFundValues(CNPJ, 
#                     AdministrationFeeInfo, 
#                     AdministrationFee, 
#                     PerformanceFeeInfo, 
#                     PerformanceFee, 
#                     BenchmarkIndex, 
#                     TaxLongTerm, 
#                     FundTypeName, 
#                     Name, 
#                     NameShort, 
#                     Situation, 
#                     DateLastUpdate, 
#                     UnitPrice
#                     )
#                 VALUES (
#                     '{row['CNPJ']}', 
#                     '{row['AdministrationFeeInfo']}', 
#                     {row['AdministrationFee']}, 
#                     '{row['PerformanceFeeInfo']}', 
#                     {row['PerformanceFee']}, 
#                     '{row['BenchmarkIndex']}', 
#                     {row['TaxLongTerm']}, 
#                     '{row['FundTypeName']}',
#                     '{row['Name']}',
#                     '{row['NameShort']}',
#                     '{row['Situation']}' ,
#                     '{row['DateLastUpdate']}' , 
#                     {row['UnitPrice']}
#                 );'''
#     cursor.execute(query)
#     # Adiciona na tabela de valore históricos
#     query = f'''
#             INSERT INTO 
#                 dbo.InvestmentFundValueHistoric(CNPJ, 
#                     Date, 
#                     UnitPrice
#                     )
#                 VALUES (
#                     '{row['CNPJ']}', 
#                     '{row['Date']}' , 
#                     {row['DateLastUpdate']}
#                 );'''   
#     cursor.execute(query)
df_upload = df.copy()
nrows = 8000;
total = df.shape[0]
while(df_upload.shape[0] > 0):
    try:
        insert_to_tmp_tbl_stmt = f'''INSERT INTO InvestmentFundValues VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?);'''
        cursor.fast_executemany = True
        cursor.executemany(insert_to_tmp_tbl_stmt, df_upload[["CNPJ",
            "Name",
            "NameShort",
            "Situation",
            "DateLastUpdate",
            "UnitPrice",
            "TaxLongTerm",
            "FundTypeName",
            "AdministrationFee",
            "AdministrationFeeInfo",
            "PerformanceFee",
            "PerformanceFeeInfo",
            "BenchmarkIndex"]][:nrows].values.tolist())
        df_upload = df_upload[nrows:]
        cursor.commit()
    except:
        print("Erro")

df_upload = df.copy()
print("tabela histórica")
while(df_upload.shape[0] > 0):
    try:
        insert_to_tmp_tbl_stmt = f'''INSERT INTO InvestmentFundValueHistoric VALUES (?,?,?);'''
        cursor.fast_executemany = True
        cursor.executemany(insert_to_tmp_tbl_stmt, df_upload[["CNPJ","DateLastUpdate","UnitPrice"]][:nrows].values.tolist())
        print(f'{len(df_upload)} rows to insert to the InvestmentFundValueHistoric table')
        df_upload = df_upload[nrows:]
        cursor.commit()

    except:
        print("Erro")




   

conn.commit()






