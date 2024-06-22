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
    public class BooksTablesController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();

        // GET: BooksTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

  
            var booksTables = db.BooksTables.Include(b => b.BookTypeTable).Include(b => b.DepartmentsTable).Include(b => b.UserTable);
            return View(booksTables.ToList());
        }

        // GET: BooksTables/Details/5
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
            BooksTable booksTable = db.BooksTables.Find(id);
            if (booksTable == null)
            {
                return HttpNotFound();
            }
            return View(booksTable);
        }

        // GET: BooksTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name","0");
            ViewBag.DepartmentID = new SelectList(db.DepartmentsTables, "DepartmentID", "Name", "0");
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", "0");
            return View();
        }

        // POST: BooksTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooksTable booksTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            booksTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.BooksTables.Add(booksTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", booksTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentsTables, "DepartmentID", "Name", booksTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", booksTable.UserID);
            return View(booksTable);
        }

        // GET: BooksTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

       
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksTable booksTable = db.BooksTables.Find(id);
            if (booksTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", booksTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentsTables, "DepartmentID", "Name", booksTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", booksTable.UserID);
            return View(booksTable);
        }

        // POST: BooksTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,UserID,DepartmentID,BookTypeID,BookTitle,ShortDescription,Author,BookName,Edition,TotalCopies,RegDate,Price,Description")] BooksTable booksTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            booksTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.Entry(booksTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypeTables, "BookTypeID", "Name", booksTable.BookTypeID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentsTables, "DepartmentID", "Name", booksTable.DepartmentID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", booksTable.UserID);
            return View(booksTable);
        }

        // GET: BooksTables/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BooksTable booksTable = db.BooksTables.Find(id);
        //    if (booksTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(booksTable);
        //}

        //// POST: BooksTables/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BooksTable booksTable = db.BooksTables.Find(id);
        //    db.BooksTables.Remove(booksTable);
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
