using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;

namespace GiaCam.Controllers
{
    public class GopYController : Controller
    {
        dbGiaCamDataContext db = new dbGiaCamDataContext();
        [HttpGet]
        public ActionResult GopY()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GopY(FormCollection collection, GopY y)
        {
            var hoten = collection["ten"];
            var sdt = collection["so"];
            var ykien = collection["gopY"];
            if (String.IsNullOrEmpty(ykien))
            {
                ViewData["Loi1"] = "Hãy nhập góp ý của bạn!";
            }
            else
            {
                y.HoTen = hoten;
                y.SDT = sdt;
                y.YKien = ykien;
                db.Gopies.InsertOnSubmit(y);
                db.SubmitChanges();
                ViewBag.ThongBao = "Cảm ơn bạn đã để lại góp ý cho chúng tôi";

            }
            return View();
        }


        // GET: GopY
        public ActionResult Index()
        {
            return View();
        }
    }
}