using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumeEmployeeApi.Models
{
    public partial class Emp
    {
        public int Empid { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int? Salary { get; set; }
    }
}
