using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        // Entity Framework is used on Models and allow us to use DataAnnotations

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1,100, ErrorMessage = "Display Order must be between 1 and 100 only!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
           
    }
}
