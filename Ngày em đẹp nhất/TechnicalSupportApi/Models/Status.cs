using System.ComponentModel.DataAnnotations;

namespace TechnicalSupportApi.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
