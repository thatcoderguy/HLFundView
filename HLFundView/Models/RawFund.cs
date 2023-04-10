using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HLFundView.Models
{
    public class Result
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public string sedol { get; set; }
        public string citicode { get; set; }
        public string full_description { get; set; }
        public string ismininv_monthly { get; set; }
        public string reg_saver { get; set; }
        public string closed_fund { get; set; }
        public string fund_name { get; set; }
        public string aims { get; set; }
        public string description { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
        public string sector_id { get; set; }
        public string sector_name { get; set; }
        public string unit_type { get; set; }
        public string full_description_with_unit_type { get; set; }
        public string initial_charge { get; set; }
        public string annual_charge { get; set; }
        public string vantage_charge { get; set; }
        public string net_annual_charge { get; set; }
        public string initial_saving { get; set; }
        public string annual_saving { get; set; }
        public string holding_fee { get; set; }
        public string loaded { get; set; }
        public string linksedol_code { get; set; }
        public string restrict_ot { get; set; }
        public string payment_frequency { get; set; }
        public string payment_type { get; set; }
        public string Wealth150 { get; set; }
        public string trustee { get; set; }
        public string assumed_growth { get; set; }
        public string assumed_growth2 { get; set; }
        public string valuation_point { get; set; }
        public string comment { get; set; }
        public string tracker { get; set; }
        public string yieldreduct { get; set; }
        public string yieldreduct2 { get; set; }
        public string launch_price { get; set; }
        public string launch_currency { get; set; }
        public string num_holdings { get; set; }
        public string icvc { get; set; }
        public string sicav { get; set; }
        public string plusfund { get; set; }
        public string standard_amc { get; set; }
        public string standard_ocf { get; set; }
        public string total_expenses { get; set; }
        public string charge_source { get; set; }
        public string valuation_frequency { get; set; }
        public string val_info { get; set; }
        public string lsmininv { get; set; }
        public string yield { get; set; }
        public string kiid { get; set; }
        public string kiid_url { get; set; }
        public string running_yield { get; set; }
        public string historic_yield { get; set; }
        public string distribution_yield { get; set; }
        public string underlying_yield { get; set; }
        public string gross_yield { get; set; }
        public string gross_running_yield { get; set; }
        public string loyalty_bonus { get; set; }
        public string reg_saver_min_inv { get; set; }
        public string lump_sum_min_inv { get; set; }
        public string renewal_commission { get; set; }
        public string initial_commission { get; set; }
        public string fund_size { get; set; }
        public string risk_rating { get; set; }
        public string other_expenses { get; set; }
        public string cost_segment { get; set; }
        public string update_time { get; set; }
        public string ref_date { get; set; }
        public string perf3m { get; set; }
        public string perf6m { get; set; }
        public string perf12m { get; set; }
        public string perf36m { get; set; }
        public string perf60m { get; set; }
        public string perf120m { get; set; }
        public string perf0t12m { get; set; }
        public string perf12t24m { get; set; }
        public string perf24t36m { get; set; }
        public string perf36t48m { get; set; }
        public string perf48t60m { get; set; }
        public string perf3m_reinv { get; set; }
        public string perf6m_reinv { get; set; }
        public string perf12m_reinv { get; set; }
        public string perf36m_reinv { get; set; }
        public string perf60m_reinv { get; set; }
        public string perf120m_reinv { get; set; }
        public string perf0t12m_reinv { get; set; }
        public string perf12t24m_reinv { get; set; }
        public string perf24t36m_reinv { get; set; }
        public string perf36t48m_reinv { get; set; }
        public string perf48t60m_reinv { get; set; }
        public string ter { get; set; }
        public string is_unit_trust { get; set; }
        public string is_oeic { get; set; }
        public string isaable { get; set; }
        public string sippable { get; set; }
        public string unwrapable { get; set; }
        public string launchdate { get; set; }
        public double bid_price { get; set; }
        public double offer_price { get; set; }
        public string price_change { get; set; }
        public string percent_change { get; set; }
        public string updated { get; set; }
        public string investment_pathway { get; set; }
    }


}
