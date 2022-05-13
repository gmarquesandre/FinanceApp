def getConnectionString():
    
    server = 'financialapidbserver.database.windows.net' 
    database = 'FinancialApi_db' 
    username = 'finadm' 
    password = 'S3nha123#'
    driver = '{ODBC Driver 17 for SQL Server}' 
    connectionString = f"Driver={driver};Server={server};Database={database};Uid={username};Pwd={password}"
    
    return connectionString

