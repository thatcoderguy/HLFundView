using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace HLFundView.Models
{
    public class Dividend
    {
        [Key, Column(Order = 0)]
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public double CurrentSharePrice { get; set; }
        public double DividendPercent { get; set; }
        public double DividendAmount { get; set; }
        [Key, Column(Order = 1)]
        public DateTime DividendExDate { get; set; }

        public Dividend(string symbol, string companyname, string currentprice, double dividendamount, DateTime exdate)
        {
            Symbol = symbol;
            CompanyName = companyname;
            DividendAmount = dividendamount;
            DividendExDate = exdate;

            CurrentSharePrice = double.Parse(currentprice, NumberStyles.AllowCurrencySymbol | NumberStyles.Currency);

            //div% = 100 / lastSalePrice * dividend
            DividendPercent = (100 / CurrentSharePrice) * DividendAmount;
        }

        public Dividend()
        {

        }
    }
}
