using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using api.Dtos.Group;
using api.Dtos.Lecturer;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/custom")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CustomController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private async Task<string?> GetJsonAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }


        private IActionResult FuzzySearch<T>(
            List<T> data,
            Func<T, string> selector,
            string query)
        {
            if (data == null || string.IsNullOrWhiteSpace(query))
                return NotFound();

            // exact
            var exact = data.FirstOrDefault(x =>
                selector(x).Equals(query, StringComparison.OrdinalIgnoreCase));

            if (exact != null)
                return Ok(new { type = "exact", result = exact });

            // fuzzy
            var similar = data
                .Where(x => selector(x)
                    .Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .ToList();

            return Ok(new { type = "similar", results = similar });
        }


        [HttpGet("lecturer/search")]
        public async Task<IActionResult> SearchLecturer(string fullName)
        {
            var json = await GetJsonAsync(
                "https://cdn.cloud.kpi.ua/lecturer-list-russian.json"
            );

            if (json == null)
                return StatusCode(500, "API error");

            var lecturers = JsonSerializer.Deserialize<List<LecturerDto>>(json);

            if (lecturers == null)
                return StatusCode(500, "Deserialize failed");

            return FuzzySearch(lecturers, x => x.name, fullName);
        }

        [HttpGet("group/search")]
        public async Task<IActionResult> SearchGroup(string name)
        {
            var json = await GetJsonAsync(
                "https://cdn.cloud.kpi.ua/schedule-groups-russian.json"
            );

            if (json == null)
                return NotFound();

            var groups = JsonSerializer.Deserialize<List<GroupDto>>(json);

            return FuzzySearch(groups!, x => x.GroupName, name);
        }
    }
}
