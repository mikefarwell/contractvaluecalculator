namespace ContractValueCalculator
{
    public class Job
    {
        public string Classification { get; set; }
        public string Grade { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }

        public override string ToString()
        {
            return $"{Classification} ({Grade}) - {MinSalary:C} to {MaxSalary:C}";
        }
    }
}
