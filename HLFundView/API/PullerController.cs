using HLFundView.Data;
using HLFundView.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

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
           
            return new JsonResult(true);
        }
    }
}
