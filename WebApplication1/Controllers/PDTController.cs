using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DAO;
using WebApplication1.Models.EF;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace WebApplication1.Controllers
{
    public class PDTController : Controller
    {
        // GET: PDT
        Model1 db = new Model1();
        public ActionResult Index()
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "PDT");
            else
            {
                var list = SinhvienDAO.Instance.GetListSV();
            return View(list);
            }
        }
        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string msnv, string password)
        {
            var result = PDTdao.Instance.login(msnv, password);
            if (result)
            {
                var nhanvien = PDTdao.Instance.getNhanVienByID(msnv);
                Session["taikhoanadmin"] = nhanvien;
                Session["MSNV"] = nhanvien.msnv;
                Session["PASS"] = nhanvien.matkhau;
                return RedirectToAction("Index", "PDT");
            }
            else
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

        }
        #endregion
        #region Bảng SV
        [HttpGet]
        public ActionResult InsertSV()
        {
            //if (Session["taikhoanadmin"] == null)
            //    return RedirectToAction("Login", "PDT");
            //else
            return View();
        }
        [HttpPost]
        public ActionResult InsertSV(sinhvien sv, string ngaysinh, string email, string gioitinh)
        {
            string str, str1;

            List<sinhvien> lst = SinhvienDAO.Instance.GetNewestSV();
            foreach (sinhvien sinhvienmoinhat in lst)
            {
                str = sinhvienmoinhat.mssv;
                str1 = str.Substring(0, 2);
                int str2 = int.Parse(str.Substring(2));
                str2++;
                sv.mssv = str1 + str2.ToString();

            }


            if (ngaysinh != "")
                sv.ngaysinh = DateTime.Parse(ngaysinh);
            if (email != "")
                sv.email = email;
            if (gioitinh != null)
            {
                if (gioitinh == "Nam")
                    sv.gioitinh = true;
                else
                    sv.gioitinh = false;
            }
            SinhvienDAO.Instance.Insert(sv);
            Session["ThongBao"] = "Thêm thành công";
            return RedirectToAction("Index", "PDT");
        }

        public ActionResult DeleteSV(string mssv)
        {
            var result = SinhvienDAO.Instance.Delete(mssv);
            if (result)
            {
                Session["ThongBao"] = "Xoá Thành Công";
                return RedirectToAction("Index", "PDT");
            }
            else
            {
                Session["ThongBao"] = "Xoá Không Thành Công";
                return RedirectToAction("Index", "PDT");

            }


        }
        [HttpGet]
        public ActionResult EditSV(string mssv)
        {
            sinhvien sv = SinhvienDAO.Instance.GetSVByMSSV(mssv);
            return View(sv);
        }
        [HttpPost]
        public ActionResult EditSV(sinhvien sv, string gioitinh, string ngaysinh)
        {
            if (ngaysinh != "")
                sv.ngaysinh = DateTime.Parse(ngaysinh);
            if (gioitinh != null)
            {
                if (gioitinh == "Nam")
                    sv.gioitinh = true;
                else
                    sv.gioitinh = false;
            }
            SinhvienDAO.Instance.Edit(sv);
            Session["ThongBao"] = "Sửa Thành Công";
            return RedirectToAction("Index", "PDT");



        }

        #endregion
        #region Bảng Điểm
        public PartialViewResult XemDiem(string tenmh, string tennhom, int hocky, int nam)
        {
            List<DiemMonHoc> lst = new List<DiemMonHoc>();
            var result = from nhom in db.nhoms
                         join chitiet in db.chitietdks on nhom.manhom equals chitiet.manhom
                         join diem in db.diems on chitiet.madiem equals diem.madiem
                         join monhoc in db.monhocs on nhom.mamh equals monhoc.mamh
                         join dk in db.dkmonhocs on chitiet.madk equals dk.madk
                         join hk in db.hockies on dk.madk equals hk.madk
                         join sv in db.sinhviens on dk.madk equals sv.madk
                         where nhom.tennhom.Contains(tennhom) && monhoc.tenmh == tenmh && hk.hocky1 == hocky && hk.nam == nam
                         select new DiemMonHoc()
                         {
                             Mssv = sv.mssv,
                             Hoten = sv.hoten,
                             Madiem = diem.madiem,
                             DiemQT = diem.diemQT,
                             DiemGK = diem.diemGK,
                             DiemCK = diem.diemCK,
                             Tongdiem = diem.tongdiem

                         };
            lst = result.ToList();
            return PartialView("XemDiem", lst);

        }
        [HttpPost]
        public ActionResult DeleteDiem(string ma)
        {
            var result = ChiTietdkDAO.Instance.delete(ma);
            if (result)
            {
                string message = "Thành công!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string message = "Không thành công!";
                return Json(message, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public ActionResult UploadFileDiem(HttpPostedFileBase FileUpload)
        {

            if (FileUpload == null || FileUpload.ContentLength == 0)
            {
                Session["ErrorMess"] = "Please select a excel file<br>";
                return RedirectToAction("Score", "PDT");
            }
            else
            {
                if (FileUpload.FileName.EndsWith("xls") || (FileUpload.FileName.EndsWith("xlsx")))
                {
                    string location = Server.MapPath("~/Content/" + FileUpload.FileName);
                    if (System.IO.File.Exists(location))
                        System.IO.File.Delete(location);
                    FileUpload.SaveAs(location);
                    //Read data from excel
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(location);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        diem diem = new diem();
                        diem.madiem = ((Excel.Range)range.Cells[row, 1]).Text;
                        diem.diemQT = double.Parse(((Excel.Range)range.Cells[row, 2]).Text);
                        diem.diemGK = double.Parse(((Excel.Range)range.Cells[row, 3]).Text);
                        diem.diemCK = double.Parse(((Excel.Range)range.Cells[row, 4]).Text);
                        diem.C_diemQT = double.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                        diem.C_diemGK = double.Parse(((Excel.Range)range.Cells[row, 6]).Text);
                        diem.C_diemck = double.Parse(((Excel.Range)range.Cells[row, 7]).Text);
                        diem.solanthi = int.Parse(((Excel.Range)range.Cells[row, 8]).Text);
                        diem.tongdiem = double.Parse(((Excel.Range)range.Cells[row, 9]).Text);
                        db.diems.Add(diem);
                        db.SaveChanges();
                    }
                    var lst = db.diems.ToList();
                    Session["ErrorMess"] = "Success!";
                    return RedirectToAction("Score", "PDT");



                }
                else
                {
                    Session["ErrorMess"] = "File type is incorrect<br>";
                    return RedirectToAction("Score", "PDT");
                }
            }



        }
        public ActionResult EditDiem(string madiem, double? diemqt, double? diemgk, double? diemck)
        {

            var result = DiemDAO.Instance.edit(madiem, diemqt, diemgk, diemck);
            if (result)
            {
                string message = "Thành công!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string message = "Không thành công!";
                return Json(message, JsonRequestBehavior.AllowGet);

            }


        }
        public ViewResult Score()
        {
            List<nhom> lst = NhomDAO.Instance.GetListNhom();
            List<monhoc> lstmh = MonHocDao.Instance.GetList();
            ViewBag.Monhoc = lstmh;
            return View(lst);
        }
        #endregion






        public ActionResult GetNhom(string mamh)
        {
            nhom n = db.nhoms.SingleOrDefault(x => x.mamh == mamh);
            return Json(n, JsonRequestBehavior.AllowGet);
        }


    }
}
