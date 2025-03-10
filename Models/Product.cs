using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
