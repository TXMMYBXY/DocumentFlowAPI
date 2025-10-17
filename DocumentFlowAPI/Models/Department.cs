using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentFlowAPI.Models
{
    public class Department : EntityBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}