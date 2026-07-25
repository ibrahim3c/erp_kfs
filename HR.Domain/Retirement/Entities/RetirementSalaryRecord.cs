using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Entities
{

    public class RetirementSalaryRecord 
    {
        public int Year { get; private set; }
        public decimal BasicInsuredSalary { get; private set; }

        private RetirementSalaryRecord() { }
        private RetirementSalaryRecord(int year, decimal basicInsuredSalary)
        {
            Year = year;
            BasicInsuredSalary = basicInsuredSalary;
        }

        public static RetirementSalaryRecord Create(int year, decimal basicInsuredSalary) => new(year, basicInsuredSalary);
        public void UpdateAmount(decimal basicInsuredSalary) => BasicInsuredSalary = basicInsuredSalary;
    }
}
