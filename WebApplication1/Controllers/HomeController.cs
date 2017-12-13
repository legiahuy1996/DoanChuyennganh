using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DAO;
using WebApplication1.Models.EF;
namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string mssv)
        {
            var sess = Session["taikhoan"];
            if (sess == null)
            {
                return RedirectToAction("dangnhap", "SinhVien");
            }
            else
            {
                sinhvien sv = SinhvienDAO.Instance.GetSVByMSSV(mssv);
                







                return View(sv);
            }



        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}