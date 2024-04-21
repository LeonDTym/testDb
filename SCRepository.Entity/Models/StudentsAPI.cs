using System;

namespace SCRepository.Entity.Models
{

    public class StudentsAPI
    {
        public string kid_name { get; set; }
        public string kid_surname { get; set; }
        public string kid_patronymic { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_name_short { get; set; }
        public DateTime end_training { get; set; }
    }
}
