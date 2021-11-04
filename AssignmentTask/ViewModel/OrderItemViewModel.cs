using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentTask.ViewModel
{
    public class OrderItemViewModel
    {
        public int itemId { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double total { get; set; }

    }
}