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
    }
}
