using MimeKit;
using System.Net.Mail;
using System.Net;

namespace DoAnTotNghiep.Services.EmailServices
{
    public class EmailServices : IEmailServices
    {
        public async Task SendEmailAsync(string email, string content)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("nguyenvietgiang1110@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Job Finder";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <html>
                    <head>
                        <title>Hòm Thư</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.5;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                            }}
                            .header {{
                                text-align: center;
                                margin-bottom: 20px;
                            }}
                            .logo {{
                                max-width: 200px;
                                max-height: 200px;
                                display: block;
                                margin: 0 auto;
                            }}
                            .content {{
                                margin-bottom: 20px;
                            }}
                            .footer {{
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                        <div class=""header"">
                         <img src=""https://themewagon.github.io/jobfinderportal/assets/img/logo/logo.png"" alt=""JobFinder"" class=""logo"">
                        </div>
                            <div class=""content"">
                                <p>Xin chào!</p>
                                <p>Chúng tôi là đội ngũ phát triển của Job Finder.</p>
                                <p>{content}</p>
                                <p>Chúc bạn một ngày mới tốt lành!</p>
                            </div>
                            <div class=""footer"">
                                <p>Best Regast,</p>
                                <p>Nguyễn Việt Giang</p>
                            </div>
                        </div>
                    </body>
                </html>";

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential("nguyenvietgiang1110@gmail.com", "kholeeizbmxzykbs");
                client.EnableSsl = true;

                await client.SendMailAsync(new MailMessage
                {
                    From = new MailAddress("nguyenvietgiang1110@gmail.com"),
                    Subject = "Thông báo",
                    Body = bodyBuilder.HtmlBody,
                    IsBodyHtml = true,
                    To = { email }
                });
            }
        }
    }
}
