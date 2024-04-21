using SCRepository.Entity.Models;
using System;
using System.Collections.Generic;

namespace SCRepository.Entity.Repository.RepositoryData
{
    public interface IDataContextRepository : IDisposable
    {
        List<Logs> GetLogs();
        List<Students> Students(int id);
        List<Students> Students_In(int id);
        List<PhotoModel> PhotoModels(int school_id, string id);
        List<Students> StudentEtc(int unique_id, string year);
        //TransferBlank SetTransfer(int sch_id, int st_id);
        string SetTransfer(int sch_id, int st_id);
        string UpgradeClass(int sch_id, int st_id);
        (ExportUID, string) CheckImportUID(int st_id);
        TransferBlank GetTransferBlank(int st_id);
        List<Transfer> GetTransfer(string unique_id, string data);
        List<Report> Report();
        List<Report> ReportDate(string start, string end);
        string UpdateKidTransfer(string uniqueID, string schoolTransfer, string classN, string endData);
        string InsertStudent(Students students);
        string InsertMultiStudent(Students students);
        string UpdateStudent(EditKid editKid);
        string InsertSchool(SchoolModel schoolModel);
        string UpdateSchool(SchoolModel schoolModel);
        string DeleteSchool(int id);
        string SetAnketaState(int sch_id, int st_id);
        string SetPersState(int sch_id, int st_id);
        string RemovePersState(int sch_id, int st_id);
        string SetLogs(Logs logs);
        string SetLogs_Out(Logs_Out logs_out);
        string InsertPhoto(PhotoModel photoModel);
        string SetUniqueId(int id);
        string SetExclude(int st_id);
        string DeleteStudent(int st_id);
        //List<Students> GetStudentByUnId(int unique_id, string year);
        StudentsAPI GetStudentInf(string unique_id, string year);
        StatusZayavkaModel GetStatusSt(string unique_id, string year);
        List<SchoolModel> GetSchoolByUNP(string unp);
        List<SchoolModel> GetSchoolsData();//For WEB
        List<SchoolModel> GetSchoolData(int id);//For WEB
        SchoolModel GetSchool(string id); //For API
        List<SchoolModel> GetSchools(); //For API
        List<Risk> GetRisks();
        List<PhotoModel> GetStudPhoto(string unique_id);
        Students GetSingle(string id, string year);
        StateModel GetSingleState(string id);
        void Add(Students student);
        bool Save();
        void Delete(string id, string year);
        Students Update(string id, Students student);
    }
}
