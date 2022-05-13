import requests
import re
from datetime import datetime
import connectionSqlServer

dates = []
dateRegex = '[0-9]{1,2}/[0-9]{1,2}/[0-9]{2}'

first_year = 2001
last_year = 2079

for year in range(first_year,last_year+1):
    try:
        r = requests.get(f"https://www.anbima.com.br/feriados/fer_nacionais/{year}.asp")
    except Exception as e:
        print(f"Erro ao capturar dados do ano {year}")    
    finally:
        thisYearDates = re.findall(dateRegex, str(r.content))
        thisYearDates = [date[0:-2:]+str(year) for date in thisYearDates]
        dates = dates + thisYearDates
    
#Formata para year-month-day
#Após o ano de 2068 a conversão de datas usa 1900 quando há dois digitos no ano
formated_dates = [datetime.strptime(date, '%d/%m/%Y').strftime('%Y-%m-%d')  for date in dates]

from datetime import datetime, timedelta
today = datetime.today().strftime('%Y-%m-%d %H:%M:%S') 
len(formated_dates)



import pypyodbc 
conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()
#
cursor.execute('DELETE FROM dbo.Holidays')
conn.commit()

for date in formated_dates:
    try:
        query = "INSERT INTO dbo.Holidays(Date, CountryCode, Type, StateCode, CityName, DateLastUpdate)"
        query +=f" VALUES ('{date}', 'BR', 'Federal', '','', '{today}');"
    #    print(query)
        cursor.execute(query)
    except Exception as e:
        print(f"Erro ao inserir data {date}")
conn.commit()
    
