using HLFundView.Models;

namespace HLFundView.Converter
{
    public static class DivdendToViewDividend
    {
        public static List<ViewDividend> Convert(List<Dividend> dividends)
        {
            List<ViewDividend> dividendlist = new List<ViewDividend>();
            foreach (Dividend dividend in dividends)
            {
                ViewDividend viewDividend = new ViewDividend()
                {
                    Symbol = dividend.Symbol,
                    CompanyName = dividend.CompanyName,
                    DividendPercent = decimal.Round((decimal)dividend.DividendPercent, 2),
                    CurrentSharePrice = decimal.Round((decimal)dividend.CurrentSharePrice, 2),
                    DividendAmount = decimal.Round((decimal)dividend.DividendAmount,2),
                    DividendExDate = DateOnly.Parse(dividend.DividendExDate.ToShortDateString()),
                    Market = dividend.Market
                };

                dividendlist.Add(viewDividend);
            }

            return dividendlist;
        }
    }
}
