using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractValueCalculator
{
    public class ContractValueCalculator
    {
        private readonly List<Employee> _employees;
        private readonly List<Contract> _contracts;

        public ContractValueCalculator(List<Employee> employees, List<Contract> contracts)
        {
            _employees = employees;
            _contracts = contracts;
        }

        public void Calculate()
        {
            foreach (var contract in _contracts)
            {
                Console.WriteLine($"Calculating contract {contract.Name}");

                var contractSummary = new ContractSummary(contract);

                for (var year = 1; year <= contract.Years; year++)
                {
                    Console.WriteLine($"Year {year}");

                    foreach (var employee in _employees)
                    {
                        var job = contract.Jobs.FirstOrDefault(j => j.Title == employee.JobTitle);

                        if (job != null)
                        {
                            var salaryGrade = job.SalaryGrades.FirstOrDefault(sg =>
                                sg.Classification == employee.Classification &&
                                sg.Grade == employee.Grade);

                            if (salaryGrade != null)
                            {
                                var salary = salaryGrade.GetSalary(year);

                                if (employee.Salary < salary)
                                {
                                    var increase = new SalaryIncrease(employee.Salary, salary, year);

                                    contractSummary.AddIncrease(increase);
                                    employee.SetSalary(salary, year);

                                    Console.WriteLine(
                                        $"{employee.Name}: Increased salary from {increase.OldSalary:C} to {increase.NewSalary:C}");

                                    if (salary > salaryGrade.Maximum)
                                    {
                                        var difference = salary - salaryGrade.Maximum;
                                        contractSummary.AddBonus(difference);
                                        Console.WriteLine(
                                            $"{employee.Name}: Exceeded maximum salary, receiving bonus of {difference:C}");
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"\nContract {contract.Name} summary:\n{contractSummary}\n");
            }
        }
    }
}
