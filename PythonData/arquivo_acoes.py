import pandas as pd
import requests
import io
import pypyodbc 
from datetime import datetime, timedelta
import connectionSqlServer

def extrai_dados_bovespa(df):

    name = list(df)
    name[0]
    df = df.rename(columns={f'{name[0]}':'HEADER'})
    
    #deleta a ultima linha
    df = df[:-1]
    
    #Local dos arquivos:
    #http://www.b3.com.br/pt_br/market-data-e-indices/servicos-de-dados/market-data/historico/mercado-a-vista/series-historicas/
    #Layout do arquivo:
    #http://www.b3.com.br/data/files/C8/F3/08/B4/297BE410F816C9E492D828A8/SeriesHistoricas_Layout.pdf 
    
    df["anomesdia"] = df["HEADER"].str.slice(2,10)
    df["data"] = pd.to_datetime(df["anomesdia"], format = '%Y%m%d', errors = 'ignore')
    df["codbdi"] = df["HEADER"].str.slice(10,12)
    df["cod_negociacao_papel"] = df["HEADER"].str.slice(12,24).str.replace(" ","")
    df["tipo_mercado"] = df["HEADER"].str.slice(24,27)
    df["nome_resumido_empresa"] = df["HEADER"].str.slice(27,39)
    df["especificacao_papel"] = df["HEADER"].str.slice(39,49)
    df["prazo_em_dias_mercado_a_termo"] = df["HEADER"].str.slice(49,52)
    df["moeda_referencia"] = df["HEADER"].str.slice(52,56)
    df["preco_abertura"] = df["HEADER"].str.slice(56,69).str.replace("  ","0").astype(float)/100
    df["preco_maximo"] = df["HEADER"].str.slice(69,82).str.replace("  ","0").astype(float)/100
    df["preco_minimo"] = df["HEADER"].str.slice(82,95).str.replace("  ","0").astype(float)/100
    df["preco_medio"] = df["HEADER"].str.slice(95,108).str.replace("  ","0").astype(float)/100
    df["preco_ultimo_negocio"] = df["HEADER"].str.slice(108,121).str.replace("  ","0").astype(float)/100
    df["preco_melhor_oferta_compra"] = df["HEADER"].str.slice(121,134).str.replace("  ","0").astype(float)/100
    df["preco_melhor_oferta_venda"] = df["HEADER"].str.slice(134,147).str.replace("  ","0").astype(float)/100
    df["qtd_de_negocios_efetuados_pregao"] = df["HEADER"].str.slice(147,152).str.replace("  ","0").astype('int64')
    df["qtd_total_tiulos_negociados_papel"] = df["HEADER"].str.slice(152,170).str.replace("  ","0").astype('int64')
    df["volume_titulos_negociados"] = df["HEADER"].str.slice(170,188).str.replace("  ","0").astype('int64')
    df["preco_de_exercicio_opcoes_contrato_mercado_de_termo_secundario"] = df["HEADER"].str.slice(188,201).str.replace("  ","0").astype('int64')
    df["data_vencimento_opcoes_contrato_mercado_de_termo_secundario"] = df["HEADER"].str.slice(202,210).str.replace("  ","0")
    df["fator_cotacao_papel"] = df["HEADER"].str.slice(210,217).str.replace("  ","0").astype('int64')
    df["codigo_acao_isin"] = df["HEADER"].str.slice(230,242).str.replace("  ","")

    del(df["HEADER"])



    return df



deltaDays = 1
if datetime.today().weekday() == 5:
    deltaDays = 1
elif datetime.today().weekday() == 6:
    deltaDays = 2


today = (datetime.today()- timedelta(days = deltaDays)).strftime('%d%m%Y')

today = '13052022'


file = requests.get(f"https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_D{today}.ZIP", stream = True, verify=False)

um teste rolando   
aaaaaaafaaaaaaaaaaa

if file.content == b'The resource you are looking for has been removed, had its name changed, or is temporarily unavailable.':
    print('Data Indisponivel')
    #Enviar mensagem de aviso
    status = False
else:
    df = pd.read_csv(io.BytesIO(file.content), compression = 'zip', encoding ='Latin-1')
    df = extrai_dados_bovespa(df)
    #Tabela do ultimo preço negociado
    # df = df[['data','cod_negociacao_papel','preco_ultimo_negocio','codigo_acao_isin','especificacao_papel','nome_resumido_empresa']]
    # df = df[(df['tipo_mercado'] == '010')][['data','cod_negociacao_papel','preco_ultimo_negocio','codigo_acao_isin','especificacao_papel','nome_resumido_empresa']]
    status = True

print(df.shape)
print(df.drop_duplicates().shape)
print(df["codigo_acao_isin"].drop_duplicates().shape)
if(status):
    df= df.rename(columns={"data":"DateLastUpdate", 
                            "cod_negociacao_papel": "AssetCode",
                            "preco_ultimo_negocio": "UnitPrice",
                            "codigo_acao_isin":"AssetCodeISIN",
                            "nome_resumido_empresa": "CompanyName"
                            })
        
        
    #df[df["AssetCode"] == "MMMC34"][["AssetCode","AssetCodeISIN"]]

    df['DateLastUpdate'] = pd.to_datetime(df['DateLastUpdate'], format = '%Y-%M-%d') 


    table_name = 'Assets'

    conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

    cursor = conn.cursor()

    #cursor.execute('DELETE FROM dbo.Assets')
    #conn.commit()

    print('entrou')
    for index,row in df.iterrows():

        query = f'''
            UPDATE dbo.Assets
            SET DateLastUpdate = '{row['DateLastUpdate']}' , UnitPrice = {row['UnitPrice']}, CompanyName = '{row['CompanyName']}'
                    WHERE AssetCode = '{row['AssetCode']}'
            IF @@ROWCOUNT = 0  
                INSERT INTO dbo.Assets(AssetCodeISIN, AssetCode, CompanyName, UnitPrice, DateLastUpdate) 
                VALUES ('{row['AssetCodeISIN']}','{row['AssetCode']}', '{row['CompanyName']}',{row['UnitPrice']},'{row['DateLastUpdate']}');
        '''
        cursor.execute(query)
    
        
    # Atualizar ações sem movimentação no dia
    df['DateLastUpdate'][0]
    cursor.execute(f"UPDATE dbo.Assets SET DateLastUpdate = '{df['DateLastUpdate'][0]}'")

    conn.commit()

        
    print('Fim Atualização tabela de ações')
else:
    print("Fim Atualização tabela de ações - Não há dados")