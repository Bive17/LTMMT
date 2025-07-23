using System.ComponentModel.DataAnnotations;

namespace TechnicalSupportApi.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
