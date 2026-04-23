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
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RailwayContext _context;
        public UserController(RailwayContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
            .Select(s => s.ToUserDto()).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToUserDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto UserDto)
        {
            var user = UserDto.ToUserFromCreateDto();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user.ToUserDto());
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var user = await  _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user.ToUserDto());
        }
        
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateUserRequestDto updateDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return NotFound();

             if (updateDto.Role == "student")
            {
                if (!updateDto.GroupId.HasValue)
                    return BadRequest("Student must have GroupId");

                var groupExists = await _context.Groups
                    .AnyAsync(g => g.Id == updateDto.GroupId.Value);

                if (!groupExists)
                    return BadRequest("Group does not exist");

                user.GroupId = updateDto.GroupId.Value;
                user.LecturerId = null;
            }
            else if (updateDto.Role == "lecturer")
            {
                user.GroupId = null;
                user.LecturerId = updateDto.LecturerId;
            }

            user.Username = updateDto.Username;
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.SurName = updateDto.SurName;
            user.GroupId = updateDto.GroupId;
            user.Role = updateDto.Role;
            user.LecturerId = updateDto.LecturerId;

            await _context.SaveChangesAsync();

            return Ok(user.ToUserDto());
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Patch(long id, [FromBody] PatchUserRequestDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found");

            if (dto.Username != null) user.Username = dto.Username;
            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.SurName != null) user.SurName = dto.SurName;
            
            if (dto.LastMessageId.HasValue)
                user.LastMessageId = dto.LastMessageId.Value;

            string targetRole = dto.Role ?? user.Role;

            if (targetRole == "lecturer")
            {
                user.Role = "lecturer";
                user.GroupId = null; 
                
                if (dto.LecturerId != null)
                {
                    user.LecturerId = dto.LecturerId;
                }
            }
            else if (targetRole == "student")
            {
                user.Role = "student";
                user.LecturerId = null; 

                if (dto.GroupId.HasValue)
                {

                    var groupExists = await _context.Groups
                        .AnyAsync(g => g.Id == dto.GroupId.Value);

                    if (!groupExists)
                        return BadRequest("Group does not exist");

                    user.GroupId = dto.GroupId.Value;
                }

                if (user.GroupId == null || user.GroupId == 0)
                {
                    return BadRequest("Student must have a valid GroupId");
                }
            }
            else
            {
                return BadRequest("Invalid role. Supported roles: 'student', 'lecturer'");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }

            return Ok(user.ToUserDto());
        }

        

    }
}