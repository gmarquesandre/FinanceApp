import yfinance as yf

msft = yf.Ticker("ITUB")

# get stock info
print(msft.info)

# get historical market data
hist = msft.history(period="5d")


# Change period to last full year
msft.history(period="10y")

# show actions (dividends, splits)
print(msft.actions)