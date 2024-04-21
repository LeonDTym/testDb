using Microsoft.AspNetCore.Mvc;
using SCRepository.Entity.Models;
using SCRepository.Entity.Repository.RepositoryData;
using System;
using System.Collections.Generic;
using System.Net;

namespace StudentCardsAdmin.Controllers
{
    public class BaseController : Controller
    {
        protected IDataContextRepository _dataContextRepository;

        public BaseController(IDataContextRepository dataContextRepository)
        {
            _dataContextRepository = dataContextRepository;
        }

        protected List<Students> GetStudents(int id)
        {
            var result = _dataContextRepository.Students_In(id);
            return result;
        }
        protected List<PhotoModel> GetStudentsPhoto(int school_id, string id)
        {
            var result = _dataContextRepository.PhotoModels(school_id, id);
            return result;
        }
        protected List<Students> GetStudentsEtc(int school_id, string unid)
        {
            var result = _dataContextRepository.StudentEtc(school_id, unid);
            return result;
        }
        protected List<Transfer> GetTransferdStud(string unique_id, string data)
        {
            var result = _dataContextRepository.GetTransfer( unique_id, data);
            return result;
        }
        protected string UpdateKidTransfer(string uniqueID, string schoolTransfer, string classN, string endData)
        {
            var result = _dataContextRepository.UpdateKidTransfer(uniqueID, schoolTransfer, classN, endData);
            return result;
        }
        protected string InsertStudent(Students students)
        {
            var result = _dataContextRepository.InsertStudent(students);
            return result;
        }

        protected string UpdateStudent(EditKid editKid)
        {
            var result = _dataContextRepository.UpdateStudent(editKid);
            return result;
        }

        protected string InsertSchool(SchoolModel schoolModel)
        {
            var result = _dataContextRepository.InsertSchool(schoolModel);
            return result;
        }

        protected string UpdateSchool(SchoolModel schoolModel)
        {
            var result = _dataContextRepository.UpdateSchool(schoolModel);
            return result;
        }
        protected string DeleteSchool(int id)
        {
            var result = _dataContextRepository.DeleteSchool(id);
            return result;
        }
        protected string InsertPhoto(PhotoModel photoModel)
        {
            var result = _dataContextRepository.InsertPhoto(photoModel);
            return result;
        }
        protected string SetUniqueId(int id)
        {
            var result = _dataContextRepository.SetUniqueId(id);
            return result;
        }
        protected List<SchoolModel> GetSchools()
        {
            var result = _dataContextRepository.GetSchoolsData();
            return result;
        }

        protected string SetKidAct(int sch_id, int st_id)
        {
            var result = _dataContextRepository.SetPersState(sch_id, st_id);
            return result;
        }
        protected List<Report> ReportGetData()
        {
            var result = _dataContextRepository.Report();
            return result;
        }
        protected List<Report> ReportChanges(string start, string end)
        {
            var result = _dataContextRepository.ReportDate(start, end);
            return result;
        }
        protected List<SchoolModel> GetSchoolData(int id)
        {
            var result = _dataContextRepository.GetSchoolData(id);
            return result;
        }
        protected List<Logs> GetLogs()
        {
            var result = _dataContextRepository.GetLogs();
            return result;
        }
        protected string SetLog(Logs logs)
        {
            var result = _dataContextRepository.SetLogs(logs);
            return result;
        }
        protected void DataForProtocol(string actionType, string actionResult, string actionDetails)
        {
            Logs insertLog = new Logs
            {
                actionDateTime = DateTime.Now,
                stationIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                stantionName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName,
                system = "SCAdmin",
                actionType = actionType,
                actionResult = actionResult,
                actionDetails = actionDetails
            };

            SetLog(insertLog);
        }
    }
}
