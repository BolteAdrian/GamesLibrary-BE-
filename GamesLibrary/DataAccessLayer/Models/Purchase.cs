using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.DataAccessLayer.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int GameId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
