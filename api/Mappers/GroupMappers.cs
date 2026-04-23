using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Group;
using api.Models;

namespace api.Mappers
{
    public static class GroupMappers
    {
        public static GroupDto ToGroupDto(this Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Faculty = group.Faculty
            };
        }
        public static Group ToAddGroupRequestDto(this Group group)
        {
            return new Group
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Faculty = group.Faculty
            };
        }
    }
}