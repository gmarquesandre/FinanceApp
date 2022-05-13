import pandas as pd
import re
import requests
import pypyodbc 
import connectionSqlServer
from bs4 import BeautifulSoup
from datetime import datetime, timedelta

today = (datetime.today()- timedelta(days = 0)) 

s = requests.Session()

# teste = s.get("https://www.anbima.com.br/pt_br/informar/valor-nominal-atualizado.htm")
teste = s.get("https://www.anbima.com.br/informacoes/vna/default.asp")
soup = BeautifulSoup(teste.text)

dtRefVer = soup.select("input[name='Dt_Ref_Ver']")[0]['value']

lastDate = soup.select("input[name='Inicio']")[0]['value']
date = str(today.strftime("%Y-%m-%d"))
# Form Data
data = {
    "Data": lastDate.replace("/",""),
    "escolha": 1,
    "Idioma": "PT",
    "saida": "xls",
    "Dt_Ref_Ver": dtRefVer,
    "Inicio": lastDate
}

teste = s.post("https://www.anbima.com.br/informacoes/vna/vna.asp", data)

soup = BeautifulSoup(teste.text)


vna = soup.select('div[id="listaNTN-B"] tr')[4]
vnaItens = re.findall('(?<=<td>)(.*?)(?=</td>)', str(vna))


igpm = soup.select('div[id="listaNTN-C"] tr')[3]
igpmItens = re.findall('(?<=<td>)(.*?)(?=</td>)', str(igpm))

print(soup)

conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()


#Insere VNA
    
conn.commit()

try:
    cursor.execute(f'''INSERT INTO 
            dbo.IndexLastValues(IndexName, Date, Value) 
        VALUES ('VNA IPCA', '{date}', {vnaItens[1].replace(".","").replace(",",".")});''')

    cursor.execute(f'''INSERT INTO 
            dbo.IndexLastValues(IndexName, Date, Value) 
            VALUES ('VNA IGPM', '{date}', {igpmItens[1].replace(".","").replace(",",".")});''')
    
except:
    try:
        cursor.execute(f'''update 
            dbo.IndexLastValues
            set Date = '{date}', Value =  {vnaItens[1].replace(".","").replace(",",".")}
            where IndexName = 'VNA IPCA'
            ''')
        cursor.execute(f'''update 
            dbo.IndexLastValues
            set Date = '{date}', Value = {igpmItens[1].replace(".","").replace(",",".")}
            where IndexName = 'VNA IGPM'
            ''')
        
    except Exception as e:
        print(e);
        print("Deu ruim")


conn.commit()
