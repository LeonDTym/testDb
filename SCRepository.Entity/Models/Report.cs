using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models
{
    public class Report
    {
        public string Name { get; set; }
        public int CountStudents { get; set; }
        public int CountDefective { get; set; }
        public int CountZayavleni { get; set; }
        public int CountTemplate { get; set; }
    }
}
