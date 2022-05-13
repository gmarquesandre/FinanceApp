import pandas as pd
import datetime
from datetime import timedelta
import connectionSqlServer
import pypyodbc
conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()

url_lista_fundos ="http://dados.cvm.gov.br/dados/FI/CAD/DADOS/cad_fi.csv"
    
df_lista_fundos = pd.read_csv(url_lista_fundos, sep = ";", encoding = 'Latin-1', low_memory = False)
df_lista_fundos = df_lista_fundos[(df_lista_fundos['VL_PATRIM_LIQ'] > 0) & (df_lista_fundos['SIT'] != 'CANCELADA')]
for year in range(2015, 2022):
    for month in range(1,13):
        if(year == 2016 and month <= 7 or year == 2017 and month > 10):
            continue
        try:
            df = pd.DataFrame()
            anomes = str(year)+ ('0'+str(month) if len(str(month)) == 1 else str(month)) 
            print(anomes)
            url_posicao_diaria_fundo = f"http://dados.cvm.gov.br/dados/FI/DOC/INF_DIARIO/DADOS/inf_diario_fi_{anomes}.csv"
            df = pd.read_csv(url_posicao_diaria_fundo, 
            sep = ";", 
            encoding = 'Latin-1', 
            usecols = ["CNPJ_FUNDO","DT_COMPTC","VL_QUOTA"],
            low_memory = False)
            #  Remove fundos cancelados ou sem patrimonio
            df.merge(df_lista_fundos[["CNPJ_FUNDO"]], on = "CNPJ_FUNDO", how = 'inner')
            
            df= df.rename(columns={"CNPJ_FUNDO":"CNPJ", 
                                "DT_COMPTC": "Date",
                                "VL_QUOTA":"UnitPrice",
                                })
            # i = 0
            # for index,row in df.iterrows():               
            #     try:
            #         i += 1
            #         print(f"anomes {anomes} - restam {df.shape[0] - i}")
            #         query = f'''
            #                 INSERT INTO 
            #                     dbo.InvestmentFundValueHistoric(CNPJ, 
            #                         Date, 
            #                         UnitPrice
            #                         )
            #                     VALUES (
            #                         '{row['CNPJ']}', 
            #                         '{row['Date']}' , 
            #                         {row['UnitPrice']}
            #                     );'''
            #         cursor.execute(query)
            #     except:
            #         print("Erro")
            # cursor.commit()
            nrows = 8000

            total = df.shape[0]
            while(df.shape[0] > 0):
                try:
                    insert_to_tmp_tbl_stmt = f'''INSERT INTO InvestmentFundValueHistoric VALUES (?,?,?);'''
                    cursor.fast_executemany = True
                    cursor.executemany(insert_to_tmp_tbl_stmt, df[:nrows].values.tolist())
                    df = df[nrows:]
                    print(f"anomes {anomes} - Restam {100*df.shape[0]/total:.2f}% dos registros")
                except:
                    print("Erro")
            cursor.commit()
            
        except:
            print(f"Erro anomes {anomes}")
            continue

conn.close()


       