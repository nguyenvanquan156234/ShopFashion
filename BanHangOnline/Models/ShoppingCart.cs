using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models
{
    public class ShoppingCart
    {
        // Định nghĩa thuộc tính Items bên trong lớp ShoppingCart
        public List<ShoppingCartItem> Items { get; set; }

        // Constructor của ShoppingCart
        public ShoppingCart()
        {
            // Khởi tạo danh sách Items
            this.Items = new List<ShoppingCartItem>();
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.ProductQuantity += Quantity;
                checkExits.TotalPrice = checkExits.ProductPrice * checkExits.ProductQuantity;
            }
            else
            {
                Items.Add(item);
            }

            // Lưu giỏ hàng vào session sau khi thêm sản phẩm
            HttpContext.Current.Session["Cart"] = this;
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }

            // Lưu giỏ hàng vào session sau khi xóa sản phẩm
            HttpContext.Current.Session["Cart"] = this;
        }

        // Cập nhật số lượng của sản phẩm trong giỏ hàng
        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.ProductQuantity = quantity;
                checkExits.TotalPrice = checkExits.ProductPrice * checkExits.ProductQuantity;
            }

            // Lưu giỏ hàng vào session sau khi cập nhật số lượng
            HttpContext.Current.Session["Cart"] = this;
        }

        // Lấy số lượng của một sản phẩm trong giỏ hàng
        public int GetQuantity(int productId)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                return item.ProductQuantity;
            }
            return 0;
        }

        // Lấy tổng số lượng các sản phẩm trong giỏ hàng
        public int GetQuantityTotal()
        {
            return Items.Sum(x => x.ProductQuantity);
        }

        // Lấy tổng số tiền của các sản phẩm trong giỏ hàng
        public decimal GetTotalAmount()
        {
            return Items.Sum(x => x.TotalPrice);
        }

        // Xóa toàn bộ sản phẩm khỏi giỏ hàng
        public void ClearCart()
        {
            Items.Clear();

            // Lưu giỏ hàng vào session sau khi xóa toàn bộ sản phẩm
            HttpContext.Current.Session["Cart"] = this;
        }

    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Alias { get; set; }
        public string ProductImg { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
    