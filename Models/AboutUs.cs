using System.ComponentModel.DataAnnotations;

namespace E_CommerceMVC.Models
{
    public class AboutUs
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImageUrl { get; set; } = string.Empty;

    }
}
