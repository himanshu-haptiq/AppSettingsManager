using AppSettingsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AppSettingsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        // When appsettings is not mapped in the program.cs
        private TwilioSettings _twilioSettings;
        // When appsettings is mapped in the program.cs
        private readonly IOptions<TwilioSettings> _twilioOptions;
        private readonly IOptions<SocialLoginSettings> _socialLoginOptions;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IOptions<TwilioSettings> twilioOptions, TwilioSettings twilioSettings, IOptions<SocialLoginSettings> socialLoginOptions)
        {
            _logger = logger;
            _config = config;

            // Generally it is preferred to move the below binding logic to the startup class
            //_twilioSettings = new TwilioSettings();
            //// Bind values from the section to the custom class which have exact same properties
            //_config.GetSection("Twilio").Bind(_twilioSettings);

            _twilioOptions = twilioOptions;

            _twilioSettings = twilioSettings;
            _socialLoginOptions = socialLoginOptions;
        }

        public IActionResult Index()
        {
            // IConfiguration contains all the configurations from appsettings file
            ViewBag.SendGridKey = _config.GetValue<string>("SendGridKey");

            // Get appsettings details from a section named twilio
            // Preferred way to get data from sections
            //ViewBag.TwilioAuthToken = _config.GetValue<string>("Twilio:AuthToken");
            //ViewBag.TwilioAccountSid = _config.GetValue<string>("Twilio:AccountSid");
            //// When there are multiple values inside a section
            //ViewBag.TwilioPhoneNumber = _twilioSettings.Phone1Number;

            // Less preferred way for getting keys from appsettings sections
            // If number of nested sections increase, then it is a pain
            //ViewBag.TwilioAuthToken = _config.GetSection("Twilio").GetValue<string>("AuthToken");
            //ViewBag.TwilioAccountSid = _config.GetSection("Twilio").GetValue<string>("AccountSid");

            //ViewBag.ThirdLevelSettingValue = _config.GetValue<string>("FirstLevelSetting:SecondLevelSetting:BottomLevelSetting");
            //ViewBag.ThirdLevelSettingValue = _config.GetSection("FirstLevelSetting").GetSection("SecondLevelSetting").GetValue<string>("BottomLevelSetting");
            ViewBag.ThirdLevelSettingValue = _config.GetSection("FirstLevelSetting").GetSection("SecondLevelSetting").GetSection("BottomLevelSetting").Value;

            // When the custom appsettings class is mapped in the program.cs
            // IOptions
            //ViewBag.TwilioAuthToken = _twilioOptions.Value.AuthToken;
            //ViewBag.TwilioAccountSid = _twilioOptions.Value.AccountSid;
            //ViewBag.TwilioPhoneNumber = _twilioOptions.Value.PhoneNumber;

            ViewBag.TwilioAuthToken = _twilioSettings.AuthToken;
            ViewBag.TwilioAccountSid = _twilioSettings.AccountSid;
            ViewBag.TwilioPhoneNumber = _twilioSettings.PhoneNumber;

            // Get the connection string
            ViewBag.ConnectionString = _config.GetConnectionString("AppSettingsManagerDb");

            ViewBag.FacebookKey = _socialLoginOptions.Value.FacebookSettings.Key;
            ViewBag.GoogleKey = _socialLoginOptions.Value.GoogleSettings.Key;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
