using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using SCRepository.Entity.Models;
using SCRepository.Entity.Repository.RepositoryData;
using StudentCardsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StudentCardsAdmin.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private IConfiguration Configuration;

        public HomeController(IDataContextRepository dataContextRepository, IHostingEnvironment appEnvironment) : base(dataContextRepository)
        {
            _dataContextRepository = dataContextRepository;
            _hostingEnvironment = appEnvironment;
        }

        public IActionResult Index(string get, string sortData, string search, SearchModelView searchModel)
        {
            if (HttpContext.Session.GetString("Authorized") == "1")
            {
                if (get == "1")
                {
                    GenerateUniqueId();
                    return RedirectToAction("Index", "Home");
                }
                //ViewBag.NameSortParm = String.IsNullOrEmpty(sortData) ? "name_desc" : "";
                //ViewBag.ClassSortParm = sortData == "Class" ? "class_desc" : "Class";
                var data_schools = GetSchools();
                if ((HttpContext.Session.GetString("Rights") != "8") && (HttpContext.Session.GetString("CBU") != "0"))
                {
                    data_schools = data_schools.Where(s => s.cbu == HttpContext.Session.GetString("CBU")).ToList();
                }
                if (HttpContext.Session.GetString("Rights") == "1" && HttpContext.Session.GetString("CBU") != "0")
                {
                    data_schools = data_schools.Where(s => s.cbu == HttpContext.Session.GetString("CBU")).ToList();
                }
                var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
                if (id_school == null) { HttpContext.Session.SetString("IdSchool", data_schools[0].id.ToString()); }

                var schoolInfo = GetSchoolData(id_school);
                var resultFromDB = GetStudents(id_school);
                if (!String.IsNullOrEmpty(search))
                {
                    if (search == "-")
                    {
                        resultFromDB = resultFromDB.Where(s => s.unique_id == ("0        ")
                                               /*|| s.Kid_Surname.Contains(search)*/).ToList();
                    }
                    if (search == "!-")
                    {
                        resultFromDB = resultFromDB.Where(s => s.unique_id != ("0        ")).ToList();
                    }
                    if (search != "!-" && search != "-")
                    {
                        resultFromDB = resultFromDB.Where(s => s.unique_id.Replace(" ", "").Contains(search)).ToList();
                    }
                }

                //if (searchModel.SName != null || searchModel.Class != null) {
                //    var sg = searchModel.SName == null ? "!null" : searchModel.SName;
                //    var sg1 = searchModel.Class == null ? "!null" : searchModel.Class;
                //    resultFromDB = resultFromDB.Where(s => s.kid_surname == sg && s.kid_class.Replace(" ", "") == sg1).ToList();
                //}


                if(searchModel.CheckSch != "-" && searchModel.CheckSch != null)
                {
                    HttpContext.Session.SetString("OldIdSchool", searchModel.CheckSch);
                    resultFromDB = GetStudents(Convert.ToInt32(searchModel.CheckSch));
                }
                else { HttpContext.Session.SetString("OldIdSchool", ""); }
                if (searchModel.UniqueId != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.unique_id.Contains(searchModel.UniqueId)).ToList();
                }
                if (searchModel.Name != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.kid_name == searchModel.Name).ToList();
                }
                if (searchModel.SName != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.kid_surname == searchModel.SName).ToList();
                }
                if (searchModel.TName != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.kid_patronymic == searchModel.TName).ToList();
                }
                if (searchModel.Class != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.kid_class.Replace(" ", "").Contains(searchModel.Class.ToLower()) || s.kid_class.Replace(" ", "").Contains(searchModel.Class.ToUpper()) /*== searchModel.Class.ToLower()*/ ).ToList();
                }
                if (searchModel.YearBirthday != null)
                {
                    resultFromDB = resultFromDB.Where(s => s.year_birthday.ToString("dd.MM.yyyy") == searchModel.YearBirthday.ToString()).ToList();
                }
                //if (searchModel?.EndTraining != null)
                //{
                //    var data = searchModel.EndTraining.ToString().Remove(0, 3).Remove(7,8);
                //    resultFromDB = resultFromDB.Where(s => s.end_training.ToString("MM yyyy") == data).ToList();
                //}
                if (searchModel.CheckAct != "0")
                {
                    if (searchModel.CheckAct == "1")
                    {
                        resultFromDB = resultFromDB.Where(s => s.pers_data == 1).ToList();
                    }
                    else if (searchModel.CheckAct == "-1")
                    {
                        resultFromDB = resultFromDB.Where(s => s.pers_data != 1).ToList();
                    }
                }
                if (searchModel.CheckPhoto != "0")
                {
                    if (searchModel.CheckPhoto == "1")
                    {
                        resultFromDB = resultFromDB.Where(s => s.photo_id != 0).ToList();
                    }
                    else if (searchModel.CheckPhoto == "-1")
                    {
                        resultFromDB = resultFromDB.Where(s => s.photo_id == 0).ToList();
                    }
                }

                switch (sortData)
                {
                    case "name_desc":
                        resultFromDB = resultFromDB.OrderByDescending(s => s.kid_name).ToList();
                        break;
                    case "Class":
                        resultFromDB = resultFromDB.OrderBy(s => s.kid_class).ToList();
                        break;
                    case "class_desc":
                        resultFromDB = resultFromDB.OrderByDescending(s => s.kid_class).ToList();
                        break;
                    default:
                        resultFromDB = resultFromDB.OrderBy(s => s.kid_surname).ToList();
                        break;
                }

                var search_school = GetSchools();
                ViewBag.Model2 = search_school;
                ViewBag.Model1 = data_schools;

                if (HttpContext.Session.GetString("Rights") != "2")
                {
                    ViewBag.Model = resultFromDB;
                    ViewBag.ModelCount = resultFromDB.Count();
                }
                else
                {
                    resultFromDB = new List<Students> { };
                    ViewBag.Model = resultFromDB;
                    ViewBag.ModelCount = resultFromDB.Count();
                }
                ViewBag.School = schoolInfo;

                //var resultSch = GetSchool();
                #region ToDo
                /// <summary>
                /// GetSchool()
                /// Получить из сессиии инфу по школе 
                /// </summary> 
                /// <remarks>
                /// В дальнейшем вытягивать определенных школьников
                /// Разбие по классам 
                /// Создание групп
                /// </remarks>
                #endregion

                //ViewBag.Utv = resultSch.Where(x => x.IsApproved == 1).OrderBy(x => x.Name); //Разбить школоту по группам 


                return View();
                }
            else
                return RedirectToAction("Index", "Autorization");
        }
        //public IActionResult _EditKid()
        //{
        //    return PartialView();
        //}
        public JsonResult _EditKid(string id)
        {
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
            if (HttpContext.Session.GetString("OldIdSchool") != "")
            {
                id_school = Convert.ToInt32(HttpContext.Session.GetString("OldIdSchool"));
            }
            var resultEtcData = GetStudentsEtc(id_school, id);
            var result = from e in resultEtcData
                         select new {
                             id = e.id,
                             unique_id = e.unique_id,
                             nonfin_app_num = e.nonfin_app_num,
                             kid_name = e.kid_name,
                             kid_surname =e.kid_surname,
                             kid_patronymic = e.kid_patronymic,
                             year_birthday = e.year_birthday.ToString("dd.MM.yyyy"),
                             school_id = e.school_id,
                             kid_class = e.kid_class,
                             end_training = e.end_training.ToString("MM.yyyy"),
                             n_telephone = e.n_telephone,
                             kid_email = e.kid_email,
                             pers_data = e.pers_data,
                             date_update = e.date_update,
                             photo_id = e.photo_id,
                             status_zayavka_id = e.status_zayavka_id,
                             card_template = e.card_template,
                             stud_status = e.stud_status,
                             state_id = e.state_id 
            };
                                  
            return Json(result.ToList());
        }

        public JsonResult PhotoStudent(string id)
        {
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
            if(HttpContext.Session.GetString("OldIdSchool") != "") { 
                id_school = Convert.ToInt32(HttpContext.Session.GetString("OldIdSchool")); 
            }
            var resultFromDBPhoto = GetStudentsPhoto(id_school, id);
            var result = from e in resultFromDBPhoto
                         select new
                         {
                             unique_id = e.stud_id,
                             photo = Convert.ToBase64String(e.photo)

                         };
            return Json(result.ToList());
        }
        public JsonResult EtcDataStudent(string id)
        {//Check
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
            if (HttpContext.Session.GetString("OldIdSchool") != "")
            {
                id_school = Convert.ToInt32(HttpContext.Session.GetString("OldIdSchool"));
            }
            var resultFromDBPhoto = GetStudentsEtc(id_school, id);
            var result = from e in resultFromDBPhoto
                         select new
                         {
                             unique_id = e.unique_id,
                             nonfin_app_num = e.nonfin_app_num,
                             n_telephone = e.n_telephone,
                             kid_email = e.kid_email,
                             anketa = e.pers_data,
                             date_update = e.date_update,
                             status_zayavka_id = e.status_zayavka_id
                         };
            return Json(result.ToList());
        }
        public JsonResult TransferdStud(string unique_id, string data)
        {
            var resultFromDBTransfer = GetTransferdStud(unique_id, data);
            var result = from e in resultFromDBTransfer
                         select new
                         {
                             unique_id = e.unique_id,
                             kid_name = e.kid_name,
                             kid_surname = e.kid_surname,
                             kid_patronymic = e.kid_patronymic,
                             year_birthday = e.year_birthday.ToString("dd.MM.yyy"),
                             kid_class = e.kid_class.Replace(" ",""),
                             end_training = e.end_training.ToString("MM.yyy"),
                             school_name_short = e.school_name_short,
                             sch_id = e.sch_id
                         };
            return Json(result.ToList());
        }
        public JsonResult EditableSchool(string id)
        {
            var resultFromDBTransfer = GetSchoolData(Convert.ToInt32(id));
            var result = from e in resultFromDBTransfer
                         select new
                         {
                             id = e.id,
                             unn = e.unn,
                             cbu = e.cbu,
                             school_name = e.school_name,
                             school_name_short = e.school_name_short,
                             school_name_card = e.school_name_card,
                             school_address = e.school_address,
                             school_phone = e.school_phone,
                             locality = e.locality,
                             district = e.district,
                             region = e.region,
                             email = e.email
                             
                         };
            return Json(result.ToList());
        }

        public JsonResult CheckSession()
        {
            if (HttpContext.Session.GetString("Authorized") == "1")
            {
                return Json("OK");
            }
            else
                return Json("Bad");
        }

            [HttpGet]
        public IActionResult DelSchool(string id)
        {
            var school = GetSchoolData(Convert.ToInt32(id));
            var result = DeleteSchool(Convert.ToInt32(id));
            //var result = "1";
            if (result == "1")
            {
                TempData["success"] = $"Учреждение образования с УНП: \"{school[0].unn}\" успешно удалено";
                DataForProtocol("Удаление школы ", "Успешно", $"Школа  {school[0].unn}, удалена пользователем {HttpContext.Session.GetString("Username")}");
                return RedirectToAction("Index", "Home", "SCSchool");
            }
            else
            {
                TempData["fail"] = $"Учреждение образования с УНП: \"{school[0].unn}\" не удалено Ошибка: {result}";
                DataForProtocol("Удаление школы ", "Ошибка", $"Школа  {school[0].unn}, не удалена пользователем {HttpContext.Session.GetString("Username")} /{result}");
                return RedirectToAction("Index", "Home", "SCSchool");
            }
            //return RedirectToAction("Index", "Home", "SCSchool");
            //return Json("ok");
        }

        [HttpGet]
        public JsonResult ChangeSchool(string id)
        {
            if (id == "-") { return Json("ok"); }
            HttpContext.Session.SetString("IdSchool", id);
            //return RedirectToAction("Index");
            return Json("ok");
        }

        [HttpGet]
        public IActionResult SetAct(string set)
        {
            var jsonObject = JsonNode.Parse(set);
            dynamic jf = JsonConvert.DeserializeObject(jsonObject.ToString());
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
              if (HttpContext.Session.GetString("OldIdSchool") != "")
            {
                id_school = Convert.ToInt32(HttpContext.Session.GetString("OldIdSchool"));
            }
            try
            {
                foreach (int item in jf)
                {
                    var resultFromDBPhoto = SetKidAct(id_school, item);
                }
                TempData["success"] = $"Акты подтвержены успешно";
                DataForProtocol("Подтвержение наличия актов", "Успешно", $"Анкеты учеников {jf}, школы {id_school}");
            }

            catch (Exception e)
            {
                TempData["fail"] = $"Ошибка акты не подтверждены: {e.Message}";
                DataForProtocol("Подтвержение наличия актов", "Ошибка", $"Анкеты учеников {jf}, школы {id_school}/ Ошибка {e.Message}");
            }
            return RedirectToAction("Index");
        }

        public string GenerateUniqueId()
        {
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
            var resultFromDB = SetUniqueId(id_school);
            return "1";
        }

        public IActionResult AddKid()
        {
            return View();
        }

        public IActionResult ActKid(string unique_id)
        {
            var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
            if (HttpContext.Session.GetString("OldIdSchool") != "")
            {
                id_school = Convert.ToInt32(HttpContext.Session.GetString("OldIdSchool"));
            }
            var resultFromDBAct = GetStudentsEtc(id_school, unique_id);
            var resultFromDBPhoto = GetStudentsPhoto(id_school, resultFromDBAct[0].photo_id.ToString());
            var resultSchool = GetSchoolData(id_school);
            ViewBag.Model = resultFromDBAct;
            ViewBag.Photo = resultFromDBPhoto;
            ViewBag.School = resultSchool;
            #region

            //            string chipIng = CreateMD5(resultFromDBAct[0].id.ToString());
            //            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
            //            Image barcodeImage = barcode.Encode(BarcodeLib.TYPE.CODE128, chipIng, Color.Black, Color.White, 290, 120);

            //            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            //            string sFileName = "ImageCode/"+ unique_id + ".jpg";
            //            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            //            var memory = new MemoryStream();
            //            try { 
            //            using (FileStream fs = System.IO.File.Create(Path.Combine(sWebRootFolder, sFileName)))
            //            {

            //                barcodeImage.Save(fs, ImageFormat.Jpeg);
            //                fs.Position = 0;
            //              /*  barcodeImage.Dispose()*/;
            //                //var img = Image.FromStream(fs);
            //                //img.Save(Path.Combine(sWebRootFolder, sFileName);
            //            }
            //}catch(Exception e) { }
            //var stream = ToStream(barcodeImage,ImageFormat.Jpeg);

            //barcodeImage.Save($"wwwroot\\ImageCode\\{unique_id}.jpg",System.Drawing.Imaging.ImageFormat.Jpeg);
            //barcodeImage.Dispose();
            //byte[] imageArray = System.IO.File.ReadAllBytes($"wwwroot\\ImageCode\\{unique_id}.jpg");
            //string base64Image = Convert.ToBase64String(imageArray);
            //ViewBag.Code = base64Image;
            #endregion
            return View();
        }

        public IActionResult EditKid()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            if (HttpContext.Session.GetString("Authorized") == "1")
            {
                var data_schools = GetSchools();
                ViewBag.Model1 = data_schools;
                return View();
            }
            else
                return RedirectToAction("Index", "Autorization");
        }

        public IActionResult Report()
        {
        if (HttpContext.Session.GetString("Authorized") == "1")
        {
            var data_schools = GetSchools().OrderBy(s => s.id);
            List<ReportModel> listReport = new List<ReportModel>();
            foreach (SchoolModel item in data_schools) {
            var school = data_schools.Where(s => s.id == item.id).ToList();
            var resultFromDB = GetStudents(Convert.ToInt32(school[0].id));
                string countStud = resultFromDB.Count.ToString();
                string emptyPhoto = resultFromDB.Where(s => s.photo_id == 0).Count().ToString();
                string emptyUnId = resultFromDB.Where(s => s.unique_id == "0        ").Count().ToString();
                ReportModel report = new ReportModel{
                   id = school[0].id,
                   unn = school[0].unn,
                   school_name_short = school[0].school_name_short,
                   count_students = countStud +"| Отсутствуют фото: "+emptyPhoto + "| Отсутствует ID-учащегося: " + emptyUnId,
                   students = resultFromDB
                };
                listReport.Add(report);
            }
            ViewBag.Model = listReport;
            return View();
        }
         else
        return RedirectToAction("Index", "Autorization");
    }

        public async Task<IActionResult> SaveReportAll()
        {
            string h3 = "";

            DateTime now = DateTime.Now;
            string sFileName = "Files/Отчет_Общий" + now.Day + now.Month + now.Hour + now.Minute + now.Second + ".xlsx";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            //int id = Convert.ToInt32(info.Split('_')[0]);
            //string type = info.Split('_')[1];
            //DateTime datefrom = DateTime.Parse(info.Split('_')[2]);
            //DateTime dateto = DateTime.Parse(info.Split('_')[3]);

            h3 = "Общий отчет";
            string thName = "";

            var resultFromDB = ReportGetData();

          
            //h3 += $"с {datefrom.ToShortDateString()} по {dateto.ToShortDateString()}";

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet");
                IRow row = excelSheet.CreateRow(0);
                NPOI.SS.UserModel.ICell cell = row.CreateCell(0);
                ICellStyle cellStyleHeader = workbook.CreateCellStyle();
                ICellStyle cellStylerow1 = workbook.CreateCellStyle();
                ICellStyle cellStylerow2 = workbook.CreateCellStyle();
                ICellStyle cellStylerow3 = workbook.CreateCellStyle();
                var font = workbook.CreateFont();

                cell.SetCellValue(h3);

                CellRangeAddress cra = new CellRangeAddress(0, 0, 0, 5);
                excelSheet.AddMergedRegion(cra);
                cellStyleHeader.VerticalAlignment = VerticalAlignment.Top;
                cellStyleHeader.Alignment = HorizontalAlignment.Center;
                cellStyleHeader.WrapText = true;
                font.IsBold = true;
                font.FontHeightInPoints = 11;
                cellStyleHeader.SetFont(font);
                cell.CellStyle = cellStyleHeader;


                IRow row1 = excelSheet.CreateRow(1);
                NPOI.SS.UserModel.ICell cell1_0 = row1.CreateCell(0);
                cell1_0.SetCellValue("№ \n п/п");
                NPOI.SS.UserModel.ICell cell1_1 = row1.CreateCell(1);
                cell1_1.SetCellValue("Наименование УО");
                NPOI.SS.UserModel.ICell cell1_2 = row1.CreateCell(2);
                cell1_2.SetCellValue("Кол-во поступивших");
                NPOI.SS.UserModel.ICell cell1_3 = row1.CreateCell(3);
                cell1_3.SetCellValue("Кол-во неполных записей");
                NPOI.SS.UserModel.ICell cell1_4 = row1.CreateCell(4);
                cell1_4.SetCellValue("Кол-во оформленных заявок");
                NPOI.SS.UserModel.ICell cell1_5 = row1.CreateCell(5);
                cell1_5.SetCellValue("Кол-во сформированных макетов");

                cellStylerow1.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow1.Alignment = HorizontalAlignment.Center;
                cellStylerow1.WrapText = true;
                cellStylerow1.BorderTop = BorderStyle.Medium;
                cellStylerow1.BorderLeft = BorderStyle.Medium;
                cellStylerow1.BorderRight = BorderStyle.Medium;
                cellStylerow1.BorderBottom = BorderStyle.Medium;

                cell1_0.CellStyle = cellStylerow1;
                cell1_1.CellStyle = cellStylerow1;
                cell1_2.CellStyle = cellStylerow1;
                cell1_3.CellStyle = cellStylerow1;
                cell1_4.CellStyle = cellStylerow1;
                cell1_5.CellStyle = cellStylerow1;

                excelSheet.SetColumnWidth(1, 15000);
                excelSheet.SetColumnWidth(2, 4000);
                excelSheet.SetColumnWidth(3, 5000);
                excelSheet.SetColumnWidth(4, 5000);
                excelSheet.SetColumnWidth(5, 5000);

                int i =2;
                int j = 1;
                NPOI.SS.UserModel.ICell cell2_0;
                NPOI.SS.UserModel.ICell cell2_1;
                NPOI.SS.UserModel.ICell cell2_2;
                NPOI.SS.UserModel.ICell cell2_3;
                NPOI.SS.UserModel.ICell cell2_4;
                NPOI.SS.UserModel.ICell cell2_5;

                cellStylerow2.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow2.Alignment = HorizontalAlignment.Center;
                cellStylerow2.WrapText = true;
                cellStylerow2.BorderTop = BorderStyle.Thin;
                cellStylerow2.BorderLeft = BorderStyle.Thin;
                cellStylerow2.BorderRight = BorderStyle.Thin;
                cellStylerow2.BorderBottom = BorderStyle.Thin;

                cellStylerow3.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow3.Alignment = HorizontalAlignment.Left;
                cellStylerow3.WrapText = true;
                cellStylerow3.BorderTop = BorderStyle.Thin;
                cellStylerow3.BorderLeft = BorderStyle.Thin;
                cellStylerow3.BorderRight = BorderStyle.Thin;
                cellStylerow3.BorderBottom = BorderStyle.Thin;

                foreach (var item in resultFromDB)
                {
                    row = excelSheet.CreateRow(i);
                    cell2_0 = row.CreateCell(0);
                    cell2_0.SetCellValue(j);
                    cell2_1 = row.CreateCell(1);
                    cell2_1.SetCellValue(item.Name);
                    cell2_2 = row.CreateCell(2);
                    cell2_2.SetCellValue(item.CountStudents);
                    cell2_3 = row.CreateCell(3);
                    cell2_3.SetCellValue(item.CountDefective);
                    cell2_4 = row.CreateCell(4);
                    cell2_4.SetCellValue(item.CountZayavleni);
                    cell2_5 = row.CreateCell(5);
                    cell2_5.SetCellValue(item.CountTemplate);
                    i++;
                    j++;

                    cell2_0.CellStyle = cellStylerow3;
                    cell2_1.CellStyle = cellStylerow3;
                    cell2_2.CellStyle = cellStylerow2;
                    cell2_3.CellStyle = cellStylerow2;
                    cell2_4.CellStyle = cellStylerow2;
                    cell2_5.CellStyle = cellStylerow2;
                }
                workbook.Write(fs,true);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> SaveReportDate(string start, string end)
        {
            string h3 = "";

            DateTime now = DateTime.Now;
            string sFileName = "Files/Отчет_Изменений" + now.Day + now.Month + now.Hour + now.Minute + now.Second + ".xlsx";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            h3 = "Изменения ";
            var resultFromDB = ReportChanges(start, end);


            h3 += $"с {start} по {end}";

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet");
                IRow row = excelSheet.CreateRow(0);
                NPOI.SS.UserModel.ICell cell = row.CreateCell(0);
                ICellStyle cellStyleHeader = workbook.CreateCellStyle();
                ICellStyle cellStylerow1 = workbook.CreateCellStyle();
                ICellStyle cellStylerow2 = workbook.CreateCellStyle();
                ICellStyle cellStylerow3 = workbook.CreateCellStyle();
                var font = workbook.CreateFont();

                cell.SetCellValue(h3);

                CellRangeAddress cra = new CellRangeAddress(0, 0, 0, 5);
                excelSheet.AddMergedRegion(cra);
                cellStyleHeader.VerticalAlignment = VerticalAlignment.Top;
                cellStyleHeader.Alignment = HorizontalAlignment.Center;
                cellStyleHeader.WrapText = true;
                font.IsBold = true;
                font.FontHeightInPoints = 11;
                cellStyleHeader.SetFont(font);
                cell.CellStyle = cellStyleHeader;


                IRow row1 = excelSheet.CreateRow(1);
                NPOI.SS.UserModel.ICell cell1_0 = row1.CreateCell(0);
                cell1_0.SetCellValue("№ \n п/п");
                NPOI.SS.UserModel.ICell cell1_1 = row1.CreateCell(1);
                cell1_1.SetCellValue("Наименование УО");
                NPOI.SS.UserModel.ICell cell1_2 = row1.CreateCell(2);
                cell1_2.SetCellValue("Кол-во изменений по ученикам");
                NPOI.SS.UserModel.ICell cell1_3 = row1.CreateCell(3);
                cell1_3.SetCellValue("Кол-во неполных записей");
                NPOI.SS.UserModel.ICell cell1_4 = row1.CreateCell(4);
                cell1_4.SetCellValue("Кол-во оформленных заявок");
                NPOI.SS.UserModel.ICell cell1_5 = row1.CreateCell(5);
                cell1_5.SetCellValue("Кол-во сформированных макетов");

                cellStylerow1.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow1.Alignment = HorizontalAlignment.Center;
                cellStylerow1.WrapText = true;
                cellStylerow1.BorderTop = BorderStyle.Medium;
                cellStylerow1.BorderLeft = BorderStyle.Medium;
                cellStylerow1.BorderRight = BorderStyle.Medium;
                cellStylerow1.BorderBottom = BorderStyle.Medium;

                cell1_0.CellStyle = cellStylerow1;
                cell1_1.CellStyle = cellStylerow1;
                cell1_2.CellStyle = cellStylerow1;
                cell1_3.CellStyle = cellStylerow1;
                cell1_4.CellStyle = cellStylerow1;
                cell1_5.CellStyle = cellStylerow1;

                excelSheet.SetColumnWidth(1, 15000);
                excelSheet.SetColumnWidth(2, 4000);
                excelSheet.SetColumnWidth(3, 5000);
                excelSheet.SetColumnWidth(4, 5000);
                excelSheet.SetColumnWidth(5, 5000);

                int i = 2;
                int j = 1;
                NPOI.SS.UserModel.ICell cell2_0;
                NPOI.SS.UserModel.ICell cell2_1;
                NPOI.SS.UserModel.ICell cell2_2;
                NPOI.SS.UserModel.ICell cell2_3;
                NPOI.SS.UserModel.ICell cell2_4;
                NPOI.SS.UserModel.ICell cell2_5;

                cellStylerow2.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow2.Alignment = HorizontalAlignment.Center;
                cellStylerow2.WrapText = true;
                cellStylerow2.BorderTop = BorderStyle.Thin;
                cellStylerow2.BorderLeft = BorderStyle.Thin;
                cellStylerow2.BorderRight = BorderStyle.Thin;
                cellStylerow2.BorderBottom = BorderStyle.Thin;

                cellStylerow3.VerticalAlignment = VerticalAlignment.Top;
                cellStylerow3.Alignment = HorizontalAlignment.Left;
                cellStylerow3.WrapText = true;
                cellStylerow3.BorderTop = BorderStyle.Thin;
                cellStylerow3.BorderLeft = BorderStyle.Thin;
                cellStylerow3.BorderRight = BorderStyle.Thin;
                cellStylerow3.BorderBottom = BorderStyle.Thin;

                foreach (var item in resultFromDB)
                {
                    row = excelSheet.CreateRow(i);
                    cell2_0 = row.CreateCell(0);
                    cell2_0.SetCellValue(j);
                    cell2_1 = row.CreateCell(1);
                    cell2_1.SetCellValue(item.Name);
                    cell2_2 = row.CreateCell(2);
                    cell2_2.SetCellValue(item.CountStudents);
                    cell2_3 = row.CreateCell(3);
                    cell2_3.SetCellValue(item.CountDefective);
                    cell2_4 = row.CreateCell(4);
                    cell2_4.SetCellValue(item.CountZayavleni);
                    cell2_5 = row.CreateCell(5);
                    cell2_5.SetCellValue(item.CountTemplate);
                    i++;
                    j++;

                    cell2_0.CellStyle = cellStylerow3;
                    cell2_1.CellStyle = cellStylerow3;
                    cell2_2.CellStyle = cellStylerow2;
                    cell2_3.CellStyle = cellStylerow2;
                    cell2_4.CellStyle = cellStylerow2;
                    cell2_5.CellStyle = cellStylerow2;
                }
                workbook.Write(fs, true);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }


        public IActionResult SCSchool()//Справочник школ
        {
            if (HttpContext.Session.GetString("Authorized") == "1")
            {

                if (HttpContext.Session.GetString("Rights") == "2")
                {
                    //var id_school = HttpContext.Session.GetString("IdSchool");
                    //var resultFromDB = GetSchoolData(Convert.ToInt32(id_school));
                    var data_schools = GetSchools();
                    data_schools = data_schools.Where(s => s.cbu == HttpContext.Session.GetString("CBU")).ToList();
                    //HttpContext.Session.SetString("IdSchool", data_schools[0].id.ToString());
                    ViewBag.Model1 = data_schools;
                }
                if (HttpContext.Session.GetString("Rights") == "1")
                {
                    var data_schools = GetSchools();
                    if (HttpContext.Session.GetString("CBU") != "0")
                    {
                        data_schools = data_schools.Where(s => s.cbu == HttpContext.Session.GetString("CBU")).ToList();
                    }
                    ViewBag.Model1 = data_schools;
                }

                if (HttpContext.Session.GetString("Rights") == "8")
                {
                    var data_schools = GetSchools();
                    ViewBag.Model1 = data_schools;
                    ViewBag.Model2 = data_schools.Count();
                }
                if ((HttpContext.Session.GetString("CBU") == "0") && (HttpContext.Session.GetString("Rights") == "2") || (HttpContext.Session.GetString("Rights") == "4"))
                {
                    var data_schools = GetSchools();
                    ViewBag.Model1 = data_schools;
                    ViewBag.Model2 = data_schools.Count();
                }
                return View();
            }
            else
               return View();
                //return RedirectToAction("Index", "Autorization");
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveKid(StudentsView students)
        {

            if (students.ChboxNedCard == false)
            {
                students.PersData = 0;
            }
            else { students.PersData = 1; }
            if (students.SchoolID == 0)
            {
                var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
                students.SchoolID = id_school;
            }

            var result = SaveKidData(students);
            if (result == "1")
            {
                return RedirectToAction("Index", "Home", "AddKid");
            }
            else
            {
                return View("Index", students);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveEditKid(EditKid editKid, PhotoModelView pmv)
        {

            var result = UpdateStudent(editKid);
            pmv.Name = editKid.id_edit.ToString();
            Create(pmv);
            if (result == "1")
            {
                TempData["success"] = $"Ученик: \"{editKid.unique_id_edit}\" успешно изменен:";
                DataForProtocol("Изменение ученика", "Успешно", $"Ученик  {editKid.unique_id_edit}, изменен пользователем {HttpContext.Session.GetString("Username")}");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["fail"] = $"Ученик: \"{editKid.unique_id_edit}\" не изменен. Ошибка: {result}";
                DataForProtocol("Изменение ученика", "Ошибка", $"Ученик  {editKid.unique_id_edit}, не изменен пользователем {HttpContext.Session.GetString("Username")} /{result}");
                return RedirectToAction("Index", "Home");
            }
           
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UpdateTransfer(string UniqueID, string SchoolTransfer, string Class, string EndData)
        {
            if (SchoolTransfer != "-" && Class != null && EndData != null && UniqueID !=null)
            {
              
                var result = UpdateKidTransfer(UniqueID, SchoolTransfer,Class,EndData);
                
                if (result == "1")
                {
                    TempData["success"] = $"Ученик: \"{UniqueID}\" успешно переведен в УО:";
                    DataForProtocol("Перевод ученика", "Успешно", $"Ученик  {UniqueID}, переведен пользователем {HttpContext.Session.GetString("Username")} в УО {SchoolTransfer}");
                    return RedirectToAction("Transfer");
                }
                else
                {
                    TempData["fail"] = $"Ученик: \"{UniqueID}\" не переведен. Ошибка: {result}";
                    DataForProtocol("Перевод ученика", "Ошибка", $"Ученик  {UniqueID}, не переведен пользователем {HttpContext.Session.GetString("Username")} в УО {SchoolTransfer} /{result}");
                    return RedirectToAction("Transfer");
                }
            }
            else
            {
                TempData["fail"] = $"Ученик: \"{UniqueID}\" не переведен. Ошибка: проверьте заполняемые поля";
                DataForProtocol("Перевод ученика", "Ошибка", $"Ученик  {UniqueID}, не переведен пользователем {HttpContext.Session.GetString("Username")} в УО {SchoolTransfer} /Ошибка: проверьте заполняемые поля");
                return RedirectToAction("Transfer");
            }
           
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveSchool(SchoolModelView school)
        {
            if (school.cbu == null) { 
            school.cbu = HttpContext.Session.GetString("CBU");}
            //if (ModelState.IsValid) { 
                var result = SaveSchData(school);
            if (result == "1")
            {
                TempData["success"] = $"Школа: \"{school.school_name_short}\" успешно добавлена для ЦБУ: \"{school.cbu}\"";
                DataForProtocol("Добавление школы ", "Успешно", $"Школа  {school.school_name_short}, добавлена пользователем {HttpContext.Session.GetString("Username")}");
                return RedirectToAction("Index", "Home", "SCSchool");
            }
            else
            {
                TempData["fail"] = $"Школа: \"{school.school_name_short}\" не добавлена для ЦБУ: \"{school.cbu}\" Ошибка: {result}";
                DataForProtocol("Добавление школы ", "Ошибка", $"Школа  {school.school_name_short}, не добавлена пользователем {HttpContext.Session.GetString("Username")} /{result}");
                return RedirectToAction("Index", "Home", "SCSchool");
            }
            //}
            //else
            //{
            //    TempData["fail"] = $"Школа: \"{school.school_name_short}\" не добавлена для ЦБУ: \"{school.cbu}\" Ошибка: ПРОВЕРЬТЕ ВВОДИМЫЕ ДАННЫЕ";
            //    DataForProtocol("Добавление школы ", "Ошибка", $"Школа  {school.school_name_short}, не добавлена пользователем {HttpContext.Session.GetString("Username")} /ОШИБКА ВАЛИДАЦИИ {ModelState.Keys()}");
            //    return RedirectToAction("Index", "Home", "SCSchool");
            //}
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditSchool(EditSchoolModelView updSchool)
        {         
                SchoolModelView school = new SchoolModelView
                {
                    id = updSchool.id_edit,
                    unn = updSchool.unn_edit,
                    school_name = updSchool.school_name_edit,
                    school_name_short = updSchool.school_name_short_edit,
                    school_name_card = updSchool.school_name_card_edit,
                    school_address = updSchool.school_address_edit,
                    school_phone = updSchool.school_phone_edit,
                    email = updSchool.email_edit,
                    district = updSchool.district_edit,
                    region = updSchool.region_edit,
                    locality = updSchool.locality_edit,
                    cbu = updSchool.cbu_edit == null ? HttpContext.Session.GetString("CBU") : updSchool.cbu_edit
                };

                var result = UpdateSchData(school);

                if (result == "1")
                {
                    TempData["success"] = $"Школа: \"{school.school_name_short}\" успешно обновлена";
                    DataForProtocol("Обновление школы ", "Успешно", $"Школа  {school.school_name_short}, обновлена пользователем {HttpContext.Session.GetString("Username")}");
                    //return RedirectToAction("SCSchool");
                    return RedirectToAction("Index", "Home", "SCSchool");
                }
                else
                {
                    TempData["fail"] = $"Школа: \"{school.school_name_short}\" не обновлена Ошибка: {result}";
                    DataForProtocol("Обновление школы ", "Ошибка", $"Школа {school.school_name_short}, не обновлена пользователем {HttpContext.Session.GetString("Username")} /{result}");
                    return RedirectToAction("Index", "Home", "SCSchool");
                }
            
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(PhotoModelView fmv)
        {
            if (fmv.Name != "0        ")
            {
                PhotoModel photo = new PhotoModel
                {
                    stud_id = fmv.Name
                };
                if (fmv.Photo != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(fmv.Photo.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)fmv.Photo.Length);
                    }
                    // установка массива байтов
                    photo.photo = imageData;
                    var id_school = Convert.ToInt32(HttpContext.Session.GetString("IdSchool"));
                    var result = InsertPhoto(photo);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public string SaveKidData(StudentsView student)
        {
            Students students = new Students
            {
                kid_name = student.Name,
                kid_surname = student.SureName,
                kid_patronymic = student.PatronymicName,
                year_birthday = student.YearBirthday.Date,
                kid_class = student.Class,
                end_training = student.EndTraining,
                n_telephone = student.Telephone,
                kid_email = student.Email,
                pers_data = student.PersData,
                school_id = student.SchoolID
            };
            var result = InsertStudent(students);
            return result;
        }
        public string SaveSchData(SchoolModelView school)
        {
            SchoolModel schoolModel = new SchoolModel
            {
                unn = school.unn,
                school_name = school.school_name,
                school_name_short = school.school_name_short,
                school_name_card = school.school_name_card,
                school_address = school.school_address,
                school_phone = school.school_phone,
                email = school.email,
                district = school.district,
                region = school.region,
                locality = school.locality,
                cbu = school.cbu
            };
            var result = InsertSchool(schoolModel);
            return result;
        }

        public string UpdateSchData(SchoolModelView school)
        {
            SchoolModel schoolModel = new SchoolModel
            {
                id = school.id,
                unn = school.unn,
                school_name = school.school_name,
                school_name_short = school.school_name_short,
                school_name_card = school.school_name_card,
                school_address = school.school_address,
                school_phone = school.school_phone,
                email = school.email,
                district = school.district,
                region = school.region,
                locality = school.locality,
                cbu = school.cbu
            };
            var result = UpdateSchool(schoolModel);
            return result;
        }

        //public string SavePhoto(PhotoModelView photoView)
        //{
        //    PhotoModel photoModel = new PhotoModel
        //    {
        //        stud_id = photoView.Name,
        //        photo = photoView.Photo

        //    };

        //    var result = InsertStudent(photoModel);
        //    return result;
        //}
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
