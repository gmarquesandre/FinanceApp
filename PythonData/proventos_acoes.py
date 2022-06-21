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
import time

import re

import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
#Configurações Iniciais
base_url = "http://bvmf.bmfbovespa.com.br/cias-listadas"

url_list = []

df_dividendos = pd.DataFrame()

df_alteracoes_acao = pd.DataFrame()

# Lista de A a Z e 0 a 9
AtoZ = [chr(x) for x in range(ord('A'), ord('Z')+1)]
ZeroToNine = [str(i) for i in range(1,10)]
# Concatena listas
loopLettersNumbers = AtoZ + ZeroToNine

#Pegar link das empresas da B3
for i in loopLettersNumbers:
    url = f"http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/BuscaEmpresaListada.aspx?Letra={i}&idioma=pt-br"
    page = requests.get(url, verify = False)

    soup = BeautifulSoup(page.content, 'html.parser')

    for table in soup.findAll('table', {'id': 'ctl00_contentPlaceHolderConteudo_BuscaNomeEmpresa1_grdEmpresa_ctl01'}):
        for tr in table.findAll('tr'):
            for td in tr.findAll('td'):
                for a in td.findAll('a'):
                    this_url = f"{base_url}/empresas-listadas/{a['href']}&idioma=pt-br"
                    url_list.append(this_url.replace("ResumoEmpresaPrincipal","ResumoEventosCorporativos"))

#Pegar empresas estrangeiras
url = f'{base_url}/Mercado-Internacional/Mercado-Internacional.aspx?Idioma=pt-br'
page = requests.get(url, verify = False)

soup = BeautifulSoup(page.content, 'html.parser')

for table in soup.findAll('table', {'id': 'tblBdrs'}):
    for tr in table.findAll('tr'):
        for td in tr.findAll('td'):
            for a in td.findAll('a'):
                this_url = f"{base_url}{a['href'][2:]}&idioma=pt-br"
                url_list.append(this_url.replace("ResumoEmpresaPrincipal","ResumoEventosCorporativos"))


#Remove duplicatas
url_list = list(dict.fromkeys(url_list))
#Alterar após dar certo
print(len(url_list))
regexCvmCode ="(?<==)(.*?)(?=&)"
i = 0

for url in url_list:
    page = requests.get(url, verify = False)
    cvmCode = re.findall(regexCvmCode, url)[0]
    soup = BeautifulSoup(page.content, 'html.parser')
    i +=1
    print(f"Captura {i}/{len(url_list)}")
    try:
        tbl = soup.find("table",{"id":"ctl00_contentPlaceHolderConteudo_grdDividendo_ctl01"})

        data_frame = pd.read_html(str(tbl), decimal=',', thousands='.')[0]


        data_frame['cvmCode'] = cvmCode

        df_dividendos = pd.concat([data_frame, df_dividendos])

    except:
        pass
    try:
        
        tbl = soup.find("table",{"id":"ctl00_contentPlaceHolderConteudo_grdBonificacao_ctl01"})
        
        data_frame = pd.read_html(str(tbl))[0]
        
        data_frame['cvmCode'] = cvmCode        
        
        df_alteracoes_acao = pd.concat([df_alteracoes_acao, data_frame])

    except:
        pass

#Ajusta tabela de dividendos  
        

df_dividendos = df_dividendos.rename(columns={"Proventos":"Type", 
                         "Código ISIN":"AssetCodeISIN",
                         "Deliberado em": "DeclarationDate",
                         "Negócios com até": "ExDate",
                         "Valor (R$)": "CashAmount",
                         "Relativo a": "Period",
                         "Início de Pagamento": "PaymentDate",
                         "cvmCode":"Notes",                         
                        })

# df_dividendos['Period'] = df_dividendos['Period'].str.upper().replace("ANUAL/","")
# df_dividendos['Period'] = df_dividendos['Period'].str.upper().replace("º TRIMESTRE","Q")

# df_dividendos.columns
    
# df_dividendos = df_dividendos[["Type","AssetCodeISIN","DeclarationDate","ExDate","CashAmount","Period","PaymentDate","Notes"]] 
 

# #Ajusta tabela alterações

# df_alteracoes_acao.columns

# df_alteracoes_acao = df_alteracoes_acao.rename(columns={"Proventos":"Type", 
#                          "Código ISIN":"AssetCodeISIN",
#                          "Deliberado em": "DeclarationDate",
#                          "Negócios com até": "ExDate",
#                          "% / Fator de Grupamento": "GroupingFactor",
#                          "Ativo Emitido": "ToAssetISIN",
#                          "cvmCode": "Notes",                         
#                         })
# df_alteracoes_acao  = df_alteracoes_acao[["Type","AssetCodeISIN","DeclarationDate","ExDate","GroupingFactor","ToAssetISIN","Notes"]] 
 

# #Cria hash unico para evitar adicionar o mesmo evento duas vezes
# #Esse Hash não é afetado pela ordem das colunas   
# df_dividendos['Hash'] = pd.util.hash_pandas_object(df_dividendos)
# df_alteracoes_acao['Hash'] = pd.util.hash_pandas_object(df_alteracoes_acao)


        

# df_dividendos['DeclarationDate'] = pd.to_datetime(df_dividendos['DeclarationDate'], format = '%d/%m/%Y', errors='coerce') 

# df_dividendos['ExDate'] = pd.to_datetime(df_dividendos['ExDate'], format = '%d/%m/%Y', errors='coerce') 


# df_dividendos['PaymentDate'] = pd.to_datetime(df_dividendos['PaymentDate'], format = '%d/%m/%Y', errors='coerce') 


# df_alteracoes_acao['DeclarationDate'] = pd.to_datetime(df_alteracoes_acao['DeclarationDate'], format = '%d/%m/%Y', errors='coerce') 

# df_alteracoes_acao['ExDate'] = pd.to_datetime(df_alteracoes_acao['ExDate'], format = '%d/%m/%Y', errors='coerce') 

# df_alteracoes_acao['GroupingFactor'] = df_alteracoes_acao['GroupingFactor']/100

    
df_dividendos.to_csv("df_dividendos_acoes.csv", sep=";", index = False)

df_alteracoes_acao.to_csv("df_dividendos_acoes.csv", sep=";", index = False)    
    
    
    
    
    
    