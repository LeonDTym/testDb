using System;
using System.ComponentModel.DataAnnotations;

namespace SCRepository.Entity.Models
{

    public class Logs_Out
    {
        [Key]
        public int id { get; set; }
        public DateTime actionDateTime { get; set; }
        public string UNP { get; set; }
        public string stationIp { get; set; }
        public string stantionName { get; set; }
        public string actionResult { get; set; }
        public string actionType { get; set; }
        public string actionDetails { get; set; }
        public string system { get; set; }
    }
}
