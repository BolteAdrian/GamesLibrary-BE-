namespace GamesLibrary.DataAccessLayer.Interfaces
{
    public class PaginationAndSearchOptions
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string[] SearchFields { get; set; } = new string[] { "Title", "Description", "Genre", "Developer", "Platform", "Price" };
        public SortOrder SortOrder { get; set; } 

        public string? SortField { get; set; } 
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

}
