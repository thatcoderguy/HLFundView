namespace HLFundView.Models
{
    public class ViewDividend
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public decimal CurrentSharePrice { get; set; }
        public decimal DividendPercent { get; set; }
        public decimal DividendAmount { get; set; }
        public DateOnly DividendExDate { get; set; }
        public string Market { get; set; }

    }
}
