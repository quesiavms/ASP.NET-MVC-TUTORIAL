using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MVCTutorial.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MVCTutorial.Controllers
{
    public class EmailController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmailController> _logger;

        public EmailController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<EmailController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SentMailToUser(SendEmailModel model)
        {
            bool result = false;

            result = SendEmail(model.From, model.To, model.Subject, model.Body);

            return Json(result);
        }

        public bool SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", from));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using var smtpClient = new SmtpClient();
                smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtpClient.Authenticate("seu_email@gmail.com", "sua_senha");

                smtpClient.Send(message);
                smtpClient.Disconnect(true);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
