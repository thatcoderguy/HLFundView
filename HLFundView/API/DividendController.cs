using HLFundView.Converter;
using HLFundView.Data;
using HLFundView.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Encodings.Web;

namespace HLFundView.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividendController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DividendController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getdividenddata")]
        public async Task<JsonResult> GetDividendData()
        {
            DividendData divs = new DividendData();
            divs.Dividends = _context.Dividends.Where(x => x.DividendExDate >= DateTime.Now).ToList();
            
            return new JsonResult(DivdendToViewDividend.Convert(divs.Dividends));
        }
     }
}
