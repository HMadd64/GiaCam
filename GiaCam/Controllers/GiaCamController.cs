using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;

namespace GiaCam.Controllers
{
    public class GiaCamController : Controller
    {
        dbGiaCamDataContext data = new dbGiaCamDataContext();
        private List<SanPham> LaySPMoi(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.NgayNhap).Take(count).ToList();
        }

        public ActionResult LienHe()
        {
            return View();
        }

        // GET: GiaCam
        public ActionResult Index()
        {
            var SPMoi = LaySPMoi(3);
            return View(SPMoi);
        }
    }
}