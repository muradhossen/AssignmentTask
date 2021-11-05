using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentTask.ViewModel
{
    public class DetailsViewModel
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Total { get; set; }
        public int? Paid { get; set; }
        public double? Due { get; set; }
        public List<DetailsItemViewModel> OrderItemList = new List<DetailsItemViewModel>();
    }
    public class DetailsItemViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double? Price { get; set; }
        public int Quantity { get; set; }
        public double? Total { get; set; }
    }
}