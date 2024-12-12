using System.ComponentModel.DataAnnotations;

namespace api_lib.Requests
{
    public class RentRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
