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
            var result = service.GetAll();
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
            int oId = 0;
            if (gTotal != "" && paid != "" && user.Length > 0 && order.Length > 0)
            {
                try
                {
                    List<OrderItemViewModel> Orders = order.ToList();
                    int cId = service.SaveCustomer(gTotal, paid, user);
                    if (cId > 0) oId = service.SaveOrders(gTotal, paid, cId);
                    if (oId > 0) service.SaveOrderItem(Orders, oId);
                }
                catch (Exception e)
                {
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
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
            var orderItems = db.OrderItems.Where(x => x.OrderId == id).ToList();
            foreach (var item in orderItems)
            {
                db.OrderItems.Remove(item);
            }
            var order = db.Orders.Find(id);
            if (order != null) db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Edit(string orderId)
        {
            var details = service.GetDetailsById(Convert.ToInt32(orderId));

            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}