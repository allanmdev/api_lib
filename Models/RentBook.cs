using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_lib.Models
{
    public class RentBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; } 

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [Required]
        public int UserId { get; set; } 

        [ForeignKey("UserId")]
        public User? User { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnDate { get; set; } 
    }
}
