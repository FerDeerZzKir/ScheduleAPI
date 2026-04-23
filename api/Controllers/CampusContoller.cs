using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.User;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CampusController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<IActionResult> ProxyGet(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }


        [HttpGet("lessons/{groupId}")]
        public Task<IActionResult> GetLessons(int groupId) =>
            ProxyGet($"https://api.campus.kpi.ua/schedule/lessons?groupId={groupId}");

        [HttpGet("slots")]
        public Task<IActionResult> GetSlots() =>
            ProxyGet("https://api.campus.kpi.ua/schedule/lessons/slots");

        [HttpGet("time/current")]
        public Task<IActionResult> GetCurrentTime() =>
            ProxyGet("https://api.campus.kpi.ua/time/current");

        [HttpGet("lecturer-list")]
        public Task<IActionResult> GetLecturerList() =>
            ProxyGet("https://cdn.cloud.kpi.ua/lecturer-list-russian.json");

        [HttpGet("lecturer/{lecturerId}")]
        public Task<IActionResult> GetLecturer(string lecturerId) =>
            ProxyGet($"https://api.campus.kpi.ua/schedule/lecturer?lecturerId={lecturerId}");

        [HttpGet("groups")]
        public Task<IActionResult> GetGroups() =>
            ProxyGet("https://cdn.cloud.kpi.ua/schedule-groups-russian.json");

        [HttpGet("status/{groupId}")]
        public Task<IActionResult> GetStatus(int groupId) =>
            ProxyGet($"https://api.campus.kpi.ua/schedule/status?groupId={groupId}");
        
    }
}
