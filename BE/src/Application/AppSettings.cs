using Domain.Enums;
using Domain.Enums.Notifications;
using Microsoft.Extensions.Configuration;

namespace Application
{
    public class AppSettings
    {
        private static AppSettings _instance;

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    IConfiguration config = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.Development.json", true, true)
                       .Build();
                    _instance = new AppSettings();
                    config.Bind(_instance);
                }

                return _instance;
            }
        }

        public string SiteLogo { get; set; }
        public string ImagePatch { get; set; }
        public string DefaultImage { get; set; }
        public string DefaultUserProfileImage { get; set; }
        public string DefaultUserProfileImagesPath { get; set; }
        public int AssignmentDays { get; set; }
        public int MaxAllowedAssignments { get; set; }
        public string DateDisplayFormat { get; set; }
        public int WishedBooksPageListSize { get; set; }
        public int BooksPageSize { get; set; }
        public int MaxAllowedBooksInUsersQueue { get; set; }
        public int MaxExtendDays { get; set; }
        public int ReviewsPerPage { get; set; }
        public int UsersPerPage { get; set; }
        public int BookRatingStars { get; set; }
        public int AdminReviewsPerPage { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public string PasswordResetEmailSubject { get; set; }
        public int PendingAuthorsPageSize { get; set; }
        public Notifications Notifications { get; set; }
    }
}
