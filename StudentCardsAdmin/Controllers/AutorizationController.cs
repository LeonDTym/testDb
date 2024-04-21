using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCRepository.Entity.Models;
using SCRepository.Entity.Models.UserModels;
using SCRepository.Entity.Repository.RepositoryData;
using StudentCardsAdmin.Services;
using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Text;

namespace StudentCardsAdmin.Controllers
{
    public class AutorizationController : BaseController
    {
        private readonly AutorizationService autorizationService;

        public AutorizationController(IDataContextRepository dataContextRepository, AutorizationService autorizationService) : base(dataContextRepository)
        {
            this.autorizationService = autorizationService;
        }
        //public AutorizationController(AutorizationService autorizationService)
        //{
        //    this.autorizationService = autorizationService;
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckUser(LoginModel model)
        {
            //if (ModelState.IsValid)
            //{
            model.Password = CreateMD5("STCDSALT" + model.Password);


            UserInfoModel userInfo = autorizationService.CheckUser(model.UserName, model.Password);

            if (Int32.Parse(userInfo.LoginCode) > 0)
            {
                UserEtcData userEtcData = autorizationService.CheckUserEtc(model.UserName);
                HttpContext.Session.SetString("Authorized", "1");
                HttpContext.Session.SetString("Administration", "1");
                HttpContext.Session.SetString("Rights", userInfo.UserRights);
                HttpContext.Session.SetString("Username", userInfo.UserName);
                HttpContext.Session.SetString("Name", userInfo.Name);
                HttpContext.Session.SetString("CBU", userEtcData.cbu);
                HttpContext.Session.SetString("ObjName", userEtcData.objName);
                //if (userInfo.UserRights == "1")
                //{
                var data_schools = GetSchools();
                data_schools = data_schools.Where(s => s.cbu == userEtcData.cbu).ToList();
                string protocolResult = DataForProtocol(model.UserName, "Авторизация", "Успешно", "Пользователь вошел в систему");
                //HttpContext.Session.SetString("IdSchool", "33"); // Get 1st school 
                //}
                //if (userInfo.UserRights == "2")
                //{
                //    var data_schools = GetSchools();
                //    data_schools = data_schools.Where(s => s.cbu == userEtcData.cbu).ToList();
                //    HttpContext.Session.SetString("IdSchool", data_schools[0].id.ToString());
                //}
                //if (userInfo.UserRights == "8")
                //{
                //    var data_schools = GetSchools();
                //    HttpContext.Session.SetString("IdSchool", data_schools[0].id.ToString());
                //}
                var rightDescrip = "";
                if(userInfo.UserRights == "1") { rightDescrip = "Работник"; } 
                else { if (userInfo.UserRights == "2") { rightDescrip = "Бизнес-администратор (менеджер)"; }
                    else { if (userInfo.UserRights == "8") { rightDescrip = "Аналитик (Куратор)"; }
                        else{ rightDescrip = "Администратор ПК"; } } }
                HttpContext.Session.SetString("RightsDiscription", rightDescrip);
                return RedirectToAction("Index", "Home");

            }
            else if (Int32.Parse(userInfo.LoginCode) == 0)
            {
                HttpContext.Session.SetString("Username", userInfo.UserName);
                HttpContext.Session.SetString("ChangePassword", "1");
                return RedirectToAction("ChangePassword", "Autorization");
            }
            else
            {
                ModelState.AddModelError("", userInfo.LoginDesc);
                string protocolResult = DataForProtocol(model.UserName, "Авторизация", "Ошибка", "Поля заполнены некорректно");
                return View("Index", model);
            }
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Поля заполнены некорректно");
            //    bool protocolResult = DataForProtocol(model.UserName, "Авторизация", "Ошибка", "Поля заполнены некорректно");
            //    return View("Index", model);
            //}
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckUserProfile(LoginModel model)
        {
            try
            {
                PrincipalContext pc = new PrincipalContext(ContextType.Domain, "bb");
                // validate the credentials
                bool isValid = pc.ValidateCredentials(model.UserNameProfile, model.PasswordProfile);
                var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, "bb\\" + model.UserNameProfile);
                //isValid = true;
                if (isValid)
                {
                    HttpContext.Session.SetString("Authorized", "1");
                    HttpContext.Session.SetString("Administration", "0");
                    HttpContext.Session.SetString("Rights", "1");
                    HttpContext.Session.SetString("Username", model.UserNameProfile);
                    HttpContext.Session.SetString("RightsDesc", "Пользователь");
                    HttpContext.Session.SetString("IdSchool", "0");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string protocolResult = DataForProtocol(model.UserName, "Авторизация", "Ошибка", "Неверные данные для авторизации в домене");
                    ModelState.AddModelError("", "Неверные данные для авторизации в домене");
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                string protocolResult = DataForProtocol(model.UserName, "Авторизация", "Ошибка", $"{ex.Message}") ;
                ModelState.AddModelError("", ex.Message);
                return View("Index", model);
            }
        }

        public IActionResult ChangePassword()
        {
            var needToChange = HttpContext.Session.GetString("ChangePassword");
            if (needToChange != "1")
            {
                return RedirectToAction("Index", "Autorization");
            }
            ChangePasswordModel changePassword = new ChangePasswordModel
            {
                UserName = HttpContext.Session.GetString("Username"),
                Password = "",
                PasswordRepeat = ""
            };
            HttpContext.Session.SetString("ChangePassword", "0");
            return View(changePassword);
        }


        public IActionResult ChangePsw(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.PasswordRepeat)
                {
                    model.Password = CreateMD5("STCDSALT" + model.Password);
                    ChangeInfoModel changeInfo = autorizationService.ChangePassword(model.UserName, model.Password);
                    if (changeInfo.ResultCode == "1")
                    {
                        string protocolResult = DataForProtocol(model.UserName, "Смена пароля", "Успешно", "Пароль пользователя изменен");
                        return RedirectToAction("Index", "Autorization");
                    }
                    else
                    {
                        ModelState.AddModelError("", changeInfo.ResultDesc);
                        string protocolResult = DataForProtocol(model.UserName, "Смена пароля", "Ошибка", changeInfo.ResultDesc);
                        return View("ChangePassword", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Введённые пароли не совпадают");
                    string protocolResult = DataForProtocol(model.UserName, "Смена пароля", "Ошибка", "Введённые пароли не совпадают");
                    return View("ChangePassword", model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Неверно заполнены данные");
                string protocolResult = DataForProtocol(model.UserName, "Смена пароля", "Ошибка", "Неверно заполнены данные");
                return View("ChangePassword", model);
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        protected string DataForProtocol(string userId, string actionType, string actionResult, string actionDetails)
        {
            Logs insertLog = new Logs
            {
                actionDateTime = DateTime.Now,
                stationIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                stantionName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName,
                system = "SCAdmin",
                actionType = actionType,
                actionResult = actionResult,
                actionDetails = actionDetails+" | "+userId
            };

            string result = SetLog(insertLog);
            return result;
        }

        public IActionResult Logout()
        {
            string protocolResult = DataForProtocol(HttpContext.Session.GetString("Username"), "Выход из системы", "Успешно", "Пользователь вышел из системы");
            HttpContext.Session.SetString("Authorized", "0");
            HttpContext.Session.SetString("Administration", "0");
            HttpContext.Session.SetString("Rights", "");
            HttpContext.Session.SetString("CBU", "");
            HttpContext.Session.SetString("IdSchool", "0");
            HttpContext.Session.SetString("Username", "");
            HttpContext.Session.SetString("RightsDesc", "");
            return RedirectToAction("Index", "Autorization");
        }
        //public IActionResult Registration()
        //{
        //    return View();
        //}


    }
}