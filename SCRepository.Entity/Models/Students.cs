using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCRepository.Entity.Models
{
    [Table("Students")]
    public class Students
    {
        [Key]
        public int id { get; set; }
        public string? unique_id { get; set; }
        public string nonfin_app_num { get; set; }
        public string kid_name { get; set; }
        public string kid_surname { get; set; }
        public string? kid_patronymic { get; set; }
        public DateTime year_birthday { get; set; }
        public int school_id { get; set; }
        public string kid_class { get; set; }
        public DateTime end_training { get; set; }
        public string? n_telephone { get; set; }
        public string kid_email { get; set; }
        public short pers_data { get; set; }
        public short anketa { get; set; }
        public DateTime date_update { get; set; }
        public int photo_id { get; set; }
        public short status_zayavka_id { get; set; }
        public short stud_status { get; set; }
        public short? state_id { get; set; }
        public short? card_template { get; set; }
    }
}
