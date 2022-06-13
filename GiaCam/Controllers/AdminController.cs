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
            int pageNum = (page ?? 1);
            int pageSize = 5;

            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNum, pageSize));
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
            ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai", sp.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
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
            ViewBag.MaLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(SanPham sp)
        {
            return RedirectToAction("SanPham");         
        }
    }
}