using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;
using PagedList;
using PagedList.Mvc;

namespace GiaCam.Controllers
{
    public class AdminController : Controller
    {
        dbGiaCamDataContext db = new dbGiaCamDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NhaCungCap()
        {
            List<NhaCungCap> list = db.NhaCungCaps.ToList();
            return View(list);
        }
        public ActionResult DetailNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            ViewBag.MaNCC = ncc.MaNCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpGet]
        public ActionResult CreateNCC()
        { 
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNCC(NhaCungCap ncc)
        {
            db.NhaCungCaps.InsertOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("NhaCungCap");
        }
        [HttpGet]
        public ActionResult EditNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost]
        public ActionResult EditNCC(int id, FormCollection collection)
        {
            var tenncc = collection["TenNCC"];
            var diachi = collection["DiaChi"];
            var sdt = collection["SDT"];
            var ncc = from cc in db.NhaCungCaps where cc.MaNCC == id select cc;
            if (ncc != null)
            {
                foreach (var item in ncc)
                {
                    if (item.MaNCC == id)
                    {
                        item.TenNCC = tenncc;
                        item.DiaChi = diachi;
                        item.SDT = sdt;
                        UpdateModel(ncc);
                        db.SubmitChanges();
                        return RedirectToAction("NhaCungCap");
                    }
                }
            }
            return RedirectToAction("NhaCungCap");
        }
        [HttpGet]
        public ActionResult DeleteNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC== id);
            ViewBag.MaNCC = ncc.MaNCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }
        [HttpPost, ActionName("DeleteNCC")]
        public ActionResult XacNhanDeleteNCC(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            ViewBag.MaNCC = ncc.MaNCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NhaCungCaps.DeleteOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("NhaCungCap");
        }

        public ActionResult LoaiSanPham()
        {
            List<LoaiSanPham> list = db.LoaiSanPhams.ToList();
            return View(list);
        }
        public ActionResult DetailLoai(int id)
        {
            LoaiSanPham loai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpGet]
        public ActionResult CreateLoai()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateLoai(LoaiSanPham loai)
        {
            db.LoaiSanPhams.InsertOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("LoaiSanPham");
        }
        [HttpGet]
        public ActionResult EditLoai(int id)
        {
            LoaiSanPham loai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoai == id);
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost]
        public ActionResult EditLoai(int id, FormCollection collection)
        {
            var tenloai = collection["TenLoai"];
            var loai = from l in db.LoaiSanPhams where l.MaLoai == id select l;
            if (tenloai != null)
            {
                foreach(var item in loai)
                {
                    if(item.MaLoai == id)
                    {
                        item.TenLoai = tenloai;
                        UpdateModel(loai);
                        db.SubmitChanges();
                        return RedirectToAction("LoaiSanPham");
                    }
                }   
            }
            return RedirectToAction("LoaiSanPham");
        }
        [HttpGet]
        public ActionResult DeleteLoai(int id)
        {
            LoaiSanPham loai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loai);
        }
        [HttpPost, ActionName("DeleteLoai")]
        public ActionResult XacNhanDeleteLoai(int id)
        {
            LoaiSanPham loai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = loai.MaLoai;
            if (loai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LoaiSanPhams.DeleteOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("LoaiSanPham");
        }

        public ActionResult HoaDon()
        {
            return View();
        }

        public ActionResult ThongKe()
        {
            return View();
        }

        public ActionResult SanPham(int? page)
        {
            var onl = Session["TaiKhoan"];
            if (onl != null)
            {
                int pageNum = (page ?? 1);
                int pageSize = 5;

                return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNum, pageSize));
            }
            return RedirectToAction("DangNhap", "TaiKhoan");
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            if (file == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh sản phẩm!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Hinh_web"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại!";
                    }
                    else
                    {
                        file.SaveAs(path);
                    }
                    sp.AnhSP = fileName;
                    db.SanPhams.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }

        public ActionResult Detail(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanDelete(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai", sp.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase fileUpdate, FormCollection collection)
        {
            var tensp = collection["TenSP"];
            var mota = collection["MoTa"];
            var giasp = float.Parse(collection["GiaSP"].ToString());
            var sl = int.Parse(collection["TonKho"]);
            var ngaynhap = Convert.ToDateTime(collection["NgayNhap"]);
            var hansd = Convert.ToDateTime(collection["HanSuDung"]);
            var maloai = int.Parse(collection["MaLoai"]);
            var mancc = int.Parse(collection["MaNCC"]);

            var sanpham = from sp in db.SanPhams where sp.MaSP == id select sp;
            if (tensp != null)
            {
                foreach (var item in sanpham)
                {
                    if (item.MaSP == id)
                    {
                        item.TenSP = tensp;
                        item.MoTa = mota;

                        ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai", item.MaLoai);
                        ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", item.MaNCC);
                        if (fileUpdate == null)
                        {
                            ViewBag.ThongBao = "Vui lòng chọn ảnh sản phẩm!";
                            return View();
                        }
                        else
                        {

                            var fileName = Path.GetFileName(fileUpdate.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Hinh_web"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.ThongBao = "Hình ảnh đã tồn tại!";
                            }
                            else
                            {
                                fileUpdate.SaveAs(path);
                            }
                            item.AnhSP = fileName;
                        }
                        item.GiaSP = giasp;
                        item.TonKho = sl;
                        item.NgayNhap = ngaynhap;
                        item.HanSuDung = hansd;
                        item.MaLoai = maloai;
                        item.MaNCC = mancc;

                        UpdateModel(sanpham);
                        db.SubmitChanges();
                        return RedirectToAction("SanPham");
                    }
                }         
            }
            
            return RedirectToAction("SanPham");
        }
    }
}
    
