using BanHangOnline.Models.EF;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanHangOnline.Models;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext _dbConnet = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var item =  _dbConnet.Categories.ToList();
            return View(item);
        }
        public ActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model) 
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                
                _dbConnet.Categories.Add(model);
                _dbConnet.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        public ActionResult Edit(int id) 
        {
            var item = _dbConnet.Categories.Find(id);
            return View(item);  
        }
        public ActionResult EditCategory(Category model)
        {   
            if (ModelState.IsValid)
            {
                _dbConnet.Categories.Attach(model);

                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConnet.Entry(model).Property(x => x.Title).IsModified = true ;
                _dbConnet.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.SeoKeyWords).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.Position).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _dbConnet.Entry(model).Property(x => x.ModifierBy).IsModified = true;
                _dbConnet.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = _dbConnet.Categories.Find(id);
            if(item != null)
            {
               
                _dbConnet.Categories.Remove(item);
                _dbConnet.SaveChanges();
                return Json(new { success=true });
            }
            return Json(new { success = false });
        }
    }
}