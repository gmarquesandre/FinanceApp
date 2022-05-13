import requests
import pandas as pd
import json
from pandas import json_normalize
from datetime import date
import pypyodbc 
import connectionSqlServer

#try:
#    resp = requests.get("https://sisweb.tesouro.gov.br/apex/f?p=2031:2:0:::", verify = False)
#    
#    soup = bs4.BeautifulSoup(resp.text, 'html.parser')
#    #https://sisweb.tesouro.gov.br/apex/cosis/sistd/obtem_arquivo/462:100856
#    objs = soup.find('div', attrs ={'class','bl-body'}).find_all('a')
#    # objs= objs[0:5]
#    objs = objs[0:6]
#    
#    n = 0
#    
#    df_total= pd.DataFrame()
#    for obj in objs:
#        n+=1
#        excel_file = pd.ExcelFile("https://sisweb.tesouro.gov.br/apex/"+obj['href'])  
#        print(f"{n} de {str(len(objs))}")
#        sheet_names = excel_file.sheet_names
#        for sheet_name in sheet_names:
#            df = pd.read_excel(excel_file, sheet_name = sheet_name)
#            data_vencimento = df.columns[1]
#            new_header = df.iloc[0] #grab the first row for the header
#            df = df[1:] #take the data less the header row
#            df.columns = new_header #set the header row as the df header
#            df['data_vencimento'] = data_vencimento
#            df['data_vencimento'] = pd.to_datetime(df['data_vencimento'], format='%d/%m/%Y')
#            df['tipo'] = re.sub("^\d+\s|\s\d+\s|\s\d+$", "", sheet_name)
#            df['Dia'] = pd.to_datetime(df['Dia'], format='%d/%m/%Y')
#            df = df[(df['Dia'] == df['Dia'].max())]
#            df_total = pd.concat([df, df_total])
#            
#            df_depara_titulos = pd.DataFrame(data = 
#            {'de':
#                 ['NTN-F', 'NTN-B Princ', 'NTN-B', 'NTN-C', 'LTN', 'LFT'] , 
#             'para':
#                 ['Tesouro Prefixado com Juros Semestrais',
#                  'Tesouro IPCA +',
#                  'Tesouro IPCA com Juros Semestrais',
#                  'Tesouro IGPM',
#                  'Tesouro Prefixado',
#                  'Tesouro Selic'
#            ],
#             'IndexName':['','IPCA','IPCA','IGPM','','Selic'
#                          ]
#            })
#        
#    df_total = df_total.merge(df_depara_titulos, left_on = 'tipo', right_on = 'de', how = 'left')
#    df_total['nome_titulo'] = df_total['para'] +' '+ df_total['data_vencimento'].dt.year.astype(str)
#    
#    
#    
#    
#    df_total = df_total[['Dia', 'data_vencimento','nome_titulo', 'PU Venda Manhã', 'Taxa Venda Manhã', 'IndexName']]
#    
#    df_total= df_total.rename(columns={"Dia":"DateLastUpdate", 
#                             "PU Venda Manhã":"UnitPrice",
#                             "Taxa Venda Manhã": "FixedInterestValue",
#                             "nome_titulo": "TreasuryBondName",
#                             "data_vencimento": "ExpirationDate"
#                            })
#            
#        
#    df_total = df_total[df_total['UnitPrice'] > 0]
#except Exception as e:

    
    
requests.packages.urllib3.disable_warnings()
requests.packages.urllib3.util.ssl_.DEFAULT_CIPHERS += ':HIGH:!DH:!aNULL'
resp = requests.get("https://www.tesourodireto.com.br/json/br/com/b3/tesourodireto/service/api/treasurybondsinfo.json",
             verify = False)


dictr = resp.json()
recs = dictr['response']['TrsrBdTradgList']
df = json_normalize(recs)
df.columns = [column.replace("TrsrBd.","") for column in df.columns]

df = df.rename(columns={"cd":"Código", 
                         "nm":"TreasuryBondName",
                         "featrs": "Description",
                         "mtrtyDt": "ExpirationDate",
                          "minInvstmtAmt": "MinInvestmentAmount",
                          "untrInvstmtVal":"UnitPriceBuy",
                          "invstmtStbl":"Description2",
                          "semiAnulIntrstInd":"InterestPaidBySemester",
                          "rcvgIncm":"PublicDescription",
                          "anulInvstmtRate":"FixedInterestValueBuy",
                          "anulRedRate":"FixedInterestValueSell",
                          "minRedQty":"MinSellQuantity",
                          "untrRedVal":"UnitPriceSell",
                          "isinCd":"CodeISIN",
                          "FinIndxs.cd":"IndexCode",
                          "FinIndxs.nm":"IndexName",
                          "wdwlDt":"LastAvailableDate"
                        })

df = df[["CodeISIN","TreasuryBondName","IndexName","ExpirationDate","FixedInterestValueSell","UnitPriceSell","UnitPriceBuy","FixedInterestValueBuy", "LastAvailableDate"]]

df["DateLastUpdate"]  = dictr['response']['TrsrBondMkt']['opngDtTm']

df['ExpirationDate'] = pd.to_datetime(df['ExpirationDate'], format = '%Y-%m-%d %H:%M:%S')

df['DateLastUpdate'] = pd.to_datetime(df['DateLastUpdate'], format = '%Y-%m-%d %H:%M:%S') 

df['LastAvailableDate'] = pd.to_datetime(df['LastAvailableDate'].fillna('2100-01-01 00:00:00'), format = '%Y-%m-%d %H:%M:%S') 

df = df.sort_values(by = ['ExpirationDate'])



conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()
i = 0

df['DateLastUpdate'] = pd.to_datetime(df['DateLastUpdate']).dt.date

for index,row in df.iterrows():

    query = f'''UPDATE dbo.TreasuryBondValues
        SET DateLastUpdate = '{row['DateLastUpdate']}',
        LastAvailableDate = '{row['LastAvailableDate']}',
        FixedInterestValueSell = {row['FixedInterestValueSell']},
        FixedInterestValueBuy = {row['FixedInterestValueBuy']},
        UnitPriceSell = {row['UnitPriceSell']},
        UnitPriceBuy = {row['UnitPriceBuy']}
                WHERE CodeISIN = '{row['CodeISIN']}'
        IF @@ROWCOUNT = 0  
            INSERT INTO dbo.TreasuryBondValues (CodeISIN, TreasuryBondName, DateLastUpdate, UnitPriceBuy, UnitPriceSell, ExpirationDate, IndexName, FixedInterestValueSell, FixedInterestValueBuy, LastAvailableDate ) 
                        VALUES ('{row['CodeISIN']}','{row['TreasuryBondName']}', '{row['DateLastUpdate']}',{row['UnitPriceBuy']},{row['UnitPriceSell']}, '{row['ExpirationDate']}', '{row['IndexName']}', {row['FixedInterestValueSell']}, {row['FixedInterestValueBuy']},  '{row['LastAvailableDate']}');
    '''
    cursor.execute(query)

for index,row in df.iterrows():
    query = f'''
            INSERT INTO dbo.TreasuryBondValueHistoric (CodeISIN, TreasuryBondName, Date, UnitPriceBuy, UnitPriceSell, FixedInterestValueSell, FixedInterestValueBuy) 
                        VALUES ('{row['CodeISIN']}','{row['TreasuryBondName']}', '{row['DateLastUpdate']}',{row['UnitPriceBuy']},{row['UnitPriceSell']}, {row['FixedInterestValueSell']}, {row['FixedInterestValueBuy']});'''
    cursor.execute(query)

conn.commit()


print("Fim atualização tabela tesouro direto")