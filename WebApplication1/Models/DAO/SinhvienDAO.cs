using WebApplication1.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class SinhvienDAO
    {
        Model1 db = new Model1();
        private static SinhvienDAO instance;

        public static SinhvienDAO Instance
        {
            get { if (instance == null) instance = new SinhvienDAO(); return instance; }
            private set { instance = value; }
        }
        private SinhvienDAO() { }


        public bool Login(string username , string password)
        {
            var result = db.sinhviens.Count(x => x.mssv == username && x.matkhau == password);
            if (result > 0)
                return true;
            else
                return false;
        }
        public sinhvien GetSVByMSSV(string mssv)
        {
           
           return db.sinhviens.SingleOrDefault(x => x.mssv == mssv); 

        }
        public List<sinhvien> GetListSV()
        {
            var result = db.sinhviens.ToList();
            return result;

        }
        public List<sinhvien> GetNewestSV()
        {
            return db.sinhviens.OrderByDescending(x=>x.mssv).Take(1).ToList();
            
           
        }
        public sinhvien GetSVByMaDK(string madk)
        {
            var result = db.sinhviens.SingleOrDefault(x => x.madk == madk);
            return result;

        }
        public void Insert(sinhvien sv)
        {
          
            db.sinhviens.Add(sv);
            db.SaveChanges();
        }
        public bool Delete(string mssv)
        {
            sinhvien sv = db.sinhviens.SingleOrDefault(x => x.mssv == mssv);
            if(sv !=null)
            {
                db.sinhviens.Remove(sv);
                db.SaveChanges();
                return true;
            }
            return false;
           
        }
        public void Edit(sinhvien sv)
        {
            
            foreach(sinhvien sv1 in db.sinhviens.ToList())
            {
                if (sv1.mssv == sv.mssv)
                {
                    sv1.hoten = sv.hoten;
                    sv1.gioitinh = sv.gioitinh;
                    sv1.lop = sv.lop;
                    sv1.madk = sv.madk;
                    sv1.makhoa = sv.makhoa;
                    sv1.matkhau = sv.matkhau;
                    sv1.email = sv.email;
                    sv1.diachi = sv.diachi;
                    sv1.ngaysinh = sv.ngaysinh;
                    sv1.sdt = sv.sdt;

              


                    db.SaveChanges();
                }
                    
            }
            
            
        }
    }
}