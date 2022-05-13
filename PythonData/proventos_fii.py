#Desdobramentos e proventos

#Para Consultar por empresa
#http://www.b3.com.br/pt_br/produtos-e-servicos/negociacao/renda-variavel/empresas-listadas.htm


#Código ISIN http://www.b3.com.br/pt_br/market-data-e-indices/servicos-de-dados/market-data/consultas/mercado-a-vista/codigo-isin/sobre-codigo-isin/codigo-isin.htm#:~:text=Como%20é%20a%20estrutura%20do,país%20(Norma%20ISO%203166)%3B



#Lista de BDRs 
#http://bvmf.bmfbovespa.com.br/cias-listadas/Mercado-Internacional/Mercado-Internacional.aspx?Idioma=pt-br
import requests
import connectionSqlServer
from bs4 import BeautifulSoup
from datetime import datetime
import pandas as pd
import pypyodbc 
import base64
import time

import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

df_proventos = pd.DataFrame()
df_changes = pd.DataFrame()
for i in [2,7]:

    objectSend = base64.b64encode(f'{{"typeFund":{i},"pageNumber":1,"pageSize":20}}'.encode()).decode()

    s = requests.session()

    
    s.get(f"https://sistemaswebb3-listados.b3.com.br/fundsPage/{i}", verify = False)


    if i == 2:
        baseUrl = "https://sistemaswebb3-listados.b3.com.br/fundsProxy/fundsCall/GetListedFundsFNET"
        
    elif i == 7:
        baseUrl = "https://sistemaswebb3-listados.b3.com.br/fundsProxy/fundsCall/GetListedFundsSIG"
        
    else:
        next

    resp = s.get(f"{baseUrl}/{objectSend}", verify = False)


    try:
        dictr = resp.json()

        totalRecords = dictr['page']['totalRecords']

        objectSend = base64.b64encode(f'{{"typeFund":{i},"pageNumber":1,"pageSize":{totalRecords}}}'.encode()).decode()

        
        resp = s.get(f"{baseUrl}/{objectSend}", verify = False)
        
        dictr = resp.json()
        print(f"total {len(dictr['results'])}")
        total = len(dictr["results"])
        x = 0
        for result in dictr["results"]:
            x+=1

            print(f"Tipo {i}, {x}/{total}")
            code = result["acronym"]
            cnpj = result["cnpj"]
            cnpj = cnpj if cnpj != None else 0
            objectSend = base64.b64encode(f'{{"cnpj":{cnpj},"identifierFund":"{code}","typfund":{i}}}'.encode()).decode()
            
            resp = s.get(f"https://sistemaswebb3-listados.b3.com.br/fundsPage/main/{cnpj}/{code}/{i}/events")
            resp = s.get(f"https://sistemaswebb3-listados.b3.com.br/fundsProxy/fundsCall/GetListedSupplementFunds/{objectSend}")

            try:
                dictr = resp.json()
                df = pd.DataFrame.from_records(dictr["cashDividends"])

                df_proventos = pd.concat([df_proventos, df ])

                df = pd.DataFrame.from_records(dictr["stockDividends"])
                if(df.shape[0] > 0):
                    print("WAIT")
                df_changes = pd.concat([df_changes, df])


            except: 
                print(f"Erro ao buscar ativo {code}")
    except:
        print("")

df_proventos = df_proventos.rename(columns={"assetIssued":"AssetCodeISIN", 
                            "paymentDate": "PaymentDate",
                            "rate": "CashAmount",
                            "relatedTo": "Period",
                            "approvedOn": "DeclarationDate",   
                            "label": "Type", 
                            "lastDatePrior": "ExDate",
                            "remarks": "Notes",
                            })

df_proventos["CashAmount"] = df_proventos["CashAmount"].str.replace(".","", regex = True).replace(",",".", regex = True)

df_proventos = df_proventos[["Type","AssetCodeISIN","DeclarationDate","ExDate","CashAmount","Period","PaymentDate","Notes"]] 

df_proventos['Hash'] = pd.util.hash_pandas_object(df_proventos)

df_proventos['DeclarationDate'] = pd.to_datetime(df_proventos['DeclarationDate'], format = '%d/%m/%Y', errors='coerce') 

df_proventos['ExDate'] = pd.to_datetime(df_proventos['ExDate'], format = '%d/%m/%Y', errors='coerce') 

df_proventos['PaymentDate'] = pd.to_datetime(df_proventos['PaymentDate'], format = '%d/%m/%Y', errors='coerce') 


conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()

table = cursor.execute('SELECT * FROM dbo.AssetEarnings')

df_db = pd.DataFrame(tuple(t) for t in table.fetchall())

if(df_db.shape[0] <  1):
    print('entrou')
    for index,row in df_proventos.iterrows():
        
        try:
            query = "INSERT INTO dbo.AssetEarnings(AssetCodeISIN, Type, DeclarationDate, ExDate, CashAmount, Period, PaymentDate, Notes, Hash)" 
            query+= f" VALUES ('{row['AssetCodeISIN']}', '{row['Type']}','{row['DeclarationDate']}','{row['ExDate']}',{row['CashAmount']},'{row['Period']}','{row['PaymentDate']}' , '{row['Notes']}', '{row['Hash']}');"
            cursor.execute(query)
            conn.commit()
        except Exception as e:
            print(f"Erro ao inserir {e}")
        
        
else:    
    for index, row in df_proventos.iterrows():
        if(False):                    
            query = f'''
                       '''
            try:
                cursor.execute(query)           
            except e:
                print(f"Erro ao inserir {e}")
        
        else:            
            try:
                query = "INSERT INTO dbo.AssetEarnings(AssetCodeISIN, Type, DeclarationDate, ExDate, CashAmount, Period, PaymentDate, Notes, Hash)" 
                query+= f" VALUES ('{row['AssetCodeISIN']}', '{row['Type']}','{row['DeclarationDate']}','{row['ExDate']}',{row['CashAmount']},'{row['Period']}','{row['PaymentDate']}' , '{row['Notes']}', '{row['Hash']}');"
                cursor.execute(query)
            except Exception as e:
                print(f"Erro ao inserir o {row['Type']}, {row['AssetCodeISIN']}, {row['Notes']}")
                print(f"{e}")
    conn.commit()



df_changes = df_changes.rename(columns={"label":"Type", 
                         "isinCode":"AssetCodeISIN",
                         "approvedOn": "DeclarationDate",
                         "lastDatePrior": "ExDate",
                         "factor": "GroupingFactor",
                         "isinCode": "ToAssetISIN",
                         "remarks": "Notes",                         
                        })

df_changes['Hash'] = pd.util.hash_pandas_object(df_changes)


df_changes['GroupingFactor'] = df_changes['GroupingFactor'].replace(".",'', regex = True).replace(",",'.', regex = True)


table = cursor.execute('SELECT * FROM dbo.AssetChanges')

df_changes['DeclarationDate'] = pd.to_datetime(df_changes['DeclarationDate'], format = '%d/%m/%Y', errors='coerce') 

df_changes['ExDate'] = pd.to_datetime(df_changes['ExDate'], format = '%d/%m/%Y', errors='coerce') 

df_changes['GroupingFactor'] = float(df_changes['GroupingFactor'].fillna(0))/100

df_db = pd.DataFrame(tuple(t) for t in table.fetchall())

df_db.head()
if(df_db.shape[0] <  1):
    print('entrou')
    for index,row in df_changes.iterrows():
        
        try:
            query = "INSERT INTO dbo.AssetChanges(AssetCodeISIN, Type, DeclarationDate, ExDate, GroupingFactor, ToAssetISIN, Notes, Hash)" 
            query+= f" VALUES ('{row['AssetCodeISIN']}', '{row['Type']}','{row['DeclarationDate']}','{row['ExDate']}',{row['GroupingFactor']},'{row['ToAssetISIN']}','{row['Notes']}', '{row['Hash']}');"
            cursor.execute(query)
            conn.commit()
        except:
            print(f"Erro ao inserir o {row['Type']}, {row['AssetCodeISIN']}, {row['Notes']}")
        
    conn.commit()
else:    
    columns = [column[0] for column in table.description]

    for index, row in df_changes.iterrows():
        if(False):                    
            query = f'''
                       '''
            try:
                cursor.execute(query)           
                conn.commit()
            except:
                print(f"Erro ao atualizar a ação {row['CNPJ']}, UnitPrice {row['UnitPrice']}")
        else:
            
            try:
                query = "INSERT INTO dbo.AssetChanges(AssetCodeISIN, Type, DeclarationDate, ExDate, GroupingFactor, ToAssetISIN, Notes, Hash)" 
                query+= f" VALUES ('{row['AssetCodeISIN']}', '{row['Type']}','{row['DeclarationDate']}','{row['ExDate']}',{row['GroupingFacotr']}','{row['ToAssetISIN']}','{row['Notes']}', '{row['Hash']}');"
                cursor.execute(query)
                conn.commit()
            except:
                print(f"Erro ao inserir o {row['Type']}, {row['AssetCodeISIN']}")
conn.commit()