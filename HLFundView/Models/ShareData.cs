namespace HLFundView.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ShareData
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string stockType { get; set; }
        public string exchange { get; set; }
        public bool isNasdaqListed { get; set; }
        public bool isNasdaq100 { get; set; }
        public bool isHeld { get; set; }
        public PrimaryData primaryData { get; set; }
        public object secondaryData { get; set; }
        public string marketStatus { get; set; }
        public string assetClass { get; set; }
        public object keyStats { get; set; }
        public List<Notification> notifications { get; set; }
    }

    public class EventType
    {
        public string message { get; set; }
        public string eventName { get; set; }
        public Url url { get; set; }
        public string id { get; set; }
    }

    public class Notification
    {
        public string headline { get; set; }
        public List<EventType> eventTypes { get; set; }
    }

    public class PrimaryData
    {
        public string lastSalePrice { get; set; }
        public string netChange { get; set; }
        public string percentageChange { get; set; }
        public string deltaIndicator { get; set; }
        public string lastTradeTimestamp { get; set; }
        public bool isRealTime { get; set; }
        public string bidPrice { get; set; }
        public string askPrice { get; set; }
        public string bidSize { get; set; }
        public string askSize { get; set; }
        public string volume { get; set; }
    }

    public class ShareRoot
    {
        public ShareData data { get; set; }
        public object message { get; set; }
        public Status status { get; set; }
    }


    public class Url
    {
        public string label { get; set; }
        public string value { get; set; }
    }


}
