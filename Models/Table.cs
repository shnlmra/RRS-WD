using System.ComponentModel.DataAnnotations;

namespace RRS.Models
{
    public class Table
    {
        public int Id { get; set; }
        [Required]
        public int TableNumber { get; set; }
        public string? Description { get; set; }
        [Required]
        public int SeatingCapacity { get; set; }
        [Required, StringLength(100)]
        public string TableLocation { get; set; }
        public string? Status { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

}