using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HLFundView.Models
{
    public class Dividend
    {
        [Key, Column(Order = 0)]
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public double CurrentSharePrice { get; set; }
        public double DividendPercent { get; set; }
        [Key, Column(Order = 2)]
        public double DividendAmount { get; set; }
        [Key, Column(Order = 1)]
        public DateTime DividendExDate { get; set; }
        public string Market { get; set; }

        public Dividend(string symbol, string companyname, string currentprice, double dividendamount, string exdate, string market)
        {
            Symbol = symbol;
            CompanyName = companyname;
            DividendAmount = dividendamount;
            Market = market;

            CurrentSharePrice = double.Parse(currentprice, NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);

            //div% = 100 / lastSalePrice * dividend
            DividendPercent = (100 / CurrentSharePrice) * DividendAmount;

            DividendExDate = DateTime.Parse(exdate);
        }

        public Dividend(string symbol, string companyname, double dividendamount, string exdate, string market)
        {
            Symbol = symbol;
            CompanyName = companyname;
            DividendAmount = dividendamount;
            Market = market;

            //div% = 100 / lastSalePrice * dividend


            DividendExDate = DateTime.Parse(exdate, new CultureInfo("en-US", false));
        }

        public Dividend(string symbol, string companyname, string exdate, string market)
        {
            Symbol = symbol;
            CompanyName = companyname;
            Market = market;
            //div% = 100 / lastSalePrice * dividend


            DividendExDate = DateTime.Parse(exdate, new CultureInfo("en-US", false));
        }


        public Dividend()
        {

        }

        public void UpdateCurrentPrice(string currentprice)
        {
            CurrentSharePrice = double.Parse(Regex.Match(currentprice, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);

            DividendPercent = (100 / CurrentSharePrice) * DividendAmount;
        }


        public void AddPriceAndDividendData(string currentprice, string dividend)
        {

            if (currentprice[currentprice.Length-1] == 'p')
            {
                CurrentSharePrice = double.Parse(Regex.Match(currentprice, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
                CurrentSharePrice = CurrentSharePrice / 100;
            }
            else
            {
                CurrentSharePrice = double.Parse(Regex.Match(currentprice, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
                
            }


            if (dividend[dividend.Length-1] == 'p')
            {
                DividendAmount = double.Parse(Regex.Match(dividend, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
                DividendAmount = DividendAmount / 100;
            }
            else
            {
                DividendAmount = double.Parse(Regex.Match(dividend, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
               
            }

            DividendPercent = (100 / CurrentSharePrice) * DividendAmount;
        }
    }
}
