using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models
{
    public class Transfer
    {
        public string unique_id { get; set; }
        public string kid_name { get; set; }
        public string kid_surname { get; set; }
        public string kid_patronymic { get; set; }
        public DateTime year_birthday { get; set; }
        public string kid_class { get; set; }
        public DateTime end_training { get; set; }
        public string school_name_short { get; set; }
        public int sch_id { get; set; }
    }
}
