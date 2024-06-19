using BanHangOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace BanHangOnline.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var item = db.Orders.OrderByDescending(x => x.CreateDate).ToList();
            if(page == null)
            {
                page = 1;
            };
            var pageNumber = page ?? 1;
            var pageSize = 15;

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(item.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult View(int id)
        {
            var item = db.Orders
        .Include("OrderDetails.Product") // Include related OrderDetails and Products
        .FirstOrDefault(o => o.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }
        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.typePayment= status;
                db.Entry(item).Property(x=>x.typePayment).IsModified=true;
                db.SaveChanges();
                return Json(new { message = "Thành Công", Success = true });
            }
            return Json(new {message = "Thất bại", Success=false});
        }
    }
}