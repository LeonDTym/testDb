using Microsoft.AspNetCore.Mvc;
using StudentCardsAdmin.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace StudentCardsAdmin.Controllers
{

    public class LoginController : Controller
    {
        public Key userKey = new Key();
        //readonly IAuthorizationRepository<UserClient> m_authorizationRepository;
        //readonly IAuthorizationKeyRepository<UserClient> m_authorizationKeyRepository;
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult IndexKey(Key key, byte[] data)
        //{
        //    ViewBag.Id = 3; //Вход по Эцп 
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var resultKey = m_authorizationKeyRepository.Authorization(data
        //                , userKey.KeySerialNumber
        //                , 0
        //                , HttpContext.Request.Headers?.FirstOrDefault(s => s.Key.ToLower() == "user-agent").Value);
        //            if (resultKey != null)
        //            {
        //                if (resultKey.ListCustomer.Count() > 0)
        //                {
        //                    listClients.Clear();
        //                    var sortedresult = resultKey.ListCustomer.OrderByDescending(u => u.CustomerId);
        //                    foreach (var item in sortedresult)
        //                    {
        //                        listClients.Add(new ListClients()
        //                        {
        //                            CustomerId = item.CustomerId,
        //                            IdCl = item.IdCl,
        //                            CustomerName = item.CustomerName,
        //                            Number = userKey.KeySerialNumber,
        //                            Data = data
        //                        });
        //                    }
        //                    if (listClients.Count() > 0)
        //                    {
        //                        ViewBag.Id = 7;
        //                        return PartialView("~/Views/Login/ListClientsBoxModal.cshtml", this);
        //                    }
        //                    else
        //                    {
        //                        return NotFound("Ошибка авторизации");
        //                    }
        //                }
        //                else
        //                {
        //                    _ = Authenticate(resultKey); // аутентификация
        //                    return Ok("OK");
        //                    //return RedirectToAction("Index", "Home");
        //                }
        //            }
        //            else
        //            {
        //                return NotFound("Ошибка авторизации");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //        //ModelState.AddModelError("", ex.Message);
        //    }
        //    return View("Index");
        //}

        public IActionResult RequestKey()
        {
            DateTime dateTime = DateTime.Now;
            var key = $"<?xml version =\"1.0\" encoding=\"utf-8\"?><Input Ver=\"1\" Type=\"get_sn_list\" Create=\"{dateTime:dd.MM.yyyy HH:mm:ss}\"></Input>";
            var code = EncodingBase64(key);
            var result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><Output Ver=\"1\" SID=\"0\" Offset=\"0\" Result=\"0\"><Log></Log><Data Encode=\"base64\">{code}</Data></Output>";
            return Content(result);
        }
        private string EncodingBase64(string data)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(plainTextBytes);
        }
        public IActionResult ResponseKey(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data)) return NotFound("Неверный формат данных");
                Output result = Deserialize(data);
                if (!string.IsNullOrWhiteSpace(result.Log))
                {
                    userKey.KeySerialNumber = "";
                    return NotFound(result.Log);
                }
                result = Deserialize(DecodingBase64(result.Data.Value));
                if (!string.IsNullOrWhiteSpace(result.Log))
                {
                    userKey.KeySerialNumber = "";
                    return NotFound(result.Log);
                }
                if ((result.Devices?.Count() ?? 0) > 0)
                {
                    var keyDetails = result.Devices.FirstOrDefault(x => x.Active == "true");
                    if (keyDetails != null)
                    {
                        userKey.KeySerialNumber = keyDetails.SN;
                        return Content(keyDetails.SN);
                    }
                }
                return NotFound("Нет данных о ключе");
            }
            catch
            {
                return NotFound("Неверный формат данных");
            }
        }

        private Output Deserialize(string data)
        {
            Output result;
            var serializer = new XmlSerializer(typeof(Output));
            using (TextReader reader = new StringReader(data))
            {
                result = (Output)serializer.Deserialize(reader);
            }
            return result;
        }
        private string DecodingBase64(string data)
        {
            var base64EncodedBytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public IActionResult RequestSign(string password, string keynumber)
        {
            try
            {
                if (keynumber != null)
                {
                    var keyserialnumber = Regex.Replace(keynumber, @"[а-яА-ЯёЁ.,]", "").Trim();
                    if (password != null && keyserialnumber.Any())
                    {
                        var document = "123";
                        DateTime dateTime = DateTime.Now;
                        var key = $"<?xml version =\"1.0\" encoding=\"utf-8\"?><Input Ver=\"1\" Type=\"sign_data\" Create=\"{dateTime:dd.MM.yyyy HH:mm:ss}\"><Password>{password}</Password><Data Encode=\"base64\">{EncodingBase64(document)}</Data></Input>";
                        var code = EncodingBase64(key);
                        var result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><Output Ver=\"1\" SID=\"0\" Offset=\"0\" Result=\"0\"><Log></Log><Data Encode=\"base64\">{code}</Data></Output>";
                        return Content(result);
                    }
                }
                //return PartialView("~/Views/Home/Index.cshtml", this);
                return Content(null);
            }
            //catch (AsbBankExceptions ex)
            //{
            //    return NotFound(ex.Message);   //"Ошибка при сохранении подписанных данных"
            //}
            catch (Exception)
            {
                return NotFound("Некорректный запрос");
            }
        }

        public IActionResult ResponseSign(string data, string keynumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data)) return NotFound("Неверный формат данных");
                Output result = Deserialize(data);
                if (!string.IsNullOrWhiteSpace(result.Log))
                {
                    RequestKey();
                    return NotFound("Вставьте ключ. Введите пароль на ключ");
                    //return NotFound(result.Log);
                }
                result = Deserialize(DecodingBase64(result.Data.Value));
                if (result.Result != 0)
                {
                    return NotFound(result.Log);
                }
                var signDocument = DecodingBase64Byte(result.Data.Value);
                {
                    userKey.KeySerialNumber = keynumber;
                    //return RedirectToAction("Index", "Home");
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
            }
            catch
            {
                return NotFound("Неверный формат данных");
            }
        }

        protected byte[] DecodingBase64Byte(string data)
        {
            var dec = Convert.FromBase64String(data);
            return Convert.FromBase64String(data);
        }
        //private async Task Authenticate(UserClient userName, User user = null)
        //{
        //    // создаем один claim
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, Newtonsoft.Json.JsonConvert.SerializeObject(userName)),
        //        new Claim("UserData", Newtonsoft.Json.JsonConvert.SerializeObject(user)),
        //    };
        //    // создаем объект ClaimsIdentity
        //    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //    // установка аутентификационных куки
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}





    }
}
