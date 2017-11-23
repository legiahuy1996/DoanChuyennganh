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
        public List<monhoc> GetList()
        {
            return db.monhocs.ToList();
        }
    }
}