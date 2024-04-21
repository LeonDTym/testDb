using Microsoft.AspNetCore.Mvc;
using SCRepository.Entity.Models;
using SCRepository.Entity.Repository.RepositoryData;
using System.Buffers;
using System.Diagnostics;
using System.Net;
using WebApplication0.Models;

namespace WebApplication0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataContextRepository _dataContextRepository;
        public HomeController(ILogger<HomeController> logger,IDataContextRepository contextRepository)
        {
            _dataContextRepository = contextRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            DataForProtocol("Обнова", "Успех", "");
            return View();
        }
        public IActionResult GetData(int draw, int start, int length, string searchValue, string sortColumn, string sortDirection)
        {
            _logger.LogInformation("Обнова");
            var allLogs = _dataContextRepository.GetLogs();
            var filteredLogs = allLogs;
            // Фильтрация журналов на основе значения поиска
            if (searchValue != null) {
                filteredLogs = filteredLogs.Where(log => log.id.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Сортировка журналов на основе выбранной колонки и направления
            switch (sortColumn)
            {
                case "0":
                    filteredLogs = sortDirection == "asc"
                        ? filteredLogs.OrderBy(log => log.id).ToList()
                        : filteredLogs.OrderByDescending(log => log.id).ToList();
                    break;
                case "1":
                    filteredLogs = sortDirection == "asc"
                        ? filteredLogs.OrderBy(log => log.actionDetails).ToList()
                        : filteredLogs.OrderByDescending(log => log.actionDetails).ToList();
                    break;
                case "2":
                    filteredLogs = sortDirection == "asc"
                        ? filteredLogs.OrderBy(log => log.actionDateTime).ToList()
                        : filteredLogs.OrderByDescending(log => log.actionDateTime).ToList();
                    break;
                default:
                    break;
            }

            // Применение пагинации
            var paginatedLogs = filteredLogs.Skip(start).Take(length).ToList();
            //return Json(new { draw = draw, recordsFiltered = filteredLogs.Count, recordsTotal = allLogs.Count, data = paginatedLogs });
            return Json(new { data = allLogs });
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
