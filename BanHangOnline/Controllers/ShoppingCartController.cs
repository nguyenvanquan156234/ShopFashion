using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult CheckOut()
        {

            return View();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                int totalQuantity = cart.Items.Sum(x => x.ProductQuantity);
                return Json(new { Count = totalQuantity }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuccessCO()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            // Initialize the response object
            var response = new { Success = false, Message = "Đặt hàng thất bại", OrderCode = -1 };

            // Check if the ModelState is valid
            if (ModelState.IsValid)
            {
                // Retrieve the shopping cart from the session
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    // Create a new Order object
                    Order order = new Order
                    {
                        CustomerName = req.CustomerName,
                        Phone = req.Phone,
                        Address = req.Address,
                        typePayment = req.typePayment,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreateBy = req.Phone,
                        Email = req.Email
                    };

                    // Add OrderDetails for each item in the cart
                    foreach (var item in cart.Items)
                    {
                        order.OrderDetails.Add(new OrderDetail
                        {
                            ProductID = item.ProductId,
                            Quantity = item.ProductQuantity,
                            Price = item.ProductPrice
                        });
                    }

                    // Calculate the total amount
                    order.TotalAmount = cart.GetTotalAmount();

                    // Generate a random order code
                    Random random = new Random();
                    order.Code = random.Next(1000, 10000); // Generate a random 4-digit number

                    // Add the order to the database and save changes
                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Prepare email content
                    var strSanPham = new System.Text.StringBuilder();
                    decimal thanhtien = 0;
                    decimal tongTien = 0;

                    foreach (var sp in cart.Items)
                    {
                        strSanPham.AppendLine("<tr>");
                        strSanPham.AppendLine($"<td>{sp.ProductName}</td>");
                        strSanPham.AppendLine($"<td>{sp.ProductQuantity}</td>"); // Retrieve the quantity for each product
                        strSanPham.AppendLine($"<td>{sp.ProductPrice}</td>");
                        strSanPham.AppendLine("</tr>");
                        thanhtien += sp.ProductPrice * sp.ProductQuantity;
                    }

                    tongTien = thanhtien;

                    // Format the date explicitly
                    string formattedDate = order.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code.ToString());
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham.ToString());
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChi}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", formattedDate);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", thanhtien.ToString("N0")); // Formatting the number
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", tongTien.ToString("N0")); // Formatting the number

                    BanHangOnline.Models.Common.SendMail.SendMailer("ShopOnline", "Đơn hàng#" + order.Code.ToString(), contentCustomer, order.Email);

                    // Prepare email content for admin
                    string formattedDateAdmin = order.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

                    string adminContent = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
                    adminContent = adminContent.Replace("{{MaDon}}", order.Code.ToString());
                    adminContent = adminContent.Replace("{{SanPham}}", strSanPham.ToString());
                    adminContent = adminContent.Replace("{{TenKhachHang}}", order.CustomerName);
                    adminContent = adminContent.Replace("{{Phone}}", order.Phone);
                    adminContent = adminContent.Replace("{{Email}}", order.Email);
                    adminContent = adminContent.Replace("{{DiaChi}}", order.Address);
                    adminContent = adminContent.Replace("{{NgayDat}}", formattedDateAdmin);
                    adminContent = adminContent.Replace("{{ThanhTien}}", thanhtien.ToString("N0")); // Formatting the number
                    adminContent = adminContent.Replace("{{TongTien}}", tongTien.ToString("N0")); // Formatting the number
                    var emailAdmin = ConfigurationManager.AppSettings["EmailAdmin"];
                    BanHangOnline.Models.Common.SendMail.SendMailer("ShopOnline", "Đơn hàng Mới#" + order.Code.ToString(), adminContent, emailAdmin); // Replace with actual admin email

                    // Clear the cart after successful order placement
                    cart.ClearCart();

                    // Set the response for successful order placement
                    return RedirectToAction("SuccessCO");
                }
            }

            // Return the JSON response
            return Json(response);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }

                // Check if the product already exists in the cart
                var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (existingItem != null)
                {
                    // Increment the quantity
                    existingItem.ProductQuantity += quantity;
                    existingItem.TotalPrice = existingItem.ProductPrice * existingItem.ProductQuantity;
                   
                }
                else
                {
                    ShoppingCartItem item = new ShoppingCartItem
                    {
                        ProductId = checkProduct.Id,
                        ProductName = checkProduct.Title,
                        Alias=checkProduct.Alias,
                        CategoryName = checkProduct.ProductCategory.Title,
                        ProductQuantity = quantity,
                        ProductPrice = checkProduct.Price
                    };

                    if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                    {
                        item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                    }

                    if (checkProduct.PriceSale > 0)
                    {
                        item.ProductPrice = (decimal)checkProduct.PriceSale;
                    }

                    item.TotalPrice = item.ProductPrice * item.ProductQuantity;
                    cart.AddToCart(item, quantity);
                }

                Session["Cart"] = cart;
                int totalQuantity = cart.Items.Sum(x => x.ProductQuantity);
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = totalQuantity };
            }
            else
            {
                code = new { Success = false, msg = "Sản phẩm không tồn tại!", code = 0, Count = 0 };
            }

            return Json(code);
        }
     
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x=>x.ProductId==id);
                if (checkProduct != null)
                {
                    cart.Items.Remove(checkProduct);
                    int totalQuantity = cart.Items.Sum(x => x.ProductQuantity);
                    code = new { Success = true, msg = "", code = 1, Count = totalQuantity };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {
                cart.ClearCart();
                return Json(new {Success= true});
            }

            return Json(new { Success = false });



        }
        [HttpPost]
        public ActionResult UpdateAll(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                // Lưu lại giỏ hàng vào Session sau khi cập nhật số lượng
                Session["Cart"] = cart;

                // Recalculate total amount
                decimal totalAmount = cart.GetTotalAmount();

                // Lấy số lượng của sản phẩm đã cập nhật trong giỏ hàng để trả về
                int updatedQuantity = cart.GetQuantity(id);

                return Json(new { Success = true, TotalAmount = totalAmount, UpdatedQuantity = updatedQuantity });
            }

            return Json(new { Success = false });
        }


    }

}
