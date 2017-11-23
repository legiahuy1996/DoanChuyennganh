using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;

namespace WebApplication1.Models.DAO
{
   
    public class DkmonhocDAO
    {
        Model1 db = new Model1();
        private static DkmonhocDAO instance;

        public static DkmonhocDAO Instance
        {
            get { if (instance == null) instance = new DkmonhocDAO(); return instance; }
            private set { instance = value; }
        }
        private DkmonhocDAO() { }



       

        public dkmonhoc GetDkmonhocbyID(string ma)
        {
            return db.dkmonhocs.SingleOrDefault(x => x.madk == ma);
        }
    }
}