using AppSettingsManager;
using AppSettingsManager.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Map custom class file directly to the twilio section inside the appsettings 
// This profile is preferred when there are lot of values in a appsettings
// Ioptions
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));

// The inbuild configure method can handle multi level hierarchies
builder.Services.Configure<SocialLoginSettings>(builder.Configuration.GetSection("SocialLoginSettings"));

// Steps to Inject TwilioSettings class directly
// Generally done inside an extension method
// Generally dont write 3-4 lines of code in program.cs, try to do it one line
//var twilioSettings = new TwilioSettings();
//new ConfigureFromConfigurationOptions<TwilioSettings>(builder.Configuration.GetSection("Twilio")).Configure(twilioSettings);
//builder.Services.AddSingleton(twilioSettings);
// Replaced the above 4 lines by adding an extension method
builder.Services.AddConfiguration<TwilioSettings>(builder.Configuration, "Twilio");

// The below line will fail because AddConfiguration extension method can not handle multi level hierarchies
//builder.Services.AddConfiguration<SocialLoginSettings>(builder.Configuration, "Twilio");

// Change the default hierarchy to change precedence of secrets, appsettings, environment variables, command line variables etc
builder.Host.ConfigureAppConfiguration((hostingContext, builder) =>
{
    builder.Sources.Clear(); // This will clear the default precedence of items
    // Last one added will have a higher precedence
    builder.AddJsonFile("appsettings.json",optional:false, reloadOnChange:true);
    builder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",optional:true, reloadOnChange:true);
    if (hostingContext.HostingEnvironment.IsDevelopment())
    {
        builder.AddUserSecrets<Program>();
    }
    builder.AddEnvironmentVariables();
    builder.AddCommandLine(args);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
