using AssignmentTask.Models;
using AssignmentTask.Service;
using AssignmentTask.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssignmentTask.Controllers
{
    public class HomeController : Controller
    {
        InvoiceDb db = new InvoiceDb();
        OrderService service = new OrderService();
        public ActionResult Index()
        {
            var result = (from cus in db.Customers
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
            return View(result);
        }

        public JsonResult GetProductList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var productList = db.Products.ToList();
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public double GetPriceById(dynamic id)
        {
            if (Convert.ToDouble(id[0]) > 0)
            {
                var price = db.Products.Find(Convert.ToDouble(id[0])).Price;
                return price;
            }
            return -1;

        }
        public JsonResult SaveOrder(string gTotal, string paid, User[] user, OrderItemViewModel[] order)
        {
            if(gTotal!="" && paid!="" && user!=null && order != null)
            {
                try
                {

                    List<OrderItemViewModel> Orders = order.ToList();
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
                }
                catch (Exception e)
                {

                    throw;
                }
            }
           
            else return Json("Please Fill Up All Properties", JsonRequestBehavior.AllowGet);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        public ActionResult viewDeails(int id)
        {

            var details = service.GetDetailsById(Convert.ToInt32(id));
            return View(details);
        }

        public ActionResult DeleteSales(int id)
        {
            var orderItems= db.OrderItems.Where(x => x.OrderId == id).ToList();
            foreach (var item in orderItems)
            {
                db.OrderItems.Remove(item);
            }
            var order= db.Orders.Find(id);
            if (order != null) db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Edit(string orderId)
        {
            var details= service.GetDetailsById(Convert.ToInt32(orderId));

            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}