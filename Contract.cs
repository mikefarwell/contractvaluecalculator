using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractValueCalculator
{
    public class Contract
    {
        public string Name { get; set; }
        public double WageIncrease { get; set; }
        public List<JobClass> JobClasses { get; set; }
        public double FlatBonus { get; set; }
        public double PercentageBonus { get; set; }

        public double GetSalary(Employee employee, int year, DateTime effectiveDate)
        {
            // Find the matching job class for the employee
            JobClass jobClass = JobClasses.FirstOrDefault(c => c.Name == employee.JobClassName && c.Grade == employee.JobClassGrade);
            if (jobClass == null)
            {
                throw new Exception($"No job class found for employee {employee.Name} with job class name {employee.JobClassName} and grade {employee.JobClassGrade}");
            }

            // Calculate the salary based on the job class and the wage increase
            double salary = jobClass.GetSalary(year, effectiveDate);
            double wageIncrease = WageIncrease;
            if (salary + wageIncrease < jobClass.MinimumSalary)
            {
                wageIncrease = jobClass.MinimumSalary - salary;
            }
            else if (salary + wageIncrease > jobClass.MaximumSalary)
            {
                wageIncrease = jobClass.MaximumSalary - salary;
            }
            salary += wageIncrease;

            // Apply flat bonus
            salary += FlatBonus;

            // Apply percentage bonus
            salary += salary * (PercentageBonus / 100);

            return salary;
        }

        public double GetTotalValue(List<Employee> employees, DateTime effectiveDate)
        {
            double totalValue = 0;
            foreach (Employee employee in employees)
            {
                for (int i = 1; i <= employee.ContractLength; i++)
                {
                    double salary = GetSalary(employee, i, effectiveDate);
                    double bonus = 0;
                    if (salary > employee.MaximumSalary)
                    {
                        bonus = salary - employee.MaximumSalary;
                        salary = employee.MaximumSalary;
                    }
                    totalValue += salary + bonus + employee.VacationDays + employee.SickDays + employee.PersonalDays;
                }
            }

            return totalValue;
        }
    }
}
