using BitMiracle.Docotic.Pdf;
using HLFundView.Data;
using HLFundView.Models;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Globalization;
using System.Net;

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

        [HttpGet("pulldividendcalander")]
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

                        Dividend div = new Dividend(row.symbol, row.companyName, row.dividend_Rate, row.dividend_Ex_Date);

                        if (_context.Dividends.Any(e => e.Symbol == row.symbol && e.DividendExDate == DateTime.Parse(row.dividend_Ex_Date, new CultureInfo("en-US", false))))
                        {
                            _context.Dividends.Attach(div);
                            _context.Entry(div).Property(x => x.DividendAmount).IsModified = true;
                        }
                        else
                        {
                            _context.Dividends.Add(div);
                        }

                        await _context.SaveChangesAsync();
                    }
                }


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
                rows = GetCalanderData(date);

                count++;
            }

            return new JsonResult(true);
        }



        [HttpGet("pullsharedata")]
        public async Task<JsonResult> PullShareData()
        {
            List<Dividend> dividends = _context.Dividends.Where(x => x.DividendExDate > DateTime.Now).ToList();

            foreach(Dividend dividend in dividends) {
                ShareData share = GetShareData(dividend.Symbol);

                dividend.UpdateCurrentPrice(share.primaryData.lastSalePrice);

                _context.Dividends.Attach(dividend);
                _context.Entry(dividend).Property(x => x.DividendPercent).IsModified = true;
                _context.Entry(dividend).Property(x => x.CurrentSharePrice).IsModified = true;

                await _context.SaveChangesAsync();
            }

            return new JsonResult(true);
        }

        private ShareData GetShareData(string tickersymbol)
        {
            var client = new RestClient("https://api.nasdaq.com/api/quote/" + tickersymbol + "/");
            var request = new RestRequest("info?assetclass=stocks", Method.Get);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.32.2");

            var queryResult = client.Execute(request);

            ShareRoot myDeserializedClass = JsonConvert.DeserializeObject<ShareRoot>(queryResult.Content);

            return myDeserializedClass.data;
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

            CalanderRoot myDeserializedClass = JsonConvert.DeserializeObject<CalanderRoot>(queryResult.Content);

            return myDeserializedClass.data.calendar.rows;
        }


    }
}
