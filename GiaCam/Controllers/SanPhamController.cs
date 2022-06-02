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