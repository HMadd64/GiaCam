using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;

namespace GiaCam.Controllers
{
    public class TaiKhoanController : Controller
    {
        dbGiaCamDataContext db = new dbGiaCamDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection,KhachHang kh)
        {
            var hoten = collection["tenKH"];
            var tendn = collection["tenDN"];
            var matkhau = collection["matKhau"];
            var matkhaunhaplai = collection["matKhauNhapLai"];
            var email = collection["Email"];
            var dienthoai = collection["sdt"];
            var dc = collection["diaChi"];
            var ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["ngaySinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không dược trống!";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Hãy nhập tên tài khoản!";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Hãy nhập mật khẩu!";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu!";
            }
            else
            {
                if(matkhaunhaplai != matkhau)
                {
                    ViewData["Loi4"] = "Mật khẩu nhập lại không đúng!";
                }
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Hãy nhập số điện thoại!";
            }
            else
            {
                if (dienthoai.Length!=10)
                {
                    ViewData["Loi6"] = "Số điện thoại không hợp lệ!";
                }
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Hãy nhập email!";
            }
            else
            {
                kh.TenKH = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.SDT = dienthoai;
                kh.DiaChi = dc;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                ViewBag.ThongBao = "Chúc mừng đăng ký thành công!";
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["tenDN"];
            var matkhau = collection["matKhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
            }
            else
            {
                if (string.IsNullOrEmpty(matkhau))
                {
                    ViewData["Loi1"] = "Phải nhập tên mật khẩu!";
                }
                else
                {
                    KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                    if(kh != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công!";
                        Session["TaiKhoan"] = kh;
                        return RedirectToAction("Index", "GiaCam");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    }
                }
            }
            return View();
        }
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }
    }
}