using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace CG_AirLineApi.Models
{
    public partial class Flight
    {
        public Flight()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int FlightId { get; set; }
        [Required]
        public DateTime? LaunchDate { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string DeptTime { get; set; }
        [Required]
        public string ArrivalTime { get; set; }
        [Required]
        public int? NoOfSeats { get; set; }
        [Required]
        public decimal? Fare { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
