using GradProject_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GradProject_API.DTOs;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ContactUsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactUsDTO dto)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var password = _configuration["EmailSettings:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(dto.Name, senderEmail));
                message.To.Add(MailboxAddress.Parse(senderEmail)); // send to myself
                message.Subject = "Preloved Attire Feedback";

                message.Body = new TextPart("plain")
                {
                    Text = $"From: {dto.Name}\nEmail: {dto.Email}\n\nMessage:\n{dto.Message}"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(senderEmail, password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                smtp.Timeout = 10000; // 10 seconds


                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error sending email: {ex.Message}");
            }
        }
    }
}


    
