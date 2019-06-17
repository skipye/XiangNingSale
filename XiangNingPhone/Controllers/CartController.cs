using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiangNingPhone.Controllers
{
    public class CartController : BaseController
    {
        private static readonly NewsService NSer = new NewsService();
        private static readonly AddressService AdSer = new AddressService();
        public ActionResult AddToCart(int ProductId, int Amount = 1)
        {
            int Count = 1;
            var product = NSer.GetDetailById(ProductId);

            // 驗證產品是否存在
            if (product == null)
                return HttpNotFound();

            var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == ProductId);
            if (existingCart != null)
            {
                existingCart.Amount += 1;
                Count = existingCart.Amount;
            }
            else
            {
                if (this.Carts != null)
                {
                    foreach (var item in this.Carts)
                    {
                        Count += item.Amount;
                    }
                }
                this.Carts.Add(new CartModel() { Product = product, Amount = Amount });
            }
            return Content(Count.ToString());
        }
        public ActionResult Index()
        {
            return View(this.Carts);
        }

        // 移除購物車項目
        [HttpPost] // 因為知道要透過 AJAX 呼叫這個 Action，所以可以先標示 [HttpPost] 屬性
        public ActionResult Remove(int ProductId)
        {
            var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == ProductId);
            if (existingCart != null)
            {
                this.Carts.Remove(existingCart);
            }

            return Content("1");
        }

        // 更新購物車中特定項目的購買數量
        [HttpPost] // 因為知道要透過 AJAX 呼叫這個 Action，所以可以先標示 [HttpPost] 屬性
        public ActionResult UpdateAmount(int ProductId, int Amount)
        {
            Double CrrPrice = 0;
            //foreach (var item in Carts)
            //{
            var existingCart = this.Carts.FirstOrDefault(p => p.Product.Id == ProductId);
            if (existingCart != null)
            {
                existingCart.Amount = Amount;
                CrrPrice = existingCart.Amount * Convert.ToDouble(existingCart.Product.SalePrice);
            }
            //}
            return Content(CrrPrice.ToString("0.00"));
            //return RedirectToAction("Index", "Cart");
        }
        public ActionResult CartOrder(int? AddId)
        {
            Guid UserId = Guid.Empty;
            CartAddressModel models = new CartAddressModel();
            if (Session["User"] !=null)
            {
                string UserModel = Session["User"].ToString();
                UserId = new Guid(UserModel.Split('|')[1]);
            }if (UserId == Guid.Empty)
            {
                return RedirectToAction("Login","Account",new { ReturnUrl= "/Cart/CartOrder" });
            }
            if (AddId > 0)
            {
                models.AddModel = AdSer.GetAddressDetailById(AddId.Value);
            }
            else { models.AddModel = AdSer.GetTop1Address(UserId); }
            if (this.Carts != null)
            {
                models.CartModel = this.Carts;
            }
            else { return RedirectToAction("Index", "Home"); }
            //models.UserModel = USer.GetUserDetail(UserId);
            //models.ShangPinTolit = 0;
            //models.YLGole = models.UserModel.Gold ?? 0;
            //models.YLStock = models.UserModel.Stock ?? 0;
            return View(models);

        }

    }
}
