using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SCRepository.Entity.Models
{
    public class TransferBlank
    {
        public string unique_id { get; set; }
        public DateTime year_birthday { get; set; }
        public string kid_surname { get; set; }
        public string kid_name { get; set; }
        [NotMapped]
        public string kid_patronymic { get; set; }
        public byte[] photo { get; set; }
    }
}
