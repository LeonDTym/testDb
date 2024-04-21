using Microsoft.AspNetCore.Http;

namespace StudentCardsAdmin.Models
{
    public class PhotoModelView
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
    }
}
