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
using System.Runtime.InteropServices;
using ExcelLibrary.SpreadSheet;
using System.Text;
using System.Net.Mail;

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
        //Tạo chuỗi ngẫu nhiên
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        //
        [HttpGet]
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LayMatKhau(string email)
        {
            PDT nv = db.PDTs.SingleOrDefault(x => x.email == email);
            if (nv == null)
            {
                Session["ErrorMess"] = "Email này không tồn tại!";
                return RedirectToAction("Login", "PDT");
            }
            string newpass = RandomString(10, false);
            nv.matkhau = Encryptor.MD5Hash(newpass);
            db.SaveChanges();
            StringBuilder Body = new StringBuilder();
            //Tạo body mail
            Body.Append("<table>");
            Body.Append("<tr><td colspan='2'><h4>Lấy lại mật khẩu</h4></td></tr>");
            Body.Append("<tr><td>Mật khẩu mới của bạn là:</td><td>" + newpass + "</td></tr>");
            Body.Append("<tr><td>Vui lòng đăng nhập bằng tài khoản này để đổi lại mật khẩu</td></tr>");
            Body.Append("</table>");
            //

            //Cài đặt mail
            MailMessage mail = new MailMessage();
            mail.To.Add(nv.email);
            mail.From = new MailAddress("stucaolo180@gmail.com");
            mail.Subject = "Trả lời về vấn đề quên mật khẩu của nhân viên";
            mail.Body = Body.ToString();// phần thân của mail ở trên
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("stucaolo180@gmail.com", "Smile123");// tài khoản Gmail của bạn
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return View();


            //
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
                    string location = Server.MapPath("~/FileExcel/" + FileUpload.FileName);
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
                    application.Quit();
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
      
        public ActionResult ExportSV()
        {
            #region code chạy local được nhưng không chạy trên server dc
            List<sinhvien> lst = new List<sinhvien>();
            lst = db.sinhviens.ToList();
            Excel.Application application = new Excel.Application();
            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            worksheet.Cells[1, 1] = "MSSV";
            worksheet.Cells[1, 2] = "Họ Tên";
            worksheet.Cells[1, 3] = "Giới tính";
            worksheet.Cells[1, 4] = "Ngày Sinh";
            worksheet.Cells[1, 5] = "Số điện thoại";
            worksheet.Cells[1, 6] = "Địa chỉ";
            worksheet.Cells[1, 7] = "Email";
            worksheet.Cells[1, 8] = "Lớp";
            worksheet.Cells[1, 9] = "Mã Khoa";
            worksheet.Cells[1, 10] = "Mã đăng ký môn học";
            int row = 2;
            foreach (sinhvien sv in lst)
            {
                worksheet.Cells[row, 1] = sv.mssv;
                worksheet.Cells[row, 2] = sv.hoten;
                if (sv.gioitinh == true)
                {
                    worksheet.Cells[row, 3] = "Nam";
                }
                else
                {
                    worksheet.Cells[row, 3] = "Nữ";
                }
                if (sv.ngaysinh != null)
                {
                    DateTime dt = DateTime.ParseExact(sv.ngaysinh.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

                    string s = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    worksheet.Cells[row, 4] = s;
                }
                else
                {
                    worksheet.Cells[row, 4] = sv.ngaysinh;
                }

                worksheet.Cells[row, 5] = sv.sdt;
                worksheet.Cells[row, 6] = sv.diachi;
                worksheet.Cells[row, 7] = sv.email;
                worksheet.Cells[row, 8] = sv.lop;
                worksheet.Cells[row, 9] = sv.makhoa;
                worksheet.Cells[row, 10] = sv.madk;
                row++;
            }
            worksheet.get_Range("A1", "K1").EntireColumn.AutoFit();
            //Format Heading
            var range_Heading = worksheet.get_Range("A1", "J1");
            range_Heading.Font.Bold = true;
            range_Heading.Font.Color = System.Drawing.Color.Red;
            range_Heading.Font.Size = 13;

            //Format date
            //var range_date = worksheet.get_Range("D2","D"+(lst.Count+1));
            //range_date.NumberFormat("mm/dd/yyyy");

            workbook.SaveAs("d:\\DanhSachSV.xls");
            workbook.Close();
            Marshal.ReleaseComObject(workbook);

            application.Quit();
            Marshal.FinalReleaseComObject(application);
            #endregion
            #region 1 cách export khác đã thử nhưng vẫn ko chạy dc trên server
            ////create new xls file
            //string file = "d:\\newdoc.xls";
            //List<sinhvien> lst = new List<sinhvien>();
            //lst = db.sinhviens.ToList();
            //Workbook workbook = new Workbook();
            //Worksheet worksheet = new Worksheet("First Sheet");
            //worksheet.Cells[1, 1] = new Cell("MSSV");
            //worksheet.Cells[1, 2] = new Cell("Họ Tên");
            //worksheet.Cells[1, 3] = new Cell("Giới tính");
            //worksheet.Cells[1, 4] = new Cell("Ngày Sinh");
            //worksheet.Cells[1, 5] = new Cell("Số điện thoại");
            //worksheet.Cells[1, 6] = new Cell("Địa chỉ");
            //worksheet.Cells[1, 7] = new Cell("Email");
            //worksheet.Cells[1, 8] = new Cell("Lớp");
            //worksheet.Cells[1, 9] = new Cell("Mã Khoa");
            //worksheet.Cells[1, 10] = new Cell("Mã đăng ký môn học");
            //int row = 2;
            //foreach (sinhvien sv in lst)
            //{
            //    worksheet.Cells[row, 1] = new Cell(sv.mssv);
            //    worksheet.Cells[row, 2] = new Cell(sv.hoten);
            //    if (sv.gioitinh == true)
            //    {
            //        worksheet.Cells[row, 3] = new Cell("Nam");
            //    }
            //    else
            //    {
            //        worksheet.Cells[row, 3] = new Cell("Nữ");
            //    }
            //    if (sv.ngaysinh != null)
            //    {
            //        DateTime dt = DateTime.ParseExact(sv.ngaysinh.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            //        string s = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            //        worksheet.Cells[row, 4] = new Cell(s);
            //    }
            //    else
            //    {
            //        worksheet.Cells[row, 4] = new Cell(sv.ngaysinh);
            //    }

            //    worksheet.Cells[row, 5] = new Cell(sv.sdt);
            //    worksheet.Cells[row, 6] = new Cell(sv.diachi);
            //    worksheet.Cells[row, 7] = new Cell(sv.email);
            //    worksheet.Cells[row, 8] = new Cell(sv.lop);
            //    worksheet.Cells[row, 9] = new Cell(sv.makhoa);
            //    worksheet.Cells[row, 10] = new Cell(sv.madk);
            //    row++;
            //}



            //workbook.Worksheets.Add(worksheet);
            //workbook.Save(file);

            ////// open xls file 
            ////Workbook book = Workbook.Load(file);
            ////Worksheet sheet = book.Worksheets[0];

            #endregion

            Session["ErrorMess"] = "Success!";
            return RedirectToAction("Index");
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
        public ActionResult EditSV(string ma,string hoten,string diachi,string sdt, string gioitinh, string ngaysinh,string lop,string email,string madk,string makhoa)
        {
            
            string checkhoten = hoten.Replace(" ", "");
            string checkdiachi = diachi.Replace(" ", "");
            string checklop = lop.Replace(" ", "");
            string checkemail = email.Replace(" ", "");
            string checkmadk = madk.Replace(" ", "");
            string checkmakhoa = makhoa.Replace(" ", "");
            


            //Kiểm tra rỗng
            if (checkhoten.Length == 0 || checkdiachi.Length==0 || checklop.Length == 0 || checkemail.Length == 0 || checkmadk.Length == 0 || checkmakhoa.Length == 0) 
            {
                Session["ErrorMess"] = "Không được để trống thông tin";
                return RedirectToAction("EditSV",new { mssv= ma });
            }
           else if((db.dkmonhocs.SingleOrDefault(x=>x.madk == madk))==null)
            {
                Session["ErrorMess"] = "Mã dk ko tồn tại!";
                return RedirectToAction("EditSV", new { mssv = ma });
            }

            else if ((db.khoas.SingleOrDefault(x => x.makhoa == makhoa)) == null)
            {
                Session["ErrorMess"] = "Mã khoa ko tồn tại!";
                return RedirectToAction("EditSV", new { mssv = ma });
            }

            else
            {
                sinhvien sinhvien = new sinhvien();
                sinhvien.mssv = ma;
                sinhvien.hoten = hoten;
                sinhvien.email = email;
                sinhvien.diachi = diachi;
                sinhvien.lop = lop;
                sinhvien.madk = madk;
                sinhvien.makhoa = makhoa;
                sinhvien.ngaysinh = DateTime.Parse(ngaysinh);
                sinhvien.sdt = sdt;
                sinhvien.gioitinh = bool.Parse(gioitinh);
                SinhvienDAO.Instance.Edit(sinhvien);
                Session["ErrorMess"] = "Success!";
                return RedirectToAction("Index");
            }
    


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
                Session["ErrorMess"] = "Please select a excel file";
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
                    Session["ErrorMess"] = "File type is incorrect!";
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

        #region Thông tin tài khoản
        [HttpGet]
        public ActionResult TaiKhoan()
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "PDT");
            else
            {
                PDT nhanvien = PDTdao.Instance.getNhanVienByID(Session["MSNV"].ToString());
                return View(nhanvien);
            }
           

        }
        [HttpPost]
        public ActionResult SuaTaiKhoan(string ma, string hoten, string email,string sdt)
        {
            string checkten = hoten.Replace(" ", "");
            string checkemail = email.Replace(" ", "");
            string checksdt = sdt.Replace(" ", "");
            int a = int.Parse(sdt);// Kiểm tra dấu của sdt
            

            if (checkten.Length >0 && checkemail.Length >0 && checksdt.Length >=10 && a > 0 )
            {
              
               
                    Session["ErrorMess"] = "Success!";
                    PDTdao.Instance.Sua(ma, hoten, email, sdt);
                return RedirectToAction("Index");
               
            }
           
            else
            {
                Session["ErrorMess"] = "Fail!";
                return RedirectToAction("TaiKhoan");
            }
           

        }
        [HttpGet]
        public ActionResult DoiMatKhau(string ma)
        {
            PDT nv = PDTdao.Instance.getNhanVienByID(ma);
            return View(nv);
        }
        [HttpPost]
        public ActionResult DoiMatKhau(string ma,string oldpassword, string newpassword, string newpasswordAgain)
        {
           if(newpassword == newpasswordAgain)
            {
                var kq = PDTdao.Instance.doimatkhau(ma,Encryptor.MD5Hash(oldpassword),newpassword);
                if(kq)
                {
                    Session["ErrorMess"] = "Success!";
                    return RedirectToAction("Index");
                }
                Session["ErrorMess"] = "Mật khẩu cũ không đúng!";
                return RedirectToAction("DoiMatKhau", new { ma });
            }
           else
            {
                Session["ErrorMess"] = "Nhập lại mật khẩu chưa đúng!";
                return RedirectToAction("DoiMatKhau", new { ma });
            }
           
        }
        public ActionResult DangXuat()
        {
            Session["taikhoanadmin"] = null;
            Session["MSNV"] = null;
            Session["PASS"] = null;
            return RedirectToAction("Index", "PDT");
        }
        #endregion






    }
}
