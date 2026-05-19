using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Tag;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class TagMapper
    {
        public static Tag ToModel(this TagRequestDto dto, Guid userId)
        {
            return new Tag
            {
                Name = dto.Name.ToLower().Trim(),
                UserId = userId
            };
        }
    }
}