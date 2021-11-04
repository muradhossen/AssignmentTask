using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentTask.ViewModel
{
    public class SalesViewModel
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Total { get; set; }
        public int? Paid { get; set; }
        public double? Due { get; set; }
    }
}