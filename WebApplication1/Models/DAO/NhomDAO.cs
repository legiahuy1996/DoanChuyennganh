using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
    public class NhomDAO
    {
        Model1 db = new Model1();
        private static NhomDAO instance;

        public static NhomDAO Instance
        {
            get { if (instance == null) instance = new NhomDAO(); return instance; }
            private set { instance = value; }
        }
        private NhomDAO() { }
        public List<nhom> GetListNhomByTenNhom(string tennhom)
        {
            return db.nhoms.Where(x => x.tennhom.Contains(tennhom)).ToList();
        }
        public List<nhom> GetListNhom()
        {
            return db.nhoms.ToList();
        }
        public List<nhom> GetListNhomByMonHoc(string mamh)
        {
            return db.nhoms.Where(x => x.mamh == mamh).ToList();
        }
    }
    
}