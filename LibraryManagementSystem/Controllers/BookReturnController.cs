using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BookReturnController : Controller
    {
        private OnlineLibraryMgtSystemDBEntities db = new OnlineLibraryMgtSystemDBEntities();
        // GET: BookReturn
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var returnbook = db.BookReturnTables.ToList();
            return View(returnbook);
        }
    }
}