using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DAO;
using WebApplication1.Models.EF;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;

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

            var result = PDTdao.Instance.login(msnv, Encryptor.MD5Hash(password));
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
        [HttpPost]
        public ActionResult UploadFileSinhvien(HttpPostedFileBase FileUpload)
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
                        DateTime dtDOB = new DateTime();
                        sinhvien sv = new sinhvien();
                        sv.mssv = ((Excel.Range)range.Cells[row, 1]).Text;
                        sv.hoten = ((Excel.Range)range.Cells[row, 2]).Text;
                        dtDOB = DateTime.ParseExact(((Excel.Range)range.Cells[row, 4]).Text, "d/M/yyyy", CultureInfo.InvariantCulture);
                        sv.ngaysinh = dtDOB;
                        sv.diachi = ((Excel.Range)range.Cells[row, 5]).Text;
                        sv.sdt = ((Excel.Range)range.Cells[row, 6]).Text;
                        sv.email = ((Excel.Range)range.Cells[row, 7]).Text;
                        sv.matkhau = ((Excel.Range)range.Cells[row, 8]).Text;
                        sv.lop = ((Excel.Range)range.Cells[row, 9]).Text;
                        sv.makhoa = ((Excel.Range)range.Cells[row, 10]).Text;
                        sv.gioitinh = Boolean.Parse(((Excel.Range)range.Cells[row, 11]).Text);
                        var kiemtratrung = SinhvienDAO.Instance.GetSVByMSSV(sv.mssv);
                        if (kiemtratrung != null)
                        {
                            Session["ErrorMess"] = "Đã có sv!";
                            return RedirectToAction("Index", "PDT");
                        }
                        else
                        {
                            db.sinhviens.Add(sv);
                            db.SaveChanges();
                        }

                    }
                    application.Workbooks.Close();
                    var lst = db.diems.ToList();
                    Session["ErrorMess"] = "Success!";
                    return RedirectToAction("Index", "PDT");



                }
                else
                {
                    Session["ErrorMess"] = "File type is incorrect<br>";
                    return RedirectToAction("Index", "PDT");
                }
            }
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
            sinhvien svcu = SinhvienDAO.Instance.GetSVByMSSV(sv.mssv);
            if (ngaysinh != "")
                svcu.ngaysinh = DateTime.Parse(ngaysinh);
            if (gioitinh != null)
            {
                if (gioitinh == "Nam")
                    svcu.gioitinh = true;
                else
                    svcu.gioitinh = false;
            }
            SinhvienDAO.Instance.Edit(svcu);
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

            if(diemqt >=0 && diemgk >=0 && diemck >=0)
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
            else
            {
                string message = "Điểm không được là số âm!";
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
        #region Bảng môn học
        public ActionResult XemMonHoc()
        {
            List<monhoc> lst = MonHocDao.Instance.GetList();
            return View(lst);
        }
        [HttpGet]
        public ActionResult SuaMonHoc(string ma)
        {
            monhoc mh = MonHocDao.Instance.getmonhocbyID(ma);
            return View(mh);
            
           
        }
        [HttpPost]
        public ActionResult SuaMonHoc(string ma,string tenmonhoc,int sotc, string makhoa)
        {
            monhoc mh = MonHocDao.Instance.getmonhocbyID(ma);
            var kiemtramakhoa = db.khoas.SingleOrDefault(x => x.makhoa == makhoa);
            string ten = tenmonhoc.Replace(" ", "");
            if (sotc > 0 && kiemtramakhoa != null && ten.Length >0) 
            {

                mh.tenmh = tenmonhoc;
                mh.sotc = sotc;
                mh.makhoa = makhoa;
                db.SaveChanges();
                Session["ErrorMess"] = "Success!";
                return RedirectToAction("XemMonHoc");
            }
           else
            {
                Session["ErrorMess"] = "Sửa không thành công";
                return RedirectToAction("SuaMonHoc", new { ma });
            }



        }
        public ActionResult XoaMonHoc(string ma)
        {
            var result = MonHocDao.Instance.delete(ma);
            if (result)
            {
                Session["ErrorMess"] = "Success!";
                return RedirectToAction("XemMonHoc");
            }
            else
            {
                Session["ErrorMess"] = "Xoá không thành công!";
                return RedirectToAction("XemMonHoc");
            }

        }
        [HttpGet]
        public ActionResult ThemMonHoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMonHoc(string tenmonhoc,int sotc,string makhoa)
        {
            string mamoi = "";
            string mamonhocmoinhat = "";
            string ten = tenmonhoc.Replace(" ", "");
            var kiemtramakhoa = db.khoas.SingleOrDefault(x => x.makhoa == makhoa);
            mamonhocmoinhat= MonHocDao.Instance.laymonhocmoinhat();//lấy ra mã môn học mới nhất 
            //xử lý mã môn học mới nhất, cắt phần số tăng lên 1 sau đó ghép lại với phần chữ
            string maso = mamonhocmoinhat.Substring(2);
            int masomoi = int.Parse(maso) +1;
            if (masomoi < 10)
                mamoi = "MH00" + masomoi.ToString();
            else if (masomoi < 100)
                mamoi = "MH0" + masomoi.ToString();
            else
                mamoi = "MH" + masomoi.ToString();
            //kết thúc xử lý mã mới
            if (ten.Length > 0 && sotc > 0 && kiemtramakhoa != null)
            {
                monhoc mh = new monhoc();
                mh.mamh = mamoi;
                mh.tenmh = tenmonhoc;
                mh.sotc = sotc;
                mh.makhoa = makhoa;
                MonHocDao.Instance.insert(mh);
                Session["ErrorMess"] = "Success!";
                return RedirectToAction("XemMonHoc");
            }
            else
            {
                Session["ErrorMess"] = "Thêm không thành công!";
                return RedirectToAction("ThemMonHoc");
            }
        }

        #endregion








    }
}
