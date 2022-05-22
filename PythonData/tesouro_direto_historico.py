import requests
import pypyodbc
import re
import pandas as pd
from pandas import json_normalize
import connectionSqlServer


df_total= pd.DataFrame()
for ano in range(2020, 2022):
    for type in range(1,7):
        try:
            excel_file = pd.ExcelFile(f"https://apiapex.tesouro.gov.br/aria/v1/sistd/custom/historico?ano={ano}&idSigla={type}")  
            print(f"Ano {ano} Tipo {type}")
            
            sheet_names = excel_file.sheet_names
            for sheet_name in sheet_names:
                df = pd.read_excel(excel_file, sheet_name = sheet_name)
                data_vencimento = df.columns[1]
                new_header = df.iloc[0] #grab the first row for the header
                df = df[1:] #take the data less the header row
                df.columns = new_header #set the header row as the df header
                df['data_vencimento'] = data_vencimento
                df['data_vencimento'] = pd.to_datetime(df['data_vencimento'], format='%d/%m/%Y')
                df['tipo'] = re.sub("^\d+\s|\s\d+\s|\s\d+$", "", sheet_name)
                try:
                    df['Dia'] = pd.to_datetime(df['Dia'], format='%d/%m/%Y')
                except:
                    df['Dia'] = pd.to_datetime(df['Dia'])
                # df = df[(df['Dia'] == df['Dia'].max())]
                df_total = pd.concat([df, df_total])
        except:
            print(f"Erro ao buscar ano = {ano} type {type}")


df_depara_titulos = pd.DataFrame(data = 
    {'de':
        ['NTN-F', 'NTN-B Princ', 'NTN-B', 'NTN-C', 'LTN', 'LFT'] , 
    'para':
        ['Tesouro Prefixado com Juros Semestrais',
            'Tesouro IPCA+',
            'Tesouro IPCA+ com Juros Semestrais',
            'Tesouro IGPM',
            'Tesouro Prefixado',
            'Tesouro Selic'
    ],
    'IndexName':['','IPCA','IPCA','IGPM','','Selic'
                    ]
    })
    
df_total = df_total.merge(df_depara_titulos, left_on = 'tipo', right_on = 'de', how = 'left')
df_total['nome_titulo'] = df_total['para'] +' '+ df_total['data_vencimento'].dt.year.astype(str)




df_total = df_total[['Dia', 'data_vencimento','nome_titulo', 'PU Venda Manhã', 'Taxa Venda Manhã', 'Taxa Compra Manhã', 'PU Compra Manhã']]

df_total= df_total.rename(columns={"Dia":"Date", 
                        "PU Compra Manhã":"UnitPriceBuy",
                        "Taxa Compra Manhã": "FixedInterestValueBuy",
                        "PU Venda Manhã":"UnitPriceSell",
                        "Taxa Venda Manhã": "FixedInterestValueSell",
                        
                        "nome_titulo": "TreasuryBondName",
                        "data_vencimento": "ExpirationDate"
                        })
del(df_total["ExpirationDate"])                    



requests.packages.urllib3.disable_warnings()
requests.packages.urllib3.util.ssl_.DEFAULT_CIPHERS += ':HIGH:!DH:!aNULL'
resp = requests.get("https://www.tesourodireto.com.br/json/br/com/b3/tesourodireto/service/api/treasurybondsinfo.json",
             verify = False)


dictr = resp.json()
recs = dictr['response']['TrsrBdTradgList']
df = json_normalize(recs)
df.columns = [column.replace("TrsrBd.","") for column in df.columns]

df = df.rename(columns={"nm":"TreasuryBondName", 
                        "isinCd":"CodeISIN",
                        })

df = df[["CodeISIN","TreasuryBondName"]]

df_total = df_total.merge(df, on= "TreasuryBondName", how = 'inner')

conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()


i = 0
for index,row in df_total.iterrows():
    i+=1
    print(f"{i} de {df_total.shape[0]}")
    try:
        query = f'''
                INSERT INTO dbo.TreasuryBondValueHistoric (CodeISIN, TreasuryBondName, Date, UnitPriceBuy, UnitPriceSell, FixedInterestValueSell, FixedInterestValueBuy) 
                            VALUES ('{row['CodeISIN']}','{row['TreasuryBondName']}', '{row['Date']}',{row['UnitPriceBuy']},{row['UnitPriceSell']}, {row['FixedInterestValueSell']}, 
                            {row['FixedInterestValueBuy']});'''
        cursor.execute(query)
    except:
        print("sei la")

conn.commit()