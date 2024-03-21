using PagedList;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Product


        private PCModel db = new PCModel();
        public ActionResult Index()
        {

            return View(db.Products.ToList());
        }


        //public ActionResult Index(int? page)
        //{
        //    var context = new Model1();
        //    int pageSize = 4;
        //    int pageIndex = page.HasValue ? page.Value : 1;
        //    var result = context.Books.ToList().ToPagedList(pageIndex, pageSize);
        //    return View(result);
        //}


        //public ActionResult ShopFull()
        //{
        //    return View(db.Books.ToList());
        //}

        public ActionResult ShopFull(int? page)
        {
            PCModel context = new PCModel();
            int pageSize = 8;
            int pageIndex = page.HasValue ? page.Value : 1;
            var result = context.Products.ToList().ToPagedList(pageIndex, pageSize);
            return View(result);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult GetProductByCategory(int id)
        {
            PCModel context = new PCModel();
            return View("Index", context.Products.Where(p => p.Categoryid == id).ToList().ToPagedList(1, 10));
        }

        public ActionResult GetCategory()
        {
            PCModel context = new PCModel();
            var listCategory = context.Categories.ToList();
            return PartialView(listCategory);
        }
        public ActionResult Search(string searchString)
        {
            var context = new PCModel();
            var results = (from m in context.Products where m.Title.Contains(searchString) select m).ToList();
            return View("Index", results.ToPagedList(1, 10));
        }
       /* public ActionResult RemoveCategory()
        {
            
        }*/
    }
}