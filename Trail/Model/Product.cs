using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Trail.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string? ProductName { get; set; }

        public int? SupplierId { get; set; }

        public Double? UnitPrice { get; set; }

        public String? Package { get; set; }

        public int? IsDiscontinued { get; set; }
    }
}
