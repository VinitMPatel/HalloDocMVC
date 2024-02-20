namespace Services.Contracts
{
    public interface IEmailSender
    {
        Task SendEmail(string toEmail, string subject, string message);
    }
}