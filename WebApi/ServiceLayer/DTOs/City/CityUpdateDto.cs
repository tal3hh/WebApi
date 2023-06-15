using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTOs.City
{
    public class CityUpdateDto
    {
        public string? Name { get; set; }
        public int Population { get; set; }

        public int CountryId { get; set; }
    }
}
