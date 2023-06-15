using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web.UI.Models;
using System.Reflection;

namespace Web.UI.Controllers
{
    public class CountryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CountryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> List()
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:44311/api/Country/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<CountryDtoModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return View(result);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToAction("NotFound", "Home");
            }

            return RedirectToAction("TokenNull", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"https://localhost:44311/api/Country/Remove(OnlyAdmin)/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("MemberPage", "Home");
                }
            }

            return RedirectToAction("TokenNull", "Home");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryCreateDtoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44311/api/Country/Create(Authorize)", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Xeta yarandi");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Update(int id)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"https://localhost:44311/api/Country/GetbyId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<CountryUpdateDtoModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return View(result);
                }
            }

            return RedirectToAction("TokenNull", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CountryUpdateDtoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:44311/api/Country/Update(OnlyAdmin)/{model.Id}", content);

                if (response.IsSuccessStatusCode) { return RedirectToAction("List"); }

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) { return RedirectToAction("Forbidden", "Home"); }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) { return RedirectToAction("NotFound1", "Home"); }
            }

            return View("TokenNull", "Home");
        }

        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsondata = JsonSerializer.Serialize(file);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://localhost:44311/api/Country/Upload", content);

                if (response.IsSuccessStatusCode) { return RedirectToAction("List"); }

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) { return RedirectToAction("Forbidden", "Home"); }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) { return RedirectToAction("NotFound1", "Home"); }
            }

            return View("TokenNull", "Home");
        }
    }
}
