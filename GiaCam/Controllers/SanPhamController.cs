using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiaCam.Models;

namespace GiaCam.Controllers
{
    public class SanPhamController : Controller
    {
        dbGiaCamDataContext data = new dbGiaCamDataContext();
        public ActionResult LoaiSP()
        {
            var loaiSP = from loai in data.LoaiSanPhams select loai;
            return PartialView(loaiSP);
        }
        public ActionResult NhaCungCap()
        {
            var nhaCC = from ncc in data.NhaCungCaps select ncc;
            return PartialView(nhaCC);
        }
        private List<SanPham> LayDSSP()
        {
            return data.SanPhams.ToList();
        }

        public ActionResult SPTheoLoai(int? id)
        {
            var sp = from s in data.SanPhams where s.MaLoai == id select s;
            return View(sp);
        }
        public ActionResult SPTheoNCC(int? id)
        {
            var sp = from s in data.SanPhams where s.MaNCC == id select s;
            return View(sp);
        }
        [HttpGet]
        public ActionResult TimSP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TimSP(FormCollection collection)
        {
            var tensp = collection["tenSP"];
            if (string.IsNullOrEmpty(tensp))
            {
                ViewData["Loi1"] = "Phải nhập tên sản phẩm!";
            }
            else
            {
                List<SanPham> list = data.SanPhams.ToList();
                int masp = 0;
                foreach(var item in list)
                {
                    if (item.TenSP.ToLower().Contains(tensp.ToLower()))
                    {
                        masp = item.MaSP;
                        ViewBag.ThongBao = "Đã tìm thấy sản phẩm!";
                        return RedirectToAction("SanPhamDaTim", "SanPham", new {ma = masp});
                    }
                    else
                    {
                        ViewBag.ThongBao = "Không tìm thấy sản phẩm!";
                    }    
                }      
            }
            return this.TimSP();
        }
        public ActionResult SanPhamDaTim(int ma)
        {
            var sp = from s in data.SanPhams where s.MaSP == ma select s;
            return View(sp);
        }

        public ActionResult Detail(int? id)
        {
            var sp = from s in data.SanPhams
                     where s.MaSP == id
                     select s;
            return View(sp.Single());
        }

        
        // GET: SanPham
        public ActionResult Index()
        {
            var DSSP = LayDSSP();
            return View(DSSP);
        }
    }
}