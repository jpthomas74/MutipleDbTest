using LearnNewAspNetWebAppWithAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearnNewAspNetWebAppWithAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index(string id)
        {

            ClaimsPrincipal principal = SetClaim("db1");

            if (null != principal)
            {
                List<ClaimData> claimData = new List<ClaimData>();
                foreach (var claim in principal.Claims)
                {
                    claimData.Add(new ClaimData { ClType = claim.Type, ClValue = claim.Value });
                }
                return View(claimData);
            }
        return View();
        }

        private ClaimsPrincipal SetClaim(string id)
        {
            if (id == null || id == "")
            {
                id = "db1";
            }
            ClaimsPrincipal principal = HttpContext.User;
            var identity = (ClaimsIdentity)principal.Identity;
            identity.AddClaim(new Claim("db", id));

            return principal;
        }

        public async Task<IActionResult> Privacy()
        {
            //var api = "https://localhost:44372/WeatherForecast/getcustomers";
            //var data = await JsonSerializer.DeserializeAsync<IEnumerable<Customer>>
            //            (await httpClient.GetStreamAsync(api), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //return View(data);
            var httpClient = _clientFactory.CreateClient("BrowneApi");
            
            var api = new HttpRequestMessage(HttpMethod.Get, "/weatherForecast/getcustomers");

            var response = await httpClient.SendAsync(api);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<IEnumerable<Customer>>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(data);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Privacy2()
        {
            SetClaim("db1");
            var httpClient = _clientFactory.CreateClient("TrialApi");
            var api = new HttpRequestMessage(HttpMethod.Get, "/api/departments/getall");

            var response = await httpClient.SendAsync(api);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<IEnumerable<Department>>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(data);
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> Privacy3()
        {
            SetClaim("db2");

            var httpClient = _clientFactory.CreateClient("NewApi");
            var api = new HttpRequestMessage(HttpMethod.Get, "/api/customer");

            var response = await httpClient.SendAsync(api);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<IEnumerable<Customer>>(responseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(data);
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      

    }
}
