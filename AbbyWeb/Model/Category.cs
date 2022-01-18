using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AbbyWeb.Model;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre no puede ser nulo")]
    public string? Name { get; set; }
    
    [Display(Name = "Display Order")]
    [Range(1,100,ErrorMessage = "El numero esta fuera del rango (1-100)")]
    public int DisplayOrder { get; set; }
}