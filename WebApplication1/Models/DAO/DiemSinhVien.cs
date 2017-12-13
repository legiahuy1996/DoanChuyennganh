using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class DiemSinhVien
    {
        private int namhoc;
        private int hocky;
        private string mamh;
        private string tenmh;
        private double? diemqt;
        private double? diemgk;
        private double? diemck;
        private int? solanthi;
        private double? tongdiem;

   
        
   
        public string Mamh { get => mamh; set => mamh = value; }
        public string Tenmh { get => tenmh; set => tenmh = value; }
        public double? Diemqt { get => diemqt; set => diemqt = value; }
        public double? Diemgk { get => diemgk; set => diemgk = value; }
        public double? Diemck { get => diemck; set => diemck = value; }
        public int? Solanthi { get => solanthi; set => solanthi = value; }
        public double? Tongdiem { get => tongdiem; set => tongdiem = value; }
        public int Namhoc { get => namhoc; set => namhoc = value; }
        public int Hocky { get => hocky; set => hocky = value; }
    }
}