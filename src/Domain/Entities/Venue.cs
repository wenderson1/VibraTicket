using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Venue
    {
        public int Id { get; private set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public string? ZipCode { get; set; }
        public int Capacity { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Navigation Property
        public ICollection<Event> Events { get; private set; } = new List<Event>();
    }
}
