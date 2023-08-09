namespace GamesLibrary.Repository.Interfaces
{
    public class ResetPasswordModelDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
