namespace GamesLibrary.Repository.Contacts
{
    public interface IEmailSenderDto
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

}
