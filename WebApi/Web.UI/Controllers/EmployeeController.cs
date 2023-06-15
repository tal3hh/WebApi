using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(IHttpClientFactory httpClientFactory)
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

                var response = await client.GetAsync("https://localhost:44311/api/Employee/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<EmployeeDtoModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return View(result);
                }
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"https://localhost:44311/api/Employee/Remove(OnlyAdmin)/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("MemberPage", "Home");
                }
            }

            return RedirectToAction("List");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDtoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44311/api/Employee/Create(Authorize)", content);

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

                var response = await client.GetAsync($"https://localhost:44311/api/Employee/GetbyId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<EmployeeUpdateDtoModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return View(result);
                }
            }

            return RedirectToAction("TokenNull", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeUpdateDtoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:44311/api/Employee/Update(OnlyAdmin)/{model.Id}", content);

                if (response.IsSuccessStatusCode) { return RedirectToAction("List"); }

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) { return RedirectToAction("Forbidden", "Home"); }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) { return RedirectToAction("NotFound1", "Home"); }
            }

            return View("TokenNull", "Home");
        }
    }
}
