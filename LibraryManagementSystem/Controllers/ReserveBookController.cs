using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class ReserveBookController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();
        // GET: ReserveBook
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var book = db.BooksTables.ToList();
            return View(book);
        }
        public ActionResult ReserveBook(int? id)
        {
            var book = db.BooksTables.Find(id);
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            int employeeid = Convert.ToInt32(Convert.ToString(Session["EmployeeID"]));
            var bookIssuesTable = new BookIssuesTable()
            {
                BookID = book.BookID,
                Description = "Reserve Request",
                EmployeeID = employeeid,
                IssueCopies = 1,
                IssueDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(2),
                Status = false,
                ReserveNoOfCopies = true,
                UserID = userid,
            };
            bookIssuesTable.UserID = userid;


            if (ModelState.IsValid)
            {
                var find = db.BookIssuesTables.Where(b => b.ReturnDate >= DateTime.Now && b.BookID == bookIssuesTable.BookID && (b.Status == true || b.ReserveNoOfCopies == true)).ToList();
                int issuebook = 0;
                foreach (var item in find)
                {
                    issuebook = issuebook + item.IssueCopies;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (db != null && bookIssuesTable != null && bookIssuesTable.BookID != null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    var stockbook = db.BookIssuesTables.Where(b => b.BookID == bookIssuesTable.BookID).FirstOrDefault();
                    if (stockbook != null)
                    {
                        // Check the condition using stockbook properties
                        if ((issuebook == stockbook.TotalCopies) || (issuebook + bookIssuesTable.IssueCopies > stockbook.TotalCopies))
                        {
                            ViewBag.Message = "Stock is Empty!";
                        }
                    }
                    else
                    {
                        // Handle the case when stockbook is null
                        ViewBag.Message = "Stockbook not found!";
                    }
                }
                else
                {
                    // Handle the case when db, bookIssuesTable or its property is null
                    ViewBag.Message = "Invalid data!";
                }


                db.BookIssuesTables.Add(bookIssuesTable);
                db.SaveChanges();
                ViewBag.Message = "Book Issue Syccessfully !";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}