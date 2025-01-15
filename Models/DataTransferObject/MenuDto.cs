using Microsoft.EntityFrameworkCore;
using RRS.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RRS.Models.DataTransferObject
{
    public class MenuDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [Required, Precision(10, 2)]
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
