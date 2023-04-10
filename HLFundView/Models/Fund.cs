using System.ComponentModel.DataAnnotations;

namespace HLFundView.Models
{
    public class Fund
    {
        [Key]
        public string sedol { get; set; }
        public string full_description { get; set; }
        public string fund_name { get; set; }
        public string description { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
        public UnitType unit_type { get; set; }
        public decimal running_yield { get; set; }
        public decimal historic_yield { get; set; }
        public decimal distribution_yield { get; set; }
        public decimal underlying_yield { get; set; }
        public decimal gross_yield { get; set; }
        public decimal gross_running_yield { get; set; }
        public string kiid_url { get; set; }

        public Fund()
        {

        }

        public Fund(string sedol, string full_description, string fund_name, string description, string company_id, string company_name,
    string unit_type, string running_yield, string historic_yield, string distribution_yield, string underlying_yield, string gross_yield,
    string gross_running_yield, string kiid_url)
        {
            this.sedol = sedol;
            this.kiid_url = kiid_url;
            this.full_description = full_description;
            this.fund_name = fund_name;
            this.description = description;
            this.company_id = company_id;
            this.company_name = company_name;
            this.unit_type = unit_type == "Accumulation" ? UnitType.Accumulation : UnitType.Income;
            this.running_yield = Convert.ToDecimal(running_yield);
            this.historic_yield = Convert.ToDecimal(historic_yield);
            this.distribution_yield = Convert.ToDecimal(distribution_yield);
            this.underlying_yield = Convert.ToDecimal(underlying_yield);
            this.gross_yield = Convert.ToDecimal(gross_yield);
            this.gross_running_yield = Convert.ToDecimal(gross_running_yield);
        }

    }


    public enum UnitType
    {
        Accumulation,
        Income
    }
}





