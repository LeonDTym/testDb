using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCRepository.Entity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SCRepository.Entity.Repository.RepositoryData
{
    public class DataContextRepository : IDataContextRepository
    {

        private bool disposedValue;
        private DBContext _studentDBContext;

        public DataContextRepository()
        {
            _studentDBContext = new DBContext();
        }

        public List<Logs> GetLogs()
        {
            var  data = _studentDBContext.Logs.ToList();
            return data;
        }

        public string SetLogs(Logs logs)
        {
            try
            {
                SqlParameter time = new SqlParameter("@time", DateTime.Now);
                SqlParameter st_name = new SqlParameter("@st_name", logs.stantionName);
                SqlParameter st_ip = new SqlParameter("@st_ip", logs.stationIp);
                SqlParameter type = new SqlParameter("@type", logs.actionType);
                SqlParameter result_log = new SqlParameter("@result_log", logs.actionResult);
                SqlParameter details = new SqlParameter("@details", logs.actionDetails);
                SqlParameter system = new SqlParameter("@system", logs.system);
                var result = _studentDBContext.Database.ExecuteSqlRaw("addLog @time, @st_name, @st_ip, @result_log, @type, @details, @system", time, st_name, st_ip, result_log, type, details, system);
                //_studentDBContext.Logs.Add(logs);
                //_studentDBContext.Logs.Update(logs);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }

        public string SetLogs_Out(Logs_Out logs_out)
        {
            try
            {
                SqlParameter unp = new SqlParameter("@unp", logs_out.UNP);
                SqlParameter time = new SqlParameter("@time", DateTime.Now);
                SqlParameter st_name = new SqlParameter("@st_name", logs_out.stantionName);
                SqlParameter st_ip = new SqlParameter("@st_ip", logs_out.stationIp);
                SqlParameter type = new SqlParameter("@type", logs_out.actionType);
                SqlParameter result_log = new SqlParameter("@result_log", logs_out.actionResult);
                SqlParameter details = new SqlParameter("@details", logs_out.actionDetails);
                SqlParameter system = new SqlParameter("@system", logs_out.system);
                var result = _studentDBContext.Database.ExecuteSqlRaw("addLog_Out @unp, @time, @st_name, @st_ip, @result_log, @type, @details, @system", unp, time, st_name, st_ip, result_log, type, details, system);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }

        #region Для сайта 

        public List<Students> Students(int id)
        {
            try { 
            SqlParameter param = new SqlParameter("@id", id);
            //SqlParameter id_param = new SqlParameter("@id", id);
            var result = _studentDBContext.Students.FromSqlRaw("[dbo].[getKid] @id", param).ToList();
            return result;
            }catch(Exception e)
            {
                var stud = new List<Students>();
                return null;
            }
        }
        public List<Students> Students_In(int id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@id", id);
                //SqlParameter id_param = new SqlParameter("@id", id);
                var result = _studentDBContext.Students.FromSqlRaw("[dbo].[getKid_In] @id", param).ToList();
                return result;
            }
            catch (Exception e)
            {
                var stud = new List<Students>();
                return stud;
            }
        }

        public List<SchoolModel> GetSchoolData(int id)
        {
            SqlParameter param = new SqlParameter("@id", id);
            var result = _studentDBContext.School.FromSqlRaw("[dbo].[getSchool] @id", param).ToList();
            return result;
        }
        public List<PhotoModel> PhotoModels(int school_id, string id)
        {
            SqlParameter paramSch = new SqlParameter("@idSch", school_id);
            SqlParameter paramId = new SqlParameter("@id", id);
            //SqlParameter id_param = new SqlParameter("@id", id);
            var result = _studentDBContext.StudentPhoto.FromSqlRaw("[dbo].[getKidPhoto] @idSch, @id", paramSch, paramId).ToList();
            return result;
        }
        public List<Students> StudentEtc(int id, string unid)
        {
            SqlParameter paramSch = new SqlParameter("@idSch", id);
            SqlParameter paramId = new SqlParameter("@unique_id", unid);
            //SqlParameter id_param = new SqlParameter("@id", id);
            var result = _studentDBContext.Students.FromSqlRaw("[dbo].[getKidDataEtc] @idSch, @unique_id", paramSch, paramId).ToList();
            return result;
        }
        public string SetTransfer(int sch_id, int st_id)
        {
            try {
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                SqlParameter errorCode = new SqlParameter
                { 
                    ParameterName = "@errorCode",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[setKidTransfer]  @sch_id, @st_id, @errorCode OUT", param, param2, errorCode);
                
                    if (errorCode.Value.ToString() == "0")
                    {
                        return st_id.ToString();
                    }
                    else if (errorCode.Value.ToString() == "101")
                    {
                        throw new Exception("Отсутствует уникальный ID-учащегося");
                    }
                    else
                    {
                        throw new Exception("Учащийся не найден");
                    }
            }
            catch (Exception e) 
            {
                return $"{e.Message}"; 
            }
        }

        public TransferBlank GetTransferBlank(int st_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@st_id", st_id);
                var result = _studentDBContext.TransferBlank.FromSqlRaw("[dbo].[getTransferBlank] @st_id",  param).ToList();
                return result.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Transfer> GetTransfer(string unique_id, string data)
        {
            SqlParameter paramSch = new SqlParameter("@unique_id", unique_id);
            SqlParameter paramId = new SqlParameter("@year_bth", Convert.ToDateTime(data));
            var result = _studentDBContext.Transfer.FromSqlRaw("[dbo].[getTransferWeb]  @unique_id, @year_bth", paramSch, paramId).ToList();
            return result;
        }

        public string UpdateKidTransfer(string uniqueID, string schoolTransfer, string classN, string endData)
        {
            try
            {
                endData = "01." + endData;

                SqlParameter paramUnId = new SqlParameter("@unique_id", uniqueID);
                SqlParameter paramEnD = new SqlParameter("@end_data", Convert.ToDateTime(endData).AddMonths(1).AddDays(-1));
                SqlParameter paramCl = new SqlParameter("@class", classN);
                SqlParameter paramSchId = new SqlParameter("@schoolTransfer", Convert.ToInt32(schoolTransfer));
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[updTransferKid]  @unique_id, @end_data, @class, @schoolTransfer", paramUnId, paramEnD, paramCl, paramSchId);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }

        public List<Report> Report()
        {
            var result = _studentDBContext.Reports.FromSqlRaw("[dbo].[report]").ToList();
            return result;
        }
        public List<Report> ReportDate(string start, string end)
        {
            SqlParameter pStart = new SqlParameter("@start", Convert.ToDateTime(start));
            SqlParameter pEnd = new SqlParameter("@end", Convert.ToDateTime(end).AddHours(23).AddMinutes(59));
            var result = _studentDBContext.Reports.FromSqlRaw("[dbo].[reportDate] @start, @end", pStart, pEnd).ToList();
            return result;
        }

        public string InsertStudent(Students students)
        {
            try
            {
                //SqlParameter paramId = new SqlParameter("@id", editKid.id_edit);
                //SqlParameter paramUnId = new SqlParameter("@unique_id", students.unique_id_edit);
                SqlParameter paramKName = new SqlParameter("@kid_name", students.kid_name);
                SqlParameter paramKSur = new SqlParameter("@kid_surname", students.kid_surname);
                SqlParameter paramKPatr = new SqlParameter("@kid_patronymic", students.kid_patronymic);
                SqlParameter paramYbth = new SqlParameter("@year_birthday", students.year_birthday);
                SqlParameter paramScID = new SqlParameter("@sch_id", students.school_id);
                SqlParameter paramCl = new SqlParameter("@class", students.kid_class);
                SqlParameter paramEnD = new SqlParameter("@end_data", Convert.ToDateTime(students.end_training).AddMonths(1).AddDays(-1));
                SqlParameter paramTel = new SqlParameter("@tel", students.n_telephone);
                SqlParameter paramEml = new SqlParameter("@email", students.kid_email);
                //SqlParameter paramStZv = new SqlParameter("@status_zayavka_id", editKid.status_zayavka_id_edit);
                //SqlParameter paramCtmp = new SqlParameter("@card_template", editKid.card_template_edit);
                SqlParameter paramPer = new SqlParameter("@pers_data", students.pers_data);
                SqlParameter paramLastId = new SqlParameter
                {
                    ParameterName = "@last_id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                //SqlParameter paramStSta = new SqlParameter("@stud_status", editKid.stud_status_edit);
                paramEml.Value = (object)students.kid_email ?? DBNull.Value;
                paramKPatr.Value = (object)students.kid_patronymic ?? DBNull.Value;
                paramTel.Value = (object)students.n_telephone ?? DBNull.Value;
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[addKid]   @kid_name, @kid_surname, @kid_patronymic, @year_birthday, @sch_id," +
                    " @class, @end_data, @tel, @email," +
                    " @pers_data, @last_id OUT", paramKName, paramKSur, paramKPatr, paramYbth, paramScID, paramCl, paramEnD, paramTel, paramEml, paramPer, paramLastId);

                //var result = _studentDBContext.Database.ExecuteSqlRaw("INSERT INTO dbo.Students (Kid_Name, Kid_Surname, Kid_Patronymic, Year_Birthday, School_ID, Kid_Class, End_Training, N_Telephone, Kid_Email, Unique_Id, photo_id,status_zayavka_id) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", students.kid_name, students.kid_surname, students.kid_patronymic, students.year_birthday.Date, students.school_id, students.kid_class, students.end_training, students.n_telephone, students.kid_email, "0", "000000000", 0);
                return paramLastId.Value.ToString();
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string InsertMultiStudent(Students students)
        {
            try
            {
                //SqlParameter paramId = new SqlParameter("@id", editKid.id_edit);
                //SqlParameter paramUnId = new SqlParameter("@unique_id", students.unique_id_edit);
                SqlParameter paramKName = new SqlParameter("@kid_name", students.kid_name);
                SqlParameter paramKSur = new SqlParameter("@kid_surname", students.kid_surname);
                SqlParameter paramKPatr = new SqlParameter("@kid_patronymic", students.kid_patronymic);
                SqlParameter paramYbth = new SqlParameter("@year_birthday", students.year_birthday);
                SqlParameter paramScID = new SqlParameter("@sch_id", students.school_id);
                SqlParameter paramCl = new SqlParameter("@class", students.kid_class);
                SqlParameter paramEnD = new SqlParameter("@end_data", students.end_training);
                SqlParameter paramTel = new SqlParameter("@tel", students.n_telephone);
                SqlParameter paramEml = new SqlParameter("@email", students.kid_email);
                //SqlParameter paramStZv = new SqlParameter("@status_zayavka_id", editKid.status_zayavka_id_edit);
                //SqlParameter paramCtmp = new SqlParameter("@card_template", editKid.card_template_edit);
                SqlParameter paramPer = new SqlParameter("@pers_data", students.pers_data);
                SqlParameter paramLastId = new SqlParameter
                {
                    ParameterName = "@last_id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                //SqlParameter paramStSta = new SqlParameter("@stud_status", editKid.stud_status_edit);
                paramEml.Value = (object)students.kid_email ?? DBNull.Value;
                paramKPatr.Value = (object)students.kid_patronymic ?? DBNull.Value;
                paramTel.Value = (object)students.n_telephone ?? DBNull.Value;
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[addKid]   @kid_name, @kid_surname, @kid_patronymic, @year_birthday, @sch_id," +
                    " @class, @end_data, @tel, @email," +
                    " @pers_data, @last_id OUT", paramKName, paramKSur, paramKPatr, paramYbth, paramScID, paramCl, paramEnD, paramTel, paramEml, paramPer, paramLastId);
                return "-1";
            }
            catch (Exception e)
            {
                return $"Ошибка, {e} / {students.id} ";
            }
        }

        public string UpdateStudent(EditKid editKid)
        {
            try
            {
                SqlParameter paramId = new SqlParameter("@id", editKid.id_edit);
                SqlParameter paramUnId = new SqlParameter("@unique_id", editKid.unique_id_edit);
                SqlParameter paramKName = new SqlParameter("@kid_name", editKid.kid_name_edit);
                SqlParameter paramKSur = new SqlParameter("@kid_surname", editKid.kid_surname_edit);
                SqlParameter paramKPatr = new SqlParameter("@kid_patronymic", editKid.kid_patronymic_edit);
                SqlParameter paramYbth = new SqlParameter("@year_birthday", editKid.year_birthday_edit);
                SqlParameter paramScID = new SqlParameter("@sch_id", editKid.school_id_edit);
                SqlParameter paramCl = new SqlParameter("@class", editKid.kid_class_edit);
                SqlParameter paramEnD = new SqlParameter("@end_data", Convert.ToDateTime(editKid.end_training_edit).AddMonths(1).AddDays(-1));
                SqlParameter paramTel = new SqlParameter("@tel", editKid.n_telephone_edit);
                SqlParameter paramEml = new SqlParameter("@email", editKid.kid_email_edit);
                SqlParameter paramStZv = new SqlParameter("@status_zayavka_id", editKid.status_zayavka_id_edit);
                SqlParameter paramCtmp = new SqlParameter("@card_template", editKid.card_template_edit);
                SqlParameter paramPer = new SqlParameter("@pers_data", editKid.pers_data_edit);
                SqlParameter paramStSta = new SqlParameter("@stud_status", editKid.stud_status_edit);
                paramEml.Value = (object)editKid.kid_email_edit ?? DBNull.Value;
                paramKPatr.Value = (object)editKid.kid_patronymic_edit ?? DBNull.Value;
                paramTel.Value = (object)editKid.n_telephone_edit ?? DBNull.Value;
                paramStSta.Value = editKid.stud_status_edit == 0 ? 1 : paramStSta.Value;
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[editKid]  @id, @unique_id, @kid_name, @kid_surname, @kid_patronymic, @year_birthday, @sch_id," +
                    " @class, @end_data, @tel, @email," +
                    " @status_zayavka_id, @card_template, @pers_data, @stud_status", paramId, paramUnId, paramKName, paramKSur, paramKPatr, paramYbth, paramScID, paramCl, paramEnD, paramTel, paramEml, paramStZv, paramCtmp, paramPer, paramStSta);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }

        public string UpdateSchool(SchoolModel schoolModel)
        {
            try
            {
                SqlParameter unn = new SqlParameter("@unn", schoolModel.unn);
                var check = _studentDBContext.School.FromSqlRaw("SELECT * FROM dbo.School WHERE unn= @unn", unn).Where(s => s.id != schoolModel.id).Count();
                if (check != 0)
                {
                    return "Школа с таким УНП уже существует";
                }
                else
                {
                    SqlParameter id = new SqlParameter("@id", schoolModel.id);
                    SqlParameter school_name = new SqlParameter("@school_name", schoolModel.school_name);
                    SqlParameter school_name_short = new SqlParameter("@school_name_short", schoolModel.school_name_short);
                    SqlParameter school_name_card = new SqlParameter("@school_name_card", schoolModel.school_name_card);
                    SqlParameter school_address = new SqlParameter("@school_address", schoolModel.school_address);
                    SqlParameter school_phone = new SqlParameter("@school_phone", schoolModel.school_phone);
                    SqlParameter region = new SqlParameter("@region", schoolModel.region);
                    SqlParameter district = new SqlParameter("@district", schoolModel.district);
                    SqlParameter locality = new SqlParameter("@locality", schoolModel.locality);
                    SqlParameter email = new SqlParameter("@email", schoolModel.email);
                    SqlParameter cbu = new SqlParameter("@cbu", schoolModel.cbu);
                    var result = _studentDBContext.Database.ExecuteSqlRaw("updateSchool @id, @unn, @school_name, @school_name_short, @school_name_card, @school_address, @school_phone, @region, @district, @locality, @email, @cbu", id, unn, school_name, school_name_short, school_name_card, school_address, school_phone, region, district, locality, email, cbu);
                    return "1";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string DeleteSchool(int _id)
        {
            try
            {
                SqlParameter id = new SqlParameter("@id", _id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("deleteSchool @id", id);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string InsertSchool(SchoolModel schoolModel)
        {
            try
            {
                SqlParameter unn = new SqlParameter("@unn", schoolModel.unn);
                var check = _studentDBContext.School.FromSqlRaw("SELECT * FROM dbo.School WHERE unn= @unn", unn).Count();
                if (check != 0) {
                    return "Школа с таким УНП уже существует";
                }
                else {
                    var result = _studentDBContext.Database.ExecuteSqlRaw("INSERT INTO dbo.School (unn, school_name, school_name_short, school_name_card, school_address, school_phone, region, district, email, cbu, locality) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", schoolModel.unn, schoolModel.school_name, schoolModel.school_name_short, schoolModel.school_name_card, schoolModel.school_address, schoolModel.school_phone, schoolModel.region, schoolModel.district, schoolModel.email, schoolModel.cbu, schoolModel.locality);
                    return "1";
                }
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string InsertPhoto(PhotoModel photo)
        {
            try
            {
                //var result = _studentDBContext.Database.ExecuteSqlRaw(" INSERT INTO dbo.StudentPhoto (unique_id, photo) VALUES ({0},{1})", photo.Name,photo.Photo);
                SqlParameter name = new SqlParameter("@name", photo.stud_id);
                SqlParameter photoPic = new SqlParameter("@photo", photo.photo);
                var result = _studentDBContext.Database.ExecuteSqlRaw("addPhoto @name, @photo", name, photoPic);

                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string SetPersState(int sch_id, int st_id)
        {
            try
            {
                SqlParameter errorCode = new SqlParameter
                {
                    ParameterName = "@ErrorCode",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("setKidPersData @sch_id, @st_id, @ErrorCode OUT", param, param2, errorCode);
                return errorCode.Value.ToString();
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        //Todo Anketa
        public string SetAnketaState(int sch_id, int st_id)
        {
            try
            {
                SqlParameter errorCode = new SqlParameter
                {
                    ParameterName = "@ErrorCode",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("setKidAnketa @sch_id, @st_id, @ErrorCode OUT", param, param2, errorCode);
                return errorCode.Value.ToString();
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string RemovePersState(int sch_id, int st_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("removeKidPersData @sch_id, @st_id", param, param2);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string UpgradeClass(int sch_id, int st_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                SqlParameter errorCode = new SqlParameter
                {
                    ParameterName = "@ErrorCode",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[setClassUp]  @sch_id, @st_id, @ErrorCode OUT", param, param2, errorCode);
                return errorCode.Value.ToString();
            }
            catch (Exception e)
            {
                return $"{e.Message}";
            }
        }
        public (ExportUID, string) CheckImportUID(int st_id)
        {
            try
            {
                //SqlParameter param = new SqlParameter("@sch_id", sch_id);
                SqlParameter param2 = new SqlParameter("@st_id", st_id);
                SqlParameter errorCode = new SqlParameter
                {
                    ParameterName = "@ErrorCode",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                var result = _studentDBContext.ExportUID.FromSqlRaw("[dbo].[getUID]  @st_id, @ErrorCode OUT", param2, errorCode).ToList();
                return (result.First(), errorCode.Value.ToString());
                //if (errorCode.Value.ToString() == "0")
                //{
                //    return st_id.ToString();
                //}
                //else if (errorCode.Value.ToString() == "1")
                //{
                //    throw new Exception("Диапозон класса может быть только не меньше 1 и не превышать 11");
                //}
                //else
                //{
                //    throw new Exception("Учащийся не найден");
                //}
            }
            catch (Exception e)
            {
                return (null, $"{e.Message}");
            }
        }
        public string SetUniqueId(int sch_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@sch_id", sch_id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("setUnique_Id @sch_id", param);
                return "1";
            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string SetExclude(int st_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@st_id", st_id);
                var result = _studentDBContext.Database.ExecuteSqlRaw("setExclude @st_id", param);
                return "1";
            }
            catch(Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        public string DeleteStudent(int st_id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@st_id", st_id);
                SqlParameter paramLastId = new SqlParameter
                {
                    ParameterName = "@last_id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var result = _studentDBContext.Database.ExecuteSqlRaw("[dbo].[deleteStudent]   @st_id, @last_id OUT", param, paramLastId);
                
                    return paramLastId.Value.ToString();

            }
            catch (Exception e)
            {
                return $"Ошибка {e.Message}";
            }
        }
        #endregion

        #region Для API

        public StudentsAPI GetStudentInf(string unique_id, string year)
        {
            var result = from s in _studentDBContext.Students
                         join sch in _studentDBContext.School on s.school_id equals sch.id
                         where s.unique_id == unique_id && s.year_birthday == Convert.ToDateTime(year) && s.pers_data == 1
                         select new StudentsAPI { school_id = sch.id, school_name = sch.school_name, school_name_short = sch.school_name_short, end_training = s.end_training, kid_surname = s.kid_surname, kid_name = s.kid_name, kid_patronymic = s.kid_patronymic };
            return result.FirstOrDefault();
        }
        //public List<Students> GetStudentByUnId(int unique_id, string year)
        //{
        //    SqlParameter paramUnId = new SqlParameter("@unique_id", unique_id);
        //    SqlParameter paramYear = new SqlParameter("@year", Convert.ToDateTime(year));
        //    var result = _studentDBContext.Students.FromSqlRaw("[dbo].[getKidByUniqueId] @unique_id,@year", paramUnId, paramYear).ToList();
        //    return result;
        //}
        public StatusZayavkaModel GetStatusSt(string unique_id, string year)
        {

            var result = from s in _studentDBContext.Students
                         join sz in _studentDBContext.StatusZayavka on s.status_zayavka_id equals sz.id
                         where s.unique_id == unique_id && s.year_birthday == Convert.ToDateTime(year) && s.pers_data == 1
                         select new StatusZayavkaModel { id = s.status_zayavka_id, description = sz.description };
            return result.FirstOrDefault();
        }

        public Students GetSingle(string id, string year)
        {
            return _studentDBContext.Students.Where(y => y.year_birthday == Convert.ToDateTime(year) && y.pers_data == 1).FirstOrDefault(x => x.unique_id == id);
        }

        public List<PhotoModel> GetStudPhoto(string unique_id)
        {
            var result = new List<PhotoModel>();
            try
            {
                SqlParameter paramUnId = new SqlParameter("@unique_id", unique_id);
                result = _studentDBContext.StudentPhoto.FromSqlRaw("[dbo].[getKidPhotoAPI] @unique_id", paramUnId).ToList();

            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
            return result;
        }

        public List<SchoolModel> GetSchoolsData()
        {
            var result = _studentDBContext.School.FromSqlRaw("[dbo].[Schools]").ToList();
            return result;
        }

        public List<SchoolModel> GetSchoolByUNP(string unp)
        {
            SqlParameter paramUNP = new SqlParameter("@UNP", unp);
            var result = _studentDBContext.School.FromSqlRaw("[dbo].[getSchoolByUNP] @UNP", paramUNP).ToList();
            return result;
        }

        public SchoolModel GetSchool(string id) //API
        {

            var result = from s in _studentDBContext.School
                         where s.id == Convert.ToInt32(id) 
                         select new SchoolModel { id = s.id, unn = s.unn, school_name = s.school_name, school_name_short = s.school_name_short, school_name_card = s.school_name_card, school_address = s.school_address, school_phone = s.school_phone, region = s.region, district = s.district, locality = s.locality, email = s.email, cbu = s.cbu };
            return result.FirstOrDefault();
        }

        //public List<SchoolModel> GetSchools()               //АПИ
        //{

        //    var result = from s in _studentDBContext.School
        //                 select new SchoolModel { id = s.id, unn = s.unn, school_name = s.school_name, school_name_short = s.school_name_short, school_name_card = s.school_name_card, school_address = s.school_address, school_phone = s.school_phone, region = s.region, district = s.district, locality = s.locality, email = s.email, cbu = s.cbu };
        //    return result.ToList();
        //}
        public List<SchoolModel> GetSchools()               //АПИ
        {
            var result = _studentDBContext.School.FromSqlRaw("[dbo].[API_Schools] ").ToList();
            return result;
        }
        public List<Risk> GetRisks()
        {
            var result = from s in _studentDBContext.Risks select new Risk { risk_lvl = s.risk_lvl, bin = s.bin, description = s.description, info = s.info };
            return result.ToList();
        }
        public StateModel GetSingleState(string state_id)
        {
            return _studentDBContext.State.Where(x => x.state_code == state_id).FirstOrDefault();
        }
        public void Add(Students student)
        {
            _studentDBContext.Students.Add(student);
        }
        public bool Save()
        {
            return (_studentDBContext.SaveChanges() >= 0);
        }
        public void Delete(string id, string year)
        {
            Students student = GetSingle(id, year);
            _studentDBContext.Students.Remove(student);
        }
        public Students Update(string id, Students item)
        {
            _studentDBContext.Students.Update(item);
            return item;
        }
        #endregion

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты)
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~DataContextRepository()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
