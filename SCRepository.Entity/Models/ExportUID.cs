using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SCRepository.Entity.Models
{
   public class ExportUID
    {
        [Key]
        public string nonfin_app_num { get; set; }
        public string FIO { get; set; }
        public string kid_class { get; set; }
    }
}
