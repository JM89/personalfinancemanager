namespace PFM.Models
{
	public class IncomeListModel
	{
        public int Id { get; set; }

        public string AccountCurrencySymbol { get; set; }

        public decimal Cost { get; set; }

        public string Description { get; set; }

        public string DisplayedCost
        {
            get
            {
                return AccountCurrencySymbol + String.Format("{0:0.00}", this.Cost);
            }
        }

        public DateTime DateIncome { get; set; }

        public string DisplayedDateIncome
        {
            get
            {
                return this.DateIncome.ToString("yyyy-MM-dd");
            }
        }
    }
}

