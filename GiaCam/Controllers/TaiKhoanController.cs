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
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không dược trống!";
            }
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Hãy nhập tên tài khoản!";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Hãy nhập mật khẩu!";
            }
            if (string.IsNullOrEmpty(matkhaunhaplai))
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
            if (string.IsNullOrEmpty(dienthoai))
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
                if(email.Contains("@") != true)
                {
                    ViewData["Loi5"] = "Email không hợp lệ!";
                }
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
            var onl = Session["TaiKhoan"];
            if (onl != null)
            {
                return RedirectToAction("ManageTaiKhoan");
            }    
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
                    ViewData["Loi2"] = "Phải nhập mật khẩu!";
                }
                else
                {
                    KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                    if(kh != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công!";
                        Session["TaiKhoan"] = kh;
                        Session["TenDangNhap"] = tendn;
                        return RedirectToAction("Index", "SanPham");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection collection)
        {
            var tendn = collection["tenDN"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
            }
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Mật khẩu của bạn là: "+ kh.MatKhau;
                    return View();
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập không đúng hoặc không tồn tại!";
                }
                
            }
            return View();
        }

        public ActionResult ManageTaiKhoan()
        {
            var ten = Session["TenDangNhap"];
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == ten);
            return View(kh);
        }
        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection collection)
        {
            var matkhaucu = collection["matKhauCu"];
            var matkhau = collection["matKhau"];
            var matkhaunhaplai = collection["matKhauNhapLai"];
            List<KhachHang> list = db.KhachHangs.ToList();
            if (string.IsNullOrEmpty(matkhaucu))
            {
                ViewData["Loi1"] = "Phải nhập mật khẩu cũ!";
            }
            else
            {
                foreach(var item in list)
                {
                    if(!matkhaucu.Equals(item.MatKhau))
                    {
                        ViewData["Loi1"] = "Không tồn tại mật khẩu này!";
                    }
                }
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Hãy nhập mật khẩu!";
            }
            else
            {
                if (matkhau.Equals(matkhaucu))
                {
                    ViewData["Loi2"] = "Đây là mật khẩu hiện tại!";
                    return View();
                } 
            }
            if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi3"] = "Phải nhập lại mật khẩu!";
            }
            else
            {
                if (matkhaunhaplai != matkhau)
                {
                    ViewData["Loi3"] = "Mật khẩu nhập lại không đúng!";
                }
                else
                {
                    foreach (var item in list)
                    {
                        if (matkhaucu.Equals(item.MatKhau))
                        {
                            item.MatKhau = matkhau;
                            ViewBag.ThongBao = "Đổi mật khẩu thành công!!!";
                            db.SubmitChanges();
                            return RedirectToAction("ManageTaiKhoan", "TaiKhoan");
                        }
                    }
                }    
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            Session["TenDangNhap"] = null;
            return RedirectToAction("DangNhap");
        }
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }
    }
}