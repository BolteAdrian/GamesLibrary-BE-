using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Repository.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Developer { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
