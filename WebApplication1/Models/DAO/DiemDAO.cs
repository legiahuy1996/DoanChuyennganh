using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
   
    public class DiemDAO
    {
        Model1 db = new Model1();
        private static DiemDAO instance;

        public static DiemDAO Instance
        {
            get { if (instance == null) instance = new DiemDAO(); return instance; }
            private set { instance = value; }
        }
        private DiemDAO() { }
        public List<diem> GetListDiemByID(string ma)
        {
            return db.diems.Where(x => x.madiem == ma).ToList();
        }
        public diem GetDiemByID(string ma)
        {
            return db.diems.SingleOrDefault(x => x.madiem == ma);
        }
        public List<diem> GetListDiem()
        {
            return db.diems.ToList();
        }
        public List<diem> GetListNewestDiem()
        {
            return db.diems.OrderByDescending(x => x.madiem).Take(1).ToList();
        }
        public void Insert(diem diem)
        {
            db.diems.Add(diem);
            db.SaveChanges();
        }
        public bool edit(string id,double? diemqt,double? diemgk, double? diemck)
        {
            diem d =db.diems.SingleOrDefault(x => x.madiem == id);
            if (d != null)
            {
                d.diemQT = diemqt;
                d.diemGK = diemgk;
                d.diemCK = diemck;
                d.tongdiem = ((d.diemQT * d.C_diemQT)/100) + ((d.diemGK * d.C_diemGK)/100) + ((d.diemCK * d.C_diemck)/100);
                db.SaveChanges();
                return true;
            }
            else
                return false;
          
        }
        public bool delete(string madiem)
        {
            diem diem = db.diems.SingleOrDefault(x => x.madiem == madiem);
            if (diem != null)
            {
                db.diems.Remove(diem);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}