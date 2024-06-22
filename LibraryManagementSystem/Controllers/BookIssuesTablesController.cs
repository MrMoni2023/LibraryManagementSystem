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
    public class BookIssuesTablesController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();

        // GET: BookIssuesTables
        public ActionResult BookIssue()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            var bookIssuesTables = db.BookIssuesTables.Include(b => b.BooksTable).Include(b => b.EmployeeTable).Include(b => b.UserTable).Where(b => b.Status == true && b.ReserveNoOfCopies == false);
            return View(bookIssuesTables.ToList());
        }

        public ActionResult ReserveBook()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            var bookIssuesTables = db.BookIssuesTables.Include(b => b.BooksTable).Include(b => b.EmployeeTable).Include(b => b.UserTable).Where(b => b.Status == false && b.ReserveNoOfCopies == true && b.ReturnDate > DateTime.Now);
            return View(bookIssuesTables.ToList());
        }
        public ActionResult ReturnPendingBook()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            List<BookIssuesTable> list = new List<BookIssuesTable>();
            var bookIssuesTables = db.BookIssuesTables.Include(b => b.BooksTable).Include(b => b.EmployeeTable).Include(b => b.UserTable).Where(b => b.Status == true && b.ReserveNoOfCopies == true ).ToList();
            foreach (var item in bookIssuesTables)
            {
                var returndate = item.ReturnDate;
                int noofdays = (returndate - DateTime.Now).Days;
                if (noofdays <= 3)
                {
                    list.Add(new BookIssuesTable
                    {
                        BookID = item.BookID,
                        BooksTable = item.BooksTable,
                        Description = item.Description,
                        EmployeeID = item.EmployeeID,
                        EmployeeTable = item.EmployeeTable,
                        IssueBookID = item.IssueBookID,
                        IssueCopies = item.IssueCopies,
                        ReserveNoOfCopies = item.ReserveNoOfCopies,
                        ReturnDate = item.ReturnDate,
                        Status = item.Status,
                        UserID = item.UserID,
                        UserTable = item.UserTable,
                    });
                }
                
            }

            return View(list.ToList());
        }
        
        // GET: BookIssuesTables/Details/5
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
            BookIssuesTable bookIssuesTable = db.BookIssuesTables.Find(id);
            if (bookIssuesTable == null)
            {
                return HttpNotFound();
            }
            return View(bookIssuesTable);
        }

        // GET: BookIssuesTables/Create
        
        public ActionResult Create()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", "0");
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", "0");
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", "0");
            return View();
        }

        // POST: BookIssuesTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookIssuesTable bookIssuesTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            bookIssuesTable.UserID = userid;

           
            if (ModelState.IsValid)
            {
                var find = db.BookIssuesTables.Where(b => b.ReturnDate >= DateTime.Now && b.BookID == bookIssuesTable.BookID && (b.Status == true || b.ReserveNoOfCopies == true)).ToList();
                int issuebook = 0;
                foreach (var item in find)
                {
                    issuebook = issuebook + item.IssueCopies;
                }
                if (db != null && bookIssuesTable != null && bookIssuesTable.BookID != null)
                {
                    var stockbook = db.BookIssuesTables.Where(b => b.BookID == bookIssuesTable.BookID).FirstOrDefault();
                    if (stockbook != null)
                    {
                        // Check the condition using stockbook properties
                        if ((issuebook == stockbook.TotalCopies) || (issuebook + bookIssuesTable.IssueCopies > stockbook.TotalCopies))
                        {
                            ViewBag.Message = "Stock is Empty!";
                            return View(bookIssuesTable);
                        }
                    }
                    else
                    {
                        // Handle the case when stockbook is null
                        ViewBag.Message = "Stockbook not found!";
                        return View(bookIssuesTable);
                    }
                }
                else
                {
                    // Handle the case when db, bookIssuesTable or its property is null
                    ViewBag.Message = "Invalid data!";
                    return View(bookIssuesTable);
                }


                db.BookIssuesTables.Add(bookIssuesTable);
                db.SaveChanges();
                ViewBag.Message = "Book Issue Syccessfully !";
                return RedirectToAction("Index");
            }


            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookIssuesTable.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookIssuesTable.EmployeeID);
            ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookIssuesTable.UserID);


            ModelState.AddModelError("", "Model state is invalid");
            return View(bookIssuesTable);
            
            
        }
        public ActionResult Recreate()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", "0");
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", "0");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recreate(BookIssuesTable BookIssue)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            BookIssue.UserID = userid;


            if (ModelState.IsValid)
            {
                var find = db.BookIssuesTables.Where(b => b.ReturnDate >= DateTime.Now && b.BookID == BookIssue.BookID && (b.Status == true || b.ReserveNoOfCopies == true)).ToList();
                int issuebook = 0;
                foreach (var item in find)
                {
                    issuebook = issuebook + item.IssueCopies;
                }
                if (db != null && BookIssue != null && BookIssue.BookID != null)
                {
                    var stockbook = db.BookIssuesTables.Where(b => b.BookID == BookIssue.BookID).FirstOrDefault();
                    if (stockbook != null)
                    {
                        // Check the condition using stockbook properties
                        if ((issuebook == stockbook.TotalCopies) || (issuebook + BookIssue.IssueCopies > stockbook.TotalCopies))
                        {
                            ViewBag.Message = "Stock is Empty!";
                            return View(BookIssue);
                        }
                    }
                    else
                    {
                        // Handle the case when stockbook is null
                        ViewBag.Message = "Stockbook not found!";
                        return View(BookIssue);
                    }
                }
                else
                {
                    // Handle the case when db, bookIssuesTable or its property is null
                    ViewBag.Message = "Invalid data!";
                    return View(BookIssue);
                }


                db.BookIssuesTables.Add(BookIssue);
                db.SaveChanges();
                ViewBag.Message = "Book Issue Syccessfully !";
                return RedirectToAction("Index");
            }


            ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", BookIssue.BookID);
            ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", BookIssue.EmployeeID);
            

            ModelState.AddModelError("", "Model state is invalid");
            return View(BookIssue);
        }

        public ActionResult ReturnBook(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var book = db.BookIssuesTables.Find(id);
            int fine = 0;
            var returndate = book.ReturnDate;
            int noofdays = (DateTime.Now - returndate).Days;
            
            if(book.Status == true && book.ReserveNoOfCopies == false)
            {
                if (noofdays > 0)
                {
                    fine = 20 * noofdays;
                }
                var returnbook = new BookReturnTable()
                {
                    BookID = book.BookID,
                    CurrentDate = DateTime.Now,
                    EmployeeID = book.EmployeeID,
                    IssueDate = book.IssueDate,
                    ReturnDate = book.ReturnDate,
                    UserID = userid
                };
                db.BookReturnTables.Add(returnbook);
                db.SaveChanges();
            } 
            book.Status = false;
            book.ReserveNoOfCopies = false;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            if(fine > 0)
            {
                var addfine = new BookFineTable()
                {
                    BookID = book.BookID,
                    EmployeeID=book.EmployeeID,
                    FineAmount = fine,
                    FineDate = DateTime.Now,
                    NoOfDays = noofdays,
                    ReceiveAmount = 0,
                    UserID = userid
                };
                db.BookFineTables.Add(addfine);
                db.SaveChanges();
            }

            return RedirectToAction("BookIssue");
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(BookIssuesTable bookIssuesTable)
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
        //    bookIssuesTable.UserID = userid;

        //    var books = db.BooksTables.ToList(); // Fetch the list of books from your data source
        //    var employee = db.EmployeeTables.ToList();
        //    //var issuecopies = db.BookIssuesTables.ToList();

        //    // Create a SelectList from the books collection
        //    SelectList bookList = new SelectList(books, "BookID", "BookTitle");
        //    SelectList employeelist = new SelectList(employee, "EmployeeID", "FullName");
        //    //SelectList issuecopieslist = new SelectList(issuecopieslist, "","")

        //    // Set the SelectList to ViewBag
        //    ViewBag.BookList = bookList;
        //    ViewBag.EmployeeList = employeelist;

        //    if (ModelState.IsValid)
        //    {
        //        var find = db.BookIssuesTables
        //            .Where(b => b.ReturnDate >= DateTime.Now && b.BookID == bookIssuesTable.BookID && b.Status == true || b.ReserveNoOfCopies == true)
        //            .ToList();

        //        int issuebook = 0;
        //        foreach (var item in find)
        //        {
        //            issuebook = issuebook + item.IssueCopies;
        //        }

        //        var stockbook = db.BookIssuesTables.FirstOrDefault(b => b.BookID == bookIssuesTable.BookID);

        //        if (stockbook != null)
        //        {
        //            if (issuebook == stockbook.TotalCopies || issuebook + bookIssuesTable.IssueCopies > stockbook.TotalCopies)
        //            {
        //                ViewBag.Message = "Stock is Empty";
        //                return View(bookIssuesTable);
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Stock information not found";
        //            return View(bookIssuesTable);
        //        }

        //        db.BookIssuesTables.Add(bookIssuesTable);
        //        db.SaveChanges();
        //        ViewBag.Message = "Book Issue Successfully!";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Database or book information is not available";
        //        return View(bookIssuesTable);
        //    }
        //}


        //// GET: BookIssuesTables/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BookIssuesTable bookIssuesTable = db.BookIssuesTables.Find(id);
        //    if (bookIssuesTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookIssuesTable.BookID);
        //    ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookIssuesTable.EmployeeID);
        //    ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookIssuesTable.UserID);
        //    return View(bookIssuesTable);
        //}

        //// POST: BookIssuesTables/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IssueBookID,UserID,BookID,EmployeeID,IssueCopies,IssueDate,ReturnDate,Status,Description,ReserveNoOfCopies")] BookIssuesTable bookIssuesTable)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(bookIssuesTable).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookTitle", bookIssuesTable.BookID);
        //    ViewBag.EmployeeID = new SelectList(db.EmployeeTables, "EmployeeID", "FullName", bookIssuesTable.EmployeeID);
        //    ViewBag.UserID = new SelectList(db.UserTables, "UserID", "UserName", bookIssuesTable.UserID);
        //    return View(bookIssuesTable);
        //}

        //// GET: BookIssuesTables/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BookIssuesTable bookIssuesTable = db.BookIssuesTables.Find(id);
        //    if (bookIssuesTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bookIssuesTable);
        //}

        //// POST: BookIssuesTables/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BookIssuesTable bookIssuesTable = db.BookIssuesTables.Find(id);
        //    db.BookIssuesTables.Remove(bookIssuesTable);
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
