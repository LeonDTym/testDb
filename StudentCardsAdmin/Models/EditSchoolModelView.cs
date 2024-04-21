using System.ComponentModel.DataAnnotations;

namespace StudentCardsAdmin.Models
{

    public class EditSchoolModelView
    {
 
        public int id_edit { get; set; }
   
        public string unn_edit { get; set; }
    
        public string school_name_edit { get; set; }
        
        public string school_name_short_edit { get; set; }
    
        public string school_name_card_edit { get; set; }
  
        public string school_address_edit { get; set; }
 
        public string school_phone_edit { get; set; }

        public string region_edit { get; set; }

        public string district_edit { get; set; }

        public string email_edit { get; set; }

        public string locality_edit { get; set; }

        public string cbu_edit { get; set; }
    }
}
