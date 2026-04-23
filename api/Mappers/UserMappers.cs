using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SurName = user.SurName,
                GroupId = user.GroupId,
                Role = user.Role,
                LastMessageId = user.LastMessageId,
                CreatedAt = user.CreatedAt,
                LecturerId = user.LecturerId
            };
        }

        public static User ToUserFromCreateDto(this CreateUserRequestDto userDto)
        {
            return new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                SurName = userDto.SurName,
                GroupId = userDto.GroupId,
                Role = userDto.Role,
                LastMessageId = userDto.LastMessageId,
                LecturerId = userDto.LecturerId,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };
        }
    }
}