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
    public class BookReturnTablesController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();

        // GET: BookReturnTables
        public ActionResult Index()
        {
            var bookReturnTables = db.BookReturnTables.Include(b => b.BooksTable).Include(b => b.EmployeeTable).Include(b => b.UserTable);
            return View(bookReturnTables.ToList());
        }

        // GET: BookReturnTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReturnTable bookReturnTable = db.BookReturnTables.Find(id);
            if (bookReturnTable == null)
            {
                return HttpNotFound();
            }
            return View(bookReturnTable);
        }

        // GET: BookReturnTables/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle");
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName");
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName");
            return View();
        }

        // POST: BookReturnTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookReturnID,BookID,EmployeeID,IssueDate,ReturnDate,CurrentDate,UserID")] BookReturnTable bookReturnTable)
        {
            if (ModelState.IsValid)
            {
                db.BookReturnTables.Add(bookReturnTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookReturnTable.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookReturnTable.EmployeeID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookReturnTable.UserID);
            return View(bookReturnTable);
        }

        // GET: BookReturnTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReturnTable bookReturnTable = db.BookReturnTables.Find(id);
            if (bookReturnTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookReturnTable.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookReturnTable.EmployeeID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookReturnTable.UserID);
            return View(bookReturnTable);
        }

        // POST: BookReturnTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookReturnID,BookID,EmployeeID,IssueDate,ReturnDate,CurrentDate,UserID")] BookReturnTable bookReturnTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookReturnTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookReturnTable.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookReturnTable.EmployeeID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookReturnTable.UserID);
            return View(bookReturnTable);
        }

        // GET: BookReturnTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookReturnTable bookReturnTable = db.BookReturnTables.Find(id);
            if (bookReturnTable == null)
            {
                return HttpNotFound();
            }
            return View(bookReturnTable);
        }

        // POST: BookReturnTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookReturnTable bookReturnTable = db.BookReturnTables.Find(id);
            db.BookReturnTables.Remove(bookReturnTable);
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
