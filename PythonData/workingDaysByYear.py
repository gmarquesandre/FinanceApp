from datetime import datetime
import numpy as np
import pandas as pd
import pypyodbc 

#pega tabela com feriados
import connectionSqlServer
conn = pypyodbc.connect(connectionSqlServer.getConnectionString())

cursor = conn.cursor()
table = cursor.execute('''select year(Date) as Year, count(*) HolidaysCount from 
(select *, DatePart(WEEKDAY, Date) as WeekDayNumber
from Holidays)as t1
where WeekDayNumber not in (1,7)--Remove feriados em dias não úteis, 1 = Domingo 7 = Sábado  
group by year(Date)''')

df_db = pd.DataFrame(tuple(t) for t in table.fetchall())
columns = [column[0] for column in table.description]
df_db.columns = columns


df_db.head()

df_db['InitialDate'] = pd.to_datetime(df_db['year'].astype(str)+"-01-01", 
                                 format='%Y-%m-%d', 
                                 errors='coerce').apply(lambda dt: dt.replace(day=1)).dt.date

df_db['EndDate'] = pd.to_datetime((df_db['year'] +1).astype(str)+"-01-01", 
                                 format='%Y-%m-%d', 
                                 errors='coerce').dt.date
     
df_db['WorkingDays'] = np.busday_count(df_db['InitialDate'].values.astype('datetime64[D]'), df_db['EndDate'].values.astype('datetime64[D]')).astype(int)  - df_db['holidayscount'].astype(int) + 1
     


df_db['Year'] = df_db['year']
df_db = df_db[['Year','WorkingDays']]

today = (datetime.today())

df_db['DateLastUpdate'] = datetime.today().strftime('%Y-%m-%d %H:%M:%S') 


cursor.execute('DELETE FROM dbo.WorkingDaysByYear')
conn.commit()

for index,row in df_db.iterrows():
    cursor.execute(f"INSERT INTO dbo.WorkingDaysByYear(Year, WorkingDays, DateLastUpdate) VALUES ({row['Year']}, {row['WorkingDays']},'{row['DateLastUpdate']}');")
conn.commit()

    
    