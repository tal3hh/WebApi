using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class CityController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CityController(IHttpClientFactory httpClientFactory)
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

                var response = await client.GetAsync("https://localhost:44311/api/City/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    var result = JsonSerializer.Deserialize<List<CityDtoModel>>(jsonData, new JsonSerializerOptions
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

                var response = await client.DeleteAsync($"https://localhost:44311/api/City/Remove(OnlyAdmin)/{id}");
                if (response.IsSuccessStatusCode) return RedirectToAction("List");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToAction("NotFound", "Home");
            }

            return RedirectToAction("TokenNull", "Home");
        }

        public async Task<IActionResult> Create()
        {
            var model = new CityCreateDtoModel();

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:44311/api/Country/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<CountryDtoModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    model.Countries = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(data, "Id", "Name");

                    return View(model);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToAction("NotFound", "Home");

            }
            return RedirectToAction("TokenNull", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityCreateDtoModel model)
        {
            var data = TempData["Countries"]?.ToString();
            if (data != null)
            {
                var countries = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                model.Countries = new SelectList(countries, "Value", "Text");
            }

            if (!ModelState.IsValid) return View(model);


            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44311/api/City/Create(Authorize)", content);

                if (response.IsSuccessStatusCode) return RedirectToAction("List");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Unauthorized", "Home");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToAction("NotFound", "Home");
            }

            return RedirectToAction("TokenNull", "Home");
        }

        public async Task<IActionResult> Update(int id)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var repsonseCity = await client.GetAsync($"https://localhost:44311/api/City/GetbyId/{id}");

                if (repsonseCity.IsSuccessStatusCode)
                {
                    var jsonCity = await repsonseCity.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<CityUpdateDtoModel>(jsonCity, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    var responseCountries = await client.GetAsync("https://localhost:44311/api/Country/GetAll");

                    if (responseCountries.IsSuccessStatusCode)
                    {
                        var jsonCountries = await responseCountries.Content.ReadAsStringAsync();
                        var resultCountires = JsonSerializer.Deserialize<List<CountryDtoModel>>(jsonCountries, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        if (result != null)
                            result.Countries = new SelectList(resultCountires, "Id", "Name");
                    }

                    return View(result);
                }

            }
            return RedirectToAction("TokenNull", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CityUpdateDtoModel model)
        {
            var data = TempData["Countries"]?.ToString();
            if (data != null)
            {
                var countries = JsonSerializer.Deserialize<List<SelectListItem>>(data);
                model.Countries = new SelectList(countries, "Value", "Text", model.CountryId);
            }

            if (!ModelState.IsValid) return View(model);


            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:44311/api/City/Update(OnlyAdmin)/{model.Id}", content);

                if (response.IsSuccessStatusCode) return RedirectToAction("List");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Unauthorized", "Home");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return RedirectToAction("NotFound", "Home");
            }

            return RedirectToAction("TokenNull", "Home");
        }
    }
}
