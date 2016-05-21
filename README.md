# Requirements As below

I have data in a text format. The data I was provided doesn’t contain any column name.
Data:
Mr. X~BDTýUSDýGBPýEUR~1000ý150ý25ý~20150101ý20150215ý20160310ý20160415~10

Here:
1.	Column Delimiter is “~”
2.	Row Delimiter is the symbol “ý”
3.	Column names given in separate text file

Column Names:
Name, CurrencyCode, CurrentBalance, DateValue, TaxAmount

Current Scenario:	

We can import this data using SQL Server Integration Service and also we can use a SQL Server’s table valued function to split multi delimited values into rows. After all these the scenario does look like as following:

 
I am provided with 200K records and after splitting the data will be more then 10MIL! In SQL Server using table values function it is taking more than 9 hours!

Required:
Transform the data file into CSV or SQL Server insert into script having the multi delimited values into rows.

For an example:
If the data is like,
Mr. X~BDTýUSDýGBPýEUR~1000ý150ý25ý~20150101ý20150215ý20160310ý20160415~10

Then need to transform the data file to following CSV:
Name,CurrencyCode,CurrentBalance,DateValue,TaxAmount
Mr. X;BDT;10500;20150101;10
Mr. X;USD;2500;20150201;NULL	
Mr. X;GBP;1050;20150301;NULL
Mr. X;EUR;NULL;20160415;NULL
Suggestion:

Since there are no column name provided with the data we can create another text file having those column names. While transforming the data into csv we can fetch the column names from the text file.

