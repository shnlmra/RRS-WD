using RRS.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RRS.Models.DataTransferObject
{
    public class CategoryDto
    {
        [Required, StringLength(100)]
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public ICollection<Menu> Menus { get; set; }
    }
}
