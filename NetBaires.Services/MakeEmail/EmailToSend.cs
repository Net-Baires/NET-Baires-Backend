namespace NetBaires.Services.MakeEmail
{
    public class EmailToSend
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }

        public EmailToSend(string email, string body, string subject)
        {
            Email = email;
            Body = body;
            Subject = subject;
        }
    }
}