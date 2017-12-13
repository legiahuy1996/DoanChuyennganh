using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DAO;
using WebApplication1.Models.EF;
namespace WebApplication1.Controllers
{
    // GET: SinhVien
        public class SinhVienController : Controller
        {
            Model1 db = new Model1();
            // GET: SinhVien
            [HttpGet]
            public ActionResult dangnhap()
            {
                return View();
            }
            [HttpPost]
            public ActionResult dangnhap(string MSSV, string password)
            {


                var result = SinhvienDAO.Instance.Login(MSSV, password);

                if (result)
                {
                    var sv = SinhvienDAO.Instance.GetSVByMSSV(MSSV);
                    Session["taikhoan"] = sv;
                    Session["MSSV"] = sv.mssv;
                    Session["password"] = sv.matkhau;
                    return RedirectToAction("Index", "Home", new { @mssv = sv.mssv });
                }
                else
                    return View();

            }

        public ViewResult xemtatcadiem(string MSSV)
        {
            var result = from a in db.diems
                         join b in db.chitietdks on a.madiem equals b.madiem
                         join c in db.dkmonhocs on b.madk equals c.madk
                         join d in db.sinhviens on c.madk equals d.madk
                         join e in db.nhoms on b.manhom equals e.manhom
                         join f in db.monhocs on e.mamh equals f.mamh
                         join g in db.hockies on c.madk equals g.madk
                         where d.mssv == MSSV
                         select new DiemSinhVien
                         {
                             Namhoc = g.nam,
                             Hocky = g.hocky1,
                             Mamh = f.mamh,
                             Tenmh = f.tenmh,
                             Diemqt = a.diemQT,
                             Diemgk = a.diemGK,
                             Diemck = a.diemCK,
                             Solanthi = a.solanthi,
                             Tongdiem = a.tongdiem

                         };
            sinhvien sv = db.sinhviens.SingleOrDefault(x => x.mssv == MSSV);
            ViewBag.SinhVien = sv;
            List<DiemSinhVien> lst = new List<DiemSinhVien>();
            lst = result.ToList();
            return View(lst);
        }

        public ActionResult xemthongtin(string MSSV)
        {
            sinhvien sv = db.sinhviens.SingleOrDefault(x=>x.mssv == MSSV);
            return View(sv);

        }

        public ActionResult Dangxuat()
        {
            Session["taikhoan"] = null;
            return RedirectToAction("dangnhap");
        }
        public ActionResult doimatkhau(string oldpass, string newpass, string newpass1, string mssv,string oldemail,string newemail)
        {

            sinhvien kiemtra = db.sinhviens.SingleOrDefault(x => x.mssv == mssv);
            
            if (newpass != newpass1)
            {
                ViewBag.ThongBao = "Nhập lại mật khẩu mới chưa đúng!!";
                return RedirectToAction("xemthongtin", new { mssv });
            }
            else if(kiemtra.matkhau != oldpass)
            {
                ViewBag.ThongBao = " mật khẩu chưa đúng!!";
                return RedirectToAction("xemthongtin", new { mssv });
            }
            else
            {
                if (kiemtra != null)
                {
                    string checkemail = newemail.Replace(" ", "");
                    string checknewpass = newpass.Replace(" ", "");
                    if (checkemail.Length > 0 )
                    {
                        if (checknewpass.Length > 0)
                        {
                            kiemtra.matkhau = newpass;
                            kiemtra.email = newemail;
                            db.SaveChanges();
                            return RedirectToAction("Index", "Home", new { mssv });
                        }
                        kiemtra.email = newemail;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home", new { mssv });
                    }
                    else if(checknewpass.Length > 0)
                    {
                        kiemtra.matkhau = newpass;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home", new { mssv });
                    }
                    return RedirectToAction("xemthongtin", new { mssv });
                   



                }
              return RedirectToAction("xemthongtin", new { mssv });

            }



            
        }
    }
}