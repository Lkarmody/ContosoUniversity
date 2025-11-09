using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages
{
    [ResponseCache(Duration = 7200, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public DateTime LastServerTime { get; set; }

        public void OnGet()
        {
            LastServerTime = DateTime.Now;
            _logger.LogInformation("Privacy page accessed at {Time}", LastServerTime);
        }
    }

}
