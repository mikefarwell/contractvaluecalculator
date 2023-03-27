using System;
using System.Collections.Generic;

namespace ContractValueCalculator
{
    public class SalaryGrade
    {
        public string Classification { get; set; }
        public string Grade { get; set; }
        public double MinimumSalary { get; set; }
        public double MaximumSalary { get; set; }
        public List<SalaryStep> SalarySteps { get; set; }

        public double GetSalaryForStep(int step)
        {
            if (step < 0 || step >= SalarySteps.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(step), $"Step must be between 0 and {SalarySteps.Count - 1}");
            }

            return SalarySteps[step].Salary;
        }
    }
}
