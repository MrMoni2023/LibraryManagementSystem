using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace LibraryManagementSystem.Controllers
{
    public class BookTypeTablesController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();

        // GET: BookTypeTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var bookTypeTables = db.BookTypeTables.Include(b => b.UserTable);
            return View(bookTypeTables.ToList());
        }

        // GET: BookTypeTables/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTypeTable bookTypeTable = db.BookTypeTables.Find(id);
            if (bookTypeTable == null)
            {
                return HttpNotFound();
            }
            return View(bookTypeTable);
        }

        // GET: BookTypeTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName");
            return View();
        }

        // POST: BookTypeTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookTypeTable bookTypeTable)
        {
            //[Bind(Include = "BookTypeID,Name,UserID")]
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            bookTypeTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.BookTypeTables.Add(bookTypeTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTypeTable.UserID);
            return View(bookTypeTable);
        }

        // GET: BookTypeTables/Edit/5
        public ActionResult Edit(int? id)
        {//[Bind(Include = "BookTypeID,Name,UserID")]
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTypeTable bookTypeTable = db.BookTypeTables.Find(id);
            if (bookTypeTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTypeTable.UserID);
            return View(bookTypeTable);
        }

        // POST: BookTypeTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookTypeTable bookTypeTable)
        {
            //[Bind(Include = "BookTypeID,Name,UserID")]
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            bookTypeTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.Entry(bookTypeTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookTypeTable.UserID);
            return View(bookTypeTable);
        }

        // GET: BookTypeTables/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    //[Bind(Include = "BookTypeID,Name,UserID")]
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BookTypeTable bookTypeTable = db.BookTypeTables.Find(id);
        //    if (bookTypeTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bookTypeTable);
        //}

        //// POST: BookTypeTables/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    //[Bind(Include = "BookTypeID,Name,UserID")]
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    BookTypeTable bookTypeTable = db.BookTypeTables.Find(id);
        //    db.BookTypeTables.Remove(bookTypeTable);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
