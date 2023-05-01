namespace HLFundView.Models
{

    public class Candle
    {
        public string time { get; set; }
        public string symbol { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public string volume { get; set; }
        public DateTime dateTime { get; set; }
    }

    public class NyseData
    {
        public string symbol { get; set; }
        public int interval { get; set; }
        public List<Candle> candles { get; set; }
    }


}
