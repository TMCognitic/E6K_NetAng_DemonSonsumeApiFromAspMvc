using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (HttpClient client = _httpClientFactory.CreateClient("Default")) // << Default est défini dans program.cs
            {
                HttpResponseMessage message = client.GetAsync("Contact").Result;
                message.EnsureSuccessStatusCode();

                string json = message.Content.ReadAsStringAsync().Result;
                _logger.LogInformation(json);

                Contact[]? contacts = JsonSerializer.Deserialize<Contact[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(contacts);
            }                
        }
    }
}