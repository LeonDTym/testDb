using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRepository.Entity.Models
{
    public class EditKid
    {
        public int id_edit { get; set; }
        public string? unique_id_edit { get; set; }
        //public string nonfin_app_num { get; set; }
        public string kid_name_edit { get; set; }
        public string kid_surname_edit { get; set; }
        public string kid_patronymic_edit { get; set; }
        public DateTime year_birthday_edit { get; set; }
        public int school_id_edit { get; set; }
        public string kid_class_edit { get; set; }
        public DateTime end_training_edit { get; set; }
        public string? n_telephone_edit { get; set; }
        public string kid_email_edit { get; set; }
        public short pers_data_edit { get; set; }
        //public DateTime date_update { get; set; }
        //public int photo_id { get; set; }
        public short status_zayavka_id_edit { get; set; }
        public short stud_status_edit { get; set; }
        //public short? state_id_edit { get; set; }
        public short card_template_edit { get; set; }
    }
}
