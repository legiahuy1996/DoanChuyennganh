using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
    public class MonHocDao
    {
        Model1 db = new Model1();
        private static MonHocDao instance;

        public static MonHocDao Instance
        {
            get { if (instance == null) instance = new MonHocDao(); return instance; }
            private set { instance = value; }
        }
        private MonHocDao() { }
        public monhoc getmonhocbyID(string ma)
        {
            return db.monhocs.SingleOrDefault(x => x.mamh == ma);
        }
        public List<monhoc> getlistmonhocbyID(string ma)
        {
            return db.monhocs.Where(x => x.mamh == ma).ToList();
        }
        public List<monhoc> GetList()
        {
            return db.monhocs.ToList();
        }
        public bool delete(string ma)
        {
            monhoc kq = db.monhocs.SingleOrDefault(x => x.mamh == ma);
            if (kq != null)
            {
                db.monhocs.Remove(kq);
                db.SaveChanges();
                return true;
            }
            else return false;
        }
        public string laymonhocmoinhat()
        {
            string mamh="";
            List<monhoc> lst = db.monhocs.OrderByDescending(x=>x.mamh).ToList();
            foreach(monhoc mh in lst)
            {
                 mamh = mh.mamh;
                break;
            }
            return mamh;
        }
        public void insert(monhoc mh)
        {
            db.monhocs.Add(mh);
            db.SaveChanges();
        }
    }
}