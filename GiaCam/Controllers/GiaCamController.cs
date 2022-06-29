using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;
using PagedList;
using PagedList.Mvc;
 
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
        public ActionResult Index(int ? page)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);

            var SPMoi = LaySPMoi(15);
            return View(SPMoi.ToPagedList(pageNum,pageSize));
        }
    }
}