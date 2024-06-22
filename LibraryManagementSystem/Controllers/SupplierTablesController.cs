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
    public class SupplierTablesController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();

        // GET: SupplierTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            var supplierTables = db.SupplierTables.Include(s => s.UserTable);
            return View(supplierTables.ToList());
        }

        // GET: SupplierTables/Details/5
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
            SupplierTable supplierTable = db.SupplierTables.Find(id);
            if (supplierTable == null)
            {
                return HttpNotFound();
            }
            return View(supplierTable);
        }

        // GET: SupplierTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName");
            return View();
        }

        // POST: SupplierTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierTable supplierTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            supplierTable.UserID = userid;
            if (ModelState.IsValid)
            {
                var find = db.SupplierTables.Where(s => s.SupplierName == supplierTable.SupplierName && s.ContactNo == supplierTable.ContactNo).FirstOrDefault();
                if(find == null)
                {
                    db.SupplierTables.Add(supplierTable);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Supplier Already Registered! ";
                }
                
            }

            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", supplierTable.UserID);
            return View(supplierTable);
        }

        // GET: SupplierTables/Edit/5
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
            SupplierTable supplierTable = db.SupplierTables.Find(id);
            if (supplierTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", supplierTable.UserID);
            return View(supplierTable);
        }

        // POST: SupplierTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( SupplierTable supplierTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            supplierTable.UserID = userid;
            if (ModelState.IsValid)
            {
                db.Entry(supplierTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", supplierTable.UserID);
            return View(supplierTable);
        }

        // GET: SupplierTables/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SupplierTable supplierTable = db.SupplierTables.Find(id);
        //    if (supplierTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(supplierTable);
        //}

        //// POST: SupplierTables/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SupplierTable supplierTable = db.SupplierTables.Find(id);
        //    db.SupplierTables.Remove(supplierTable);
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
