using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Shop.Models;

namespace Shop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private PCModel db = new PCModel();

        // GET: Admin/Product
        [Authorize(Roles ="Admin")]
        //public ActionResult Index()
        //{
        //    var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
        //    return View(products.ToList());
        //}
        public ActionResult Index(int? page)
        {
            PCModel context = new PCModel();
            int pageSize = 5;
            //int pageIndex = (page == null) ? 1 : page.Value;
            int pageIndex = page.HasValue ? page.Value : 1;
            var results = context.Products.ToList().ToPagedList(pageIndex, pageSize);
            return View(results);
            //return View(context.Books.ToList());
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
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

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.Brandid = new SelectList(db.Brands, "Id", "BrandName");
            ViewBag.Categoryid = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Price,Description,Image,Categoryid,Brandid")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Brandid = new SelectList(db.Brands, "Id", "BrandName", product.Brandid);
            ViewBag.Categoryid = new SelectList(db.Categories, "Id", "CategoryName", product.Categoryid);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Brandid = new SelectList(db.Brands, "Id", "BrandName", product.Brandid);
            ViewBag.Categoryid = new SelectList(db.Categories, "Id", "CategoryName", product.Categoryid);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Price,Description,Image,Categoryid,Brandid")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brandid = new SelectList(db.Brands, "Id", "BrandName", product.Brandid);
            ViewBag.Categoryid = new SelectList(db.Categories, "Id", "CategoryName", product.Categoryid);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
