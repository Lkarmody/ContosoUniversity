using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace ContosoUniversity.Pages
{
    public class ClockPageModel : PageModel
    {
        private readonly IMemoryCache _memoryCache;

        public ClockPageModel(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DateTime CurrentDateTime { get; set; }
        public DateTime CacheCurrentDateTime { get; set; }
        public void OnGet()
        {
            CurrentDateTime = DateTime.Now;

            if (!_memoryCache.TryGetValue(CacheKeys.Entry, out DateTime cacheValue))
            {
                cacheValue = CurrentDateTime;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                _memoryCache.Set(CacheKeys.Entry, cacheValue, cacheEntryOptions);
            }

            CacheCurrentDateTime = cacheValue;
        }
    }
}
