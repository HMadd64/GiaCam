using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;

namespace GiaCam.Controllers
{
    public class GioHangController : Controller
    {
        dbGiaCamDataContext data = new dbGiaCamDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }
        public ActionResult ThemGioHang(int maSP, string url)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.Find(n => n.iMaSP == maSP);
            if(sp == null)
            {
                sp = new GioHang(maSP);
                list.Add(sp);
                return Redirect(url);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(url);
            }
        }
        public ActionResult XoaGioHang(int maSP)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.SingleOrDefault(n => n.iMaSP == maSP);
            if(sp != null)
            {
                list.RemoveAll(n => n.iMaSP == maSP);
                return RedirectToAction("GioHang");
            }
            if(list.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int maSP, FormCollection f)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.SingleOrDefault(n => n.iMaSP == maSP);
            if(sp.iSoLuong != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());    
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> list = LayGioHang();
            list.Clear();
            return RedirectToAction("Index", "SanPham");
        }
        private int TongSoLuong()
        {
            int iTongSL = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if(list != null)
            {
                iTongSL = list.Sum(n => n.iSoLuong);
            }
            return iTongSL;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list != null)
            {
                dTongTien = list.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> list = LayGioHang();
            if(list.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(list);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
    }
}