using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
    public class hockyDAO
    {
        Model1 db = new Model1();
        private static hockyDAO instance;

        public static hockyDAO Instance {
            get
            {
                if (instance == null)
                    instance = new hockyDAO();
                return instance;
            }
           private set
            {
                instance = value;
            }
        }
        private hockyDAO() { }
        public hocky GetHockyByID(string ma)
        {
            return db.hockies.SingleOrDefault(x => x.mahk == ma);
        }
        public hocky GetHockyByHocKy(int hocky,int nam)
        {
            return db.hockies.SingleOrDefault(x => x.hocky1 == hocky && x.nam == nam);
        }
    }
}