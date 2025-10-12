using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public string? DefaultLogLevel { get; set; }
        public string? EnvironmentMessage { get; set; }

        public SettingsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            DefaultLogLevel = _configuration["Logging:LogLevel:Default"];
            EnvironmentMessage = _configuration["CustomSettings:EnvironmentMessage"];
        }
    }
}
