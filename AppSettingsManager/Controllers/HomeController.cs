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

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IOptions<TwilioSettings> twilioOptions)
        {
            _logger = logger;
            _config = config;
            _twilioSettings = new TwilioSettings();
            // Bind values from the section to the custom class which have exact same properties
            _config.GetSection("Twilio").Bind(_twilioSettings);

            _twilioOptions = twilioOptions;
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
            ViewBag.TwilioAuthToken = _twilioOptions.Value.AuthToken;
            ViewBag.TwilioAccountSid = _twilioOptions.Value.AccountSid;
            ViewBag.TwilioPhoneNumber = _twilioOptions.Value.PhoneNumber;

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
