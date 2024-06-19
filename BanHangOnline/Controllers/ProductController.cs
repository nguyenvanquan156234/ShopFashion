using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index()
        {
            
            var items = db.Products.ToList();
            
            
            
                return View(items);
        }
        public ActionResult ProductCategory(string alias,int? id)
        {

            var items = db.Products.ToList();
            if (id  >0)
            {

                items = items.Where(x => x.ProductCategoryID == id).ToList();
            }
            ViewBag.cateId = id;
            return View(items);
        }
        public ActionResult Detail(string alias,int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }
        public ActionResult Partial_ItembyCateId()
        {
            var items = db.Products.Where(x => x.IsHome).Take(14).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale).Take(14).ToList();
            return PartialView(items);
        }
    }
}