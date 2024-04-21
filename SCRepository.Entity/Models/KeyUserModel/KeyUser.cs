using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
   public class KeyUser
    {
        public string name { get; set; }
        public int id { get; set; }
        public int ClientID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string IDKey { get; set; }
        public string KeySN { get; set; }
        public int? LoginNo { get; set; }
        public byte? bLogin { get; set; }
        public byte? CodeARM { get; set; }
        public byte? LevelAccess { get; set; }
        public int IDRoli { get; set; }
        public DateTime? DateChangePassword { get; set; }
        public byte? PeriodChangePassword { get; set; }
        public byte? CountChangePassword { get; set; }
        public DateTime? DateNewChangePassword { get; set; }
        public byte? ExitNoChangePassword { get; set; }
        public int? ErrorLoginNo { get; set; }
        public DateTime? DateErrorLoginNo { get; set; }
        public int? DayBlockErrorLogin { get; set; }
        public int? CountDayBlockErrorLogin { get; set; }
        public DateTime? DateBlockLogin { get; set; }
        public DateTime DateCreation { get; set; }
        public bool? bWork { get; set; }
        public string UNP { get; set; }
        public string Mfo { get; set; }
        public string CBY { get; set; }
        public string BankName { get; set; }
        public string address { get; set; }
        public byte? prUrPI { get; set; }
        public byte? TypeDocument { get; set; }
        public string SeriesAndNumderDoc { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? DateOfIssueOfTheDocument { get; set; }
        public string AuthorityIssuingTheDocument { get; set; }
        public int? TypeODB { get; set; }
        public string idcl { get; set; }
        public string Phones { get; set; }
        public DateTime? LastDateStopWork { get; set; }
        public DateTime? FirstDateStopWork { get; set; }
        public int? CustomerStopWorkId { get; set; }
        public string CustomerStopDescription { get; set; }
        public DateTime? DateLogin { get; set; }
        public DateTime? DateBlock { get; set; }
        public bool? FlagChangePassword { get; set; }
        public string BikIban { get; set; }
        public string BranchNameIban { get; set; }
        public string CodeCustomerSap { get; set; }
        public bool? PrNotResident { get; set; }
        public bool? PrQueryInformationListEnlistment { get; set; }
        public int? TimeSlipBlockUser { get; set; }
        public int? TimeStepSlipBlockUser { get; set; }
        public string NameCustomerLatin { get; set; }
        public string AddressLatin { get; set; }
        public string AddressEng { get; set; }
        public string NameCustomerEng { get; set; }
        public string PhonesSMS { get; set; }
        public bool? PrSmsSendingAllowed { get; set; }

        public byte? IsWorkClientBank { get; set; }
        public byte? IsWorkMobile { get; set; }
        public string MobileBankId { get; set; }
        public byte? IsUserWorkClientBank { get; set; }
        public byte? IsUserWorkMobile { get; set; }
        public byte? IsAccessIp { get; set; }
        public string ListAllowedIp { get; set; }
        public string Politic { get; set; }
        public List<int> politicint { get; set; }

        //"Mfo": "153001795", { get; set; }
        //"CBY": "", { get; set; }

    }
}
