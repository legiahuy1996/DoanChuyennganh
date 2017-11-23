using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
    public class ChiTietdkDAO
    {
        Model1 db = new Model1();
        private static ChiTietdkDAO instance;

        public static ChiTietdkDAO Instance
        {
            get { if (instance == null) instance = new ChiTietdkDAO(); return instance; }
            private set { instance = value; }
        }
        private ChiTietdkDAO() { }
        public List<chitietdk> GetListChitietdkByIDdk(string ma)
        {
            return db.chitietdks.Where(x => x.madk == ma).ToList();
        }
        public chitietdk GetChitietdkByIDdk(string ma)
        {
            return db.chitietdks.SingleOrDefault(x => x.madk == ma);
        }
        public List<chitietdk> GetListChitietdkByManhom(string manhom)
        {
            return db.chitietdks.Where(x => x.manhom == manhom).ToList();
        }
        public bool delete(string madiem)
        {

            chitietdk ct = db.chitietdks.SingleOrDefault(x => x.madiem == madiem);
            if (ct != null)
            {
                db.chitietdks.Remove(ct);
                db.SaveChanges();
                return true;
            }
            else
                return false;

        }

    }
}