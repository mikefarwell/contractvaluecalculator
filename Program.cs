using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ContractValueCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please provide two contract files as arguments");
                Console.WriteLine("Usage: ContractValueCalculator <contract1.yaml> <contract2.yaml>");
                return;
            }

            // Load employee data
            var employees = LoadEmployees("employees.csv");

            // Load contract data
            var contract1 = LoadContract(args[0]);
            var contract2 = LoadContract(args[1]);

            // Calculate values for each contract
            var value1 = new ContractValueCalculator(contract1, employees).Calculate();
            var value2 = new ContractValueCalculator(contract2, employees).Calculate();

            // Output summaries
            Console.WriteLine("Summary of Contract 1:");
            Console.WriteLine(value1.Summary());
            Console.WriteLine();
            Console.WriteLine("Summary of Contract 2:");
            Console.WriteLine(value2.Summary());
        }

        static IEnumerable<Employee> LoadEmployees(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                return csv.GetRecords<Employee>().ToList();
            }
        }

        static Contract LoadContract(string path)
        {
            var input = File.ReadAllText(path);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<Contract>(input);
        }
    }
}
