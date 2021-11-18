using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Manager
{
    public static class ServerUtils
    {
        public static async Task<string> UploadImage(IFormFile fileImage, string partialPath)
        {
            if (fileImage != null && Enum.GetNames(typeof(AllowedImageFormats)).Any(aif => fileImage.FileName.EndsWith(aif, StringComparison.OrdinalIgnoreCase)))
            {
                var randomFileName = $"{Guid.NewGuid()}.{fileImage.FileName.Split('.').Last()}";
                using var path = File.OpenWrite(Path.Combine(partialPath, randomFileName));
                await fileImage.CopyToAsync(path);

                return randomFileName;
            }

            return null;
        }

        public static bool DeleteFile(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetBookCoverImageSrc(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                imagePath = AppSettings.Instance.DefaultImage;
            else
                imagePath = $"{AppSettings.Instance.ImagePatch}/{imagePath}";
            return imagePath;
        }

        public static string GetUserAvatarImageSrc(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                imagePath = AppSettings.Instance.DefaultUserProfileImage;
            else
                imagePath = $"{AppSettings.Instance.DefaultUserProfileImagesPath}/{imagePath}";
            return imagePath;
        }

        public static bool SendEmail(string htmlString, string subject, string receiverEmail, EmailSettings emailSettings)
        {
            try
            {
                using (MailMessage message = new())
                {
                    using SmtpClient smtp = new();
                    message.From = new MailAddress(emailSettings.SenderEmail);
                    message.To.Add(new MailAddress(receiverEmail));
                    message.Subject = subject;
                    message.IsBodyHtml = emailSettings.IsBodyHtml;
                    message.Body = htmlString;
                    smtp.Port = emailSettings.MailPort;
                    smtp.Host = emailSettings.MailServer;
                    smtp.EnableSsl = emailSettings.EnableSsl;
                    smtp.UseDefaultCredentials = emailSettings.UseDefaultCredentials;
                    smtp.Credentials = new NetworkCredential(emailSettings.SenderEmail, emailSettings.Password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetForgotPasswordBody(string passwordResetLink, string userName)
        {
            return $@"<div style='font-family: Calibri, Arial, Helvetica, sans-serif; font-size:12pt; color:rgb(0,0,0);'>
                    <br>
                    <h3>Hello {userName}</h3>
                    <span> There is the requested <a href='{passwordResetLink}'>link</a> to change the password of your account on Book Sharing.</span >
                    <br><br>
                    <span>If you did not request the reset password link we advise you to change your password or contanct the administrator.</span>
                    <br><br>
                    <span>Best regards,</span>
                    <br>
                    <span>Book Sharing Team</span>
                    </div>";
        }
    }
}
