using AssignmentTask.Models;
using AssignmentTask.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentTask.Service
{
    public class OrderService
    {
        InvoiceDb db = new InvoiceDb();
        public DetailsViewModel GetDetailsById(int id)
        {
            int intOId = Convert.ToInt32(id);
            DetailsViewModel vModel = new DetailsViewModel();

            if (intOId==0)
            {
                return vModel;
            }
            var customer = (from o in db.Orders
                            join cus in db.Customers on o.CustomerId equals cus.Id

                            select new SalesViewModel
                            {
                                OrderId = o.Id,
                                Name = cus.Name,
                                Phone = cus.Phone,
                                Address = cus.Address,
                                Total = (double)o.Total,
                                Paid = o.Paid,
                                Due = (double)o.Total - o.Paid
                            }).Where(x => x.OrderId == intOId).ToList();

            var orderItems = (from o in db.Orders
                              join oItem in db.OrderItems on o.Id equals oItem.OrderId
                              join pro in db.Products on oItem.ProductId equals pro.Id
                              where o.Id == intOId
                              select new
                              {
                                  OrderId = o.Id,
                                  ProductId = pro.Id,
                                  ProductName = pro.Name,
                                  Quantity = oItem.Quantity,
                                  Price = pro.Price
                              }).ToList();
           

            vModel.OrderId = customer.FirstOrDefault().OrderId;
            vModel.Name = customer.FirstOrDefault().Name;
            vModel.Address = customer.FirstOrDefault().Address;
            vModel.Phone = customer.FirstOrDefault().Phone;
            vModel.Total = customer.FirstOrDefault().Total;
            vModel.Paid = customer.FirstOrDefault().Paid;
            vModel.Due = customer.FirstOrDefault().Total - customer.FirstOrDefault().Paid;

            foreach (var item in orderItems)
            {
                DetailsItemViewModel itemViewModel = new DetailsItemViewModel();
                itemViewModel.ItemId = item.ProductId;
                itemViewModel.ItemName = item.ProductName;
                itemViewModel.Price = item.Price;
                itemViewModel.Quantity = item.Quantity;
                itemViewModel.Total = item.Price * item.Quantity;
                vModel.OrderItemList.Add(itemViewModel);
            }
            return vModel;
        }
    }
}