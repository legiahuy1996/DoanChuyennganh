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

                sinhvien sv = SinhvienDAO.Instance.GetSVByMSSV(MSSV);
                dkmonhoc dkmonhoc = DkmonhocDAO.Instance.GetDkmonhocbyID(sv.madk);
                List<chitietdk> lstchitiet = ChiTietdkDAO.Instance.GetListChitietdkByIDdk(dkmonhoc.madk);
                List<diem> lstdiem = new List<diem>();
                foreach (chitietdk chitiet in lstchitiet)
                {
                    diem diem = new diem();
                    diem = DiemDAO.Instance.GetDiemByID(chitiet.madiem);
                    lstdiem.Add(diem);

                }
                ViewBag.Diem = lstdiem;
                return View(sv);
            }

            public ActionResult xemthongtin(string MSSV)
            {
                var taikhoan = Session["taikhoan"];
                var mssv = Session["MSSV"];
                sinhvien sv = new sinhvien();
                if (MSSV == null)
                {
                    return HttpNotFound("Page's not found");


                }
                if (string.Compare(mssv.ToString(), MSSV) == 1 || taikhoan == null)
                {
                    return HttpNotFound("Sorry you have to login!");


                }
                else
                {
                    sv = SinhvienDAO.Instance.GetSVByMSSV(MSSV);
                    return View(sv);
                }

            }

            public ActionResult Dangxuat()
            {
                Session["taikhoan"] = null;
                return RedirectToAction("dangnhap");
            }
            public ActionResult doimatkhau(string oldpass, string newpass, string newpass1, string mssv)
            {

                sinhvien kiemtra = db.sinhviens.SingleOrDefault(x => x.mssv == mssv && x.matkhau == oldpass);
                if (newpass != newpass1)
                {
                    ViewBag.ThongBao = "Nhập lại mật khẩu mới chưa đúng!!";
                    RedirectToAction("xemthongtin", mssv);
                }
                else
                {
                    if (kiemtra != null)
                    {
                        kiemtra.matkhau = newpass;
                        db.SaveChanges();
                    }

                }



                return RedirectToAction("Index", "Home", new { mssv = mssv });
            }
        }
    }