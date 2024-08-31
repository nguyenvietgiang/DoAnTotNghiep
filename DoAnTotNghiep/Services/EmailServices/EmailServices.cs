using MimeKit;
using System.Net.Mail;
using System.Net;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Imaging;

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

            // Generate QR Code using ZXing
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 1
                }
            };
            var pixelData = writer.Write("http://localhost:5044/");
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using var ms = new MemoryStream();
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var qrCodeBase64 = Convert.ToBase64String(ms.ToArray());

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
                     .qr-code {{
                        text-align: center;
                        margin-top: 20px;
                    }}
                    .qr-code img {{
                        width: 100px;
                        height: 100px;
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
                        <div class=""qr-code"">
                            <img src=""data:image/png;base64,{qrCodeBase64}"" alt=""QR Code"">
                        </div>
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
