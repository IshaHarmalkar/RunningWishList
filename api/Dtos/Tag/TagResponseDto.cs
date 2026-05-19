using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Tag
{
    public class TagResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set;}
    }
}