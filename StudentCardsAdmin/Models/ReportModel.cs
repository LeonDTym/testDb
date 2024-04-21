using SCRepository.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentCardsAdmin.Models
{
    public class ReportModel
    {
        public int id { get; set; }
        public string unn { get; set; }
        public string school_name_short { get; set; }
        public string count_students { get; set; }
        public List<Students> students { get; set; }
    }
}
