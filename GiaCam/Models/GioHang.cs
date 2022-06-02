using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaCam.Models;

namespace GiaCam.Models
{
    public class GioHang
    {
        dbGiaCamDataContext data = new dbGiaCamDataContext();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhSP { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }   
        public double dThanhTien
        {
            get { return dDonGia * iSoLuong; }
        }

        public GioHang(int maSP)
        {
            iMaSP = maSP;
            SanPham sp = data.SanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = sp.TenSP;
            sAnhSP = sp.AnhSP;
            dDonGia = double.Parse(sp.GiaSP.ToString());
            iSoLuong = 1;
        }
    }
}