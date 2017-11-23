using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class DiemMonHoc
    {
        private string mssv;

        public string Mssv { get => mssv; set => mssv = value; }
        public string Hoten { get => hoten; set => hoten = value; }
        public double? DiemQT { get => diemQT; set => diemQT = value; }
        public double? DiemGK { get => diemGK; set => diemGK = value; }
        public double? DiemCK { get => diemCK; set => diemCK = value; }
        public double? Tongdiem { get => tongdiem; set => tongdiem = value; }
        public string Madiem { get => madiem; set => madiem = value; }

        private string madiem;
        private string hoten;
        private double? diemQT;
        private double? diemGK;
        private double? diemCK;
        private double? tongdiem;


    }
}