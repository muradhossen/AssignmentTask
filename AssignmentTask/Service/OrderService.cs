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
        public int SaveCustomer(string gTotal, string paid, User[] user)
        {
            if (user.Length==0) return -1;
            try
            {               
                //Customer Table
                User vUser = new User()
                {
                    name = user[0].name,
                    address = user[0].address,
                    phone = user[0].phone
                };

                Customer mCustomer = new Customer()
                {
                    Name = vUser.name,
                    Address = vUser.address,
                    Phone = vUser.phone
                };
                db.Customers.Add(mCustomer);
                db.SaveChanges();
                var cId = db.Customers.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                if (cId < 0) return -1;
                return cId;
               
            }
            catch (Exception)
            {

                throw;
            }
             
        }
        public int SaveOrders(string gTotal, string paid, int cId)
        {
            if (gTotal == "" || paid == "" || cId < 0)
                return -1;
            //Order Table
            Order mOrder = new Order
            {
                CustomerId = cId,
                Paid = Convert.ToInt32(paid),
                Total = Convert.ToInt32(gTotal)
            };
            db.Orders.Add(mOrder);
            db.SaveChanges();

             var oId = db.Orders.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            return oId;
        }
        public bool SaveOrderItem(List<OrderItemViewModel> Orders, int oId)
        {
            if (Orders.Count == 0 || oId < 0) return false;
            //Order Item Table
            foreach (var item in Orders)
            {
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.itemId,
                    OrderId = oId,
                    Quantity = item.quantity,
                };
                db.OrderItems.Add(orderItem);
            }
            db.SaveChanges();
            return true;
        }
        public List<SalesViewModel> GetAll()
        {
            return (from cus in db.Customers
                    join o in db.Orders on cus.Id equals o.CustomerId
                    select new SalesViewModel
                    {
                        OrderId = o.Id,
                        Name = cus.Name,
                        Phone = cus.Phone,
                        Address = cus.Address,
                        Total = (double)o.Total,
                        Paid = o.Paid,
                        Due = (double)o.Total - o.Paid
                    }).ToList();
        }
        public bool DeleteSales()
        {
            return true;
        }
    }
}