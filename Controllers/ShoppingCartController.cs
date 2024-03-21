using Microsoft.AspNet.Identity;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCartController

        public List<CartItem> GetShoppingCartFromSession()
        {
            var lstShoppingCart = Session["ShoppingCart"] as List<CartItem>;
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<CartItem>();
                Session["ShoppingCart"] = lstShoppingCart;
            }
            return lstShoppingCart;
        }

        [Authorize]
        public RedirectToRouteResult AddToCart(int id)
        {
            PCModel db = new PCModel();
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            CartItem findCardItem = ShoppingCart.FirstOrDefault(m => m.Id == id);
            if (findCardItem == null)
            {
                Product findProduct = db.Products.First(m => m.Id == id);
                CartItem newItem = new CartItem()
                {
                    Id = findProduct.Id,
                    Title = findProduct.Title,
                    Quantity = 1,
                    Image = findProduct.Image,
                    Price = findProduct.Price.Value
                };
                ShoppingCart.Add(newItem);
            }
            else
            {
                findCardItem.Quantity++;
            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult Index()
        {
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            //if(ShoppingCart.Count == 0)
            //{
            //    return RedirectToAction("Index", "Book");
            //}

            ViewBag.Tongsoluong = ShoppingCart.Sum(p => p.Quantity);
            ViewBag.Tongtien = ShoppingCart.Sum(p => p.Quantity * p.Price);

            return View(ShoppingCart);
        }

        public ActionResult UpdateCart(int id, int txtQuantity)
        {
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            CartItem findCart = ShoppingCart.Find(m => m.Id == id);
            if (findCart != null)
            {
                findCart.Quantity = txtQuantity;
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCart(int id)
        {
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            CartItem findCart = ShoppingCart.Find(m => m.Id == id);
            if (findCart != null)
            {
                ShoppingCart.Remove(findCart);
            }

            return RedirectToAction("Index");
        }

        public ActionResult CartSummary()
        {
            ViewBag.CartCount = GetShoppingCartFromSession().Count();
            return PartialView("CartSummary");
        }

        public ActionResult ConfirmOrder()
        {
            
            
          return View();
        }
        public ActionResult SelectedProduct()
        {
            return View();
        }

        public ActionResult Order()
        {
            string currentUserId = User.Identity.GetUserId();
            PCModel context = new PCModel();
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Order objOrder = new Order()
                    {
                        OrderNo = context.Orders.Count() + 1,
                        CustomerId = null,
                        OrderDate = DateTime.Now,
                        DeliveryDate = null,
                        IsPaid= false,
                        IsComplete=false
                    };
                    objOrder = context.Orders.Add(objOrder);
                    context.SaveChanges();

                    List<CartItem> listCartItems = GetShoppingCartFromSession();
                    foreach (var item in listCartItems)
                    {
                        OrderDetail ctdh = new OrderDetail()
                        {
                            OrderNo = objOrder.OrderNo,
                            ProductId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price,
                        };
                        context.OrderDetails.Add(ctdh);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Content(" Xay ra loi !!" + ex.Message);
                }
            }
            /* Session["Giohang"] = null;
             return RedirectToAction("SelectedProduct", "ShoppingCart");*/
            List<CartItem> cartItems = GetShoppingCartFromSession();
            if (cartItems.Count > 0)
            {
                return View("ConfirmOrder");
            }
            else
            {
                return RedirectToAction("SelectedProduct", "ShoppingCart");
            }

        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}