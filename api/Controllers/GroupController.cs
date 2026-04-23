using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Group;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
         private readonly RailwayContext _context;
         private readonly HttpClient _httpClient;
         public GroupController(RailwayContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _context.Groups
            .Select(g => g.ToGroupDto()).ToListAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById([FromRoute] int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group.ToGroupDto());
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] AddGroupRequestDto GroupDto)
        {
            var group = new Group
            {
                Id = GroupDto.Id,
                GroupName = GroupDto.GroupName,
                Faculty = GroupDto.Faculty
            };
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return Ok(group.ToGroupDto());
        }
        
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGroupRequestDto updateDto)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            group.GroupName = updateDto.GroupName;
            group.Faculty = updateDto.Faculty;
            await _context.SaveChangesAsync();
            return Ok(group.ToGroupDto());
        }
    }
}