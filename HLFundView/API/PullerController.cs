using BitMiracle.Docotic.Pdf;
using HLFundView.Data;
using HLFundView.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Xml;

namespace HLFundView.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PullerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PullerController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("pulldata")]
        public async Task<JsonResult> PullData()
        {
            var client = new RestClient("https://www.hl.co.uk/ajax/funds/fund-search/");

            var request = new RestRequest("search?investment=&companyid=&sectorid=&wealth=&unitTypePref=&tracker=&payment_frequency=&payment_type=&yield=&standard_ocf=&perf12m=&perf36m=&perf60m=&fund_size=&num_holdings=&start=60&rpp=6000&lo=0&sort=fd.full_description&sort_dir=asc&", Method.Get);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var queryResult = client.Execute(request);

            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(queryResult.Content);

            foreach (Result fund in myDeserializedClass.Results)
            {

                Fund realData = new Fund(fund.sedol, fund.full_description, fund.fund_name, fund.description,
                     fund.company_id, fund.company_name, fund.unit_type, fund.running_yield, fund.historic_yield,
                     fund.distribution_yield, fund.underlying_yield, fund.gross_yield, fund.gross_running_yield, fund.kiid_url);

                if (_context.Fund.Any(e => e.sedol == fund.sedol))
                {
                    _context.Fund.Attach(realData);
                    _context.Entry(realData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    _context.Fund.Add(realData);
                }

                _context.SaveChanges();
            }

            return new JsonResult(true);
        }

        [HttpGet("pullriskdata")]
        public async Task<JsonResult> PullRiskData()
        {

            List<Fund> allFunds = _context.Fund.ToList();

            foreach (Fund fund in allFunds)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(fund.kiid_url, "blah.pdf");
                }

                using (var pdf = new PdfDocument("blah.pdf"))
                {
                    string formattedText = pdf.GetText();
                    using (var writer = new StreamWriter("formatted.txt"))
                        writer.Write(formattedText);
                }


            }

            return new JsonResult(true);
        }

        [HttpGet("pulllsedividenddata")]
        public async Task<JsonResult> PullLSEDividendData()
        {

            var client = new RestClient("https://www.dividenddata.co.uk/");

            var request = new RestRequest("", Method.Get);

            var queryResult = client.Execute(request);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(queryResult.Content);


            HtmlNodeCollection rows = document.DocumentNode.SelectSingleNode("//div[@class='table-responsive']").SelectSingleNode("//table/tbody").SelectNodes("tr");

            foreach(HtmlNode row in rows)
            {
                //EPIC  0
                //Name  1
                //Market    2
                //Share Price   3
                ////Dividend    4
                //Div Impact    5
                //Declaration Date  6
                ////Ex - Dividend Date ▲    7
                ///Payment Date 8
                ///

                HtmlNodeCollection cols = row.SelectNodes("td");

                Dividend div = new Dividend(cols[0].InnerText, cols[1].InnerText, cols[7].InnerText, "LSE");
                div.AddPriceAndDividendData(cols[3].InnerText, cols[4].InnerText);

                if (!_context.Dividends.Any(e => e.Symbol == cols[0].InnerText
                    && e.DividendExDate == DateTime.Parse(cols[7].InnerText, new CultureInfo("en-US", false))
                    && e.DividendAmount == div.DividendAmount
                    && e.Market == "LSE"))
                {
                    _context.Dividends.Add(div);
                    _context.Entry(div).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
            }

           await _context.SaveChangesAsync();

            return new JsonResult(true);
        }



        [HttpGet("pullnysedividenddata")]
        public async Task<JsonResult> PullNYSEDividendData()
        {

            var client = new RestClient("https://api.benzinga.com/api/v2.1/calendar/");

            DateTime current = DateTime.Now;
            string fromdate = current.Year + "-" + current.Month.ToString("00") + "-" + current.Day.ToString("00");
            current = current.AddYears(1);
            string todate = current.Year + "-" + current.Month.ToString("00") + "-" + current.Day.ToString("00");
            //2023-05-01
            var request = new RestRequest("dividends?token=1c2735820e984715bc4081264135cb90&parameters[date_from]=" + fromdate + "&parameters[date_to]=" + todate + "&parameters[date_sort]=ex&pagesize=1000000", Method.Get);

            var queryResult = client.Execute(request);

            XmlDocument data = new XmlDocument();
            data.LoadXml(queryResult.Content);


            /*<result>
            < dividends is_array = "true" >
            < item >
                        < dividend_type > Cash </ dividend_type >
                    < record_date > 2024 - 03 - 15 </ record_date >
                    < end_regular_dividend > false </ end_regular_dividend >
                    < notes />
                    < ex_dividend_date > 2024 - 03 - 14 </ ex_dividend_date >
                    < dividend > 0.7300 </ dividend >
                    < dividend_yield > 0.0310770540655598 </ dividend_yield >
                    < date > 2023 - 02 - 22 </ date >
                    < ticker > GRMN </ ticker >
                    < frequency > 4 </ frequency >
                    < payable_date > 2024 - 03 - 29 </ payable_date >
                    < name > Garmin </ name >
                    < id > 642738f97dfba500019c64fa </ id >
                    < updated > 1680292318 </ updated >
                    < importance > 0 </ importance >
                    < dividend_prior />
                    < exchange > NYSE </ exchange >
                    < currency > USD </ currency >
              </ item >
            */

            XmlNodeList rows = data.SelectSingleNode("//result/dividends").SelectNodes("item");

            foreach (XmlNode row in rows)
            {

                if (row.SelectSingleNode("exchange").InnerText != "NYSE")
                { 

                    Dividend div = new Dividend(row.SelectSingleNode("ticker").InnerText, row.SelectSingleNode("name").InnerText, dividendamount: row.SelectSingleNode("dividend").InnerText, row.SelectSingleNode("ex_dividend_date").InnerText, "NYSE");

                    if (!_context.Dividends.Any(e => e.Symbol == row.SelectSingleNode("ticker").InnerText
                        && e.DividendExDate == DateTime.Parse(row.SelectSingleNode("ex_dividend_date").InnerText, new CultureInfo("en-US", false))
                        && e.DividendAmount == div.DividendAmount
                        && e.Market == "NYSE"))
                    {
                        _context.Dividends.Add(div);
                        _context.Entry(div).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new JsonResult(true);
        }

        //

        [HttpGet("pullnsasdaqdividendcalander")]
        public async Task<JsonResult> PullDividendCalander()
        {
            int count = 1;
            DateTime current = DateTime.Now;
            if (current.DayOfWeek == DayOfWeek.Sunday)
            {
                current=current.AddDays(1);
            }else if (current.DayOfWeek == DayOfWeek.Saturday)
            {
                current=current.AddDays(2);
            }

            string date = current.Year + "-" + current.Month.ToString("00") + "-" + current.Day.ToString("00");

            List<Row> rows = GetCalanderData(date);

            while (count < 365)
            {
                if (rows == null || rows.Count == 0)
                { }
                else {
                    foreach (Row row in rows)
                    {
                        //ex date
                        //symbol
                        //indicated anual dividend

                        //divident rate = amount paid over year

                        Console.WriteLine(date);
                        Console.WriteLine(row.symbol);

                        //ShareData share = GetShareData(row.symbol);

                        Dividend div = new Dividend(row.symbol, row.companyName, row.indicated_Annual_Dividend, row.dividend_Ex_Date, "NASDAQ");

                        if (!_context.Dividends.Any(e => e.Symbol == row.symbol 
                                && e.DividendExDate == DateTime.Parse(row.dividend_Ex_Date, new CultureInfo("en-US", false))
                                && e.DividendAmount ==row.indicated_Annual_Dividend
                                && e.Market == "NASDAQ"))
                        {
                            _context.Dividends.Add(div);
                            _context.Entry(div).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                        
                    }
                }

                await _context.SaveChangesAsync();

                current =current.AddDays(1);
                if (current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current=current.AddDays(1);
                }
                else if (current.DayOfWeek == DayOfWeek.Saturday)
                {
                    current=current.AddDays(2);
                }

                date = current.Year + "-" + current.Month.ToString("00") + "-" + current.Day.ToString("00");

               
                try {
                    rows = GetCalanderData(date);
                }catch(Exception )
                {
                    rows = new List<Row>();
                }
                

                count++;
            }

            return new JsonResult(true);
        }

        [HttpGet("pullnysesharedata")]
        public async Task<JsonResult> PullNYSEShareData()
        {
            List<Dividend> dividends = _context.Dividends.Where(x => x.DividendExDate > DateTime.Now && x.Market == "NYSE").ToList();

            foreach (Dividend dividend in dividends)
            {

                NyseData share;

                try
                {
                    share = GetNYSEShareData(dividend.Symbol);

                    dividend.UpdateCurrentPrice(share.candles[0].close);

                    _context.Dividends.Attach(dividend);
                    _context.Entry(dividend).Property(x => x.DividendPercent).IsModified = true;
                    _context.Entry(dividend).Property(x => x.CurrentSharePrice).IsModified = true;

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                }


            }

            return new JsonResult(true);
        }

        [HttpGet("pullnasdaqsharedata")]
        public async Task<JsonResult> PullNASDAQShareData()
        {
            List<Dividend> dividends = _context.Dividends.Where(x => x.DividendExDate > DateTime.Now && x.Market == "NASDAQ").ToList();

            foreach(Dividend dividend in dividends) {

                ShareData share;

                try {
                    share = GetNASDAQShareData(dividend.Symbol);

                    dividend.UpdateCurrentPrice(share.primaryData.lastSalePrice);

                    _context.Dividends.Attach(dividend);
                    _context.Entry(dividend).Property(x => x.DividendPercent).IsModified = true;
                    _context.Entry(dividend).Property(x => x.CurrentSharePrice).IsModified = true;

                    await _context.SaveChangesAsync();
                }
                catch(Exception)
                {

                }


            }

            return new JsonResult(true);
        }




        private ShareData GetNASDAQShareData(string tickersymbol)
        {
            var client = new RestClient("https://api.nasdaq.com/api/quote/" + tickersymbol + "/");
            var request = new RestRequest("info?assetclass=stocks", Method.Get);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.32.2");

            var queryResult = client.Execute(request);

            ShareRoot myDeserializedClass = JsonConvert.DeserializeObject<ShareRoot>(queryResult.Content);

            if (myDeserializedClass.data == null)
                throw new Exception();

            return myDeserializedClass.data;
        }


        private NyseData GetNYSEShareData(string tickersymbol)
        {
            var client = new RestClient("https://data-api-next.benzinga.com/rest/v2/");
            var request = new RestRequest("chart?apikey=81DC3A5A39D6D1A9D26FA6DF35A34&from=5m&interval=1d&symbol=" + tickersymbol, Method.Get);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.32.2");

            var queryResult = client.Execute(request);

            NyseData myDeserializedClass = JsonConvert.DeserializeObject<NyseData>(queryResult.Content);

            if (myDeserializedClass == null || myDeserializedClass.candles.Count == 0)
                throw new Exception();

            return myDeserializedClass;
        }

        private List<Row> GetCalanderData(string date)
        {
            //

            var client = new RestClient("https://api.nasdaq.com/api/calendar/");

            var request = new RestRequest("dividends?date=" + date, Method.Get);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.32.2");

            //request.OnBeforeDeserialization = resp => { resp.Headers. };

            var queryResult = client.Execute(request);

            CalanderRoot myDeserializedClass;

            try
            {
                myDeserializedClass = JsonConvert.DeserializeObject<CalanderRoot>(queryResult.Content);

                if (myDeserializedClass.data == null)
                    return new List<Row>();
            }
            catch (Exception)
            {
                throw;
            }

            return myDeserializedClass.data.calendar.rows;
        }


    }
}
