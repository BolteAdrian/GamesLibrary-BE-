﻿namespace GamesLibrary.Repository.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}