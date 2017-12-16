using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.EF;
namespace WebApplication1.Models.DAO
{
    public class PDTdao
    {
        Model1 db = new Model1();
        private static PDTdao instance;

        public static PDTdao Instance
        {
            get
            {
                if (instance == null)
                    instance = new PDTdao();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        private PDTdao() {}
        public bool login(string id, string password)
        {
            var result = db.PDTs.SingleOrDefault(x => x.msnv == id && x.matkhau == password);
            if (result!=null)
                return true;
            return false;
        }
        public PDT getNhanVienByID(string id)
        {
            var result = db.PDTs.SingleOrDefault(x => x.msnv == id);
            return result;
        }
        public void Sua(string ma, string hoten, string email, string sdt)
        {
            PDT nv = getNhanVienByID(ma);
            nv.hoten = hoten;
            nv.email = email;
            nv.sdt = sdt;
           
            db.SaveChanges();
        }
        public bool doimatkhau(string ma,string oldpassword,string newpassword)
        {

            PDT nv = db.PDTs.SingleOrDefault(x => x.msnv == ma && x.matkhau == oldpassword);
            if (nv == null)
                return false;
            else
            {
                nv.matkhau = Encryptor.MD5Hash(newpassword);
                db.SaveChanges();
                return true;
            }

        }
    }
}