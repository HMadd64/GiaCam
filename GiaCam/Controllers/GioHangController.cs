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
            List<SanPham> dssp = data.SanPhams.ToList();
            GioHang sp = list.SingleOrDefault(n => n.iMaSP == maSP);
            if(sp.iSoLuong != null)
            {
                foreach(var item in dssp)
                {
                    if(maSP == item.MaSP)
                    {
                        if (int.Parse(f["txtSoLuong"].ToString()) <= item.TonKho)
                        {
                            sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                        }
                    }   
                }  
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
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","TaiKhoan");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SanPham");
            }
            //var ngayGiaoHang = Session["NgayGiao"];
            //if (string.IsNullOrEmpty(ngayGiaoHang.ToString()))
            //{
            //    ViewData["Loi1"] = "Phải chọn ngày giao hàng!";
            //}
            List<GioHang> list = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(list);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            HoaDon hd = new HoaDon();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            List<SanPham> dssp = data.SanPhams.ToList();

            hd.MaKH = kh.MaKH;
            hd.NgayMua = DateTime.Now;
            string ngayGiaoHang = collection["NgayGiao"];
            //if(ngayGiaoHang.CompareTo(DateTime.Now) > 0 )
            //{
            //    ViewData["Loi1"] = "Ngày giao phải lớn hơn ngày đặt!";
            //}
            //else
            //{
              var ngayGiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
              hd.NgayGiaoHang = DateTime.Parse(ngayGiao);
            //}
            hd.TinhTrangGiao = "Chưa giao hàng";
            hd.ThanhTien = TongTien();
            data.HoaDons.InsertOnSubmit(hd);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                foreach(var sp in dssp)
                {
                    if(sp.MaSP == item.iMaSP)
                    {
                        sp.TonKho = sp.TonKho - item.iSoLuong;   
                    }
                }
                CTHD cthd = new CTHD();
                cthd.MaHD = hd.MaHD;
                cthd.MaSP = item.iMaSP;
                cthd.SoLuong = item.iSoLuong;
                cthd.DonGia = item.dDonGia;
                data.CTHDs.InsertOnSubmit(cthd);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
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