using BanHangOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Role
        public ActionResult Index()
        {
            var item = db.Roles.ToList();
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
              
                if (!roleManager.RoleExists(model.Name))
                {
                    var roleResult = roleManager.Create(new IdentityRole(model.Name));
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi tạo vai trò");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vai trò đã tồn tại");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            var item = db.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                if (!roleManager.RoleExists(model.Name))
                {
                    var roleResult = roleManager.Update(new IdentityRole(model.Name));
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi tạo vai trò");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vai trò đã tồn tại");
                }
            }
            return View(model);
        }
    }
}
