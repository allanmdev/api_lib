using System.ComponentModel.DataAnnotations;

namespace api_lib.Requests
{
    public class BookRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "Título não pode ser maior que 255 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(255, ErrorMessage = "Author não pode ser maior que 255 caracteres.")]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Year não pode ser negativo.")]
        public int Year { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa.")]
        public int Quantity { get; set; }



    }
}
