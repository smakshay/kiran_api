using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace CG_AirLineApi.Models
{
    public partial class Reservation
    {
        public int TicketNo { get; set; }
        
        public int? FlightId { get; set; }
        
        public int? Id { get; set; }
        [Required]
        public DateTime? DateofBooking { get; set; }
        [Required]
        public DateTime? JourneyDate { get; set; }
        [Required]
        public string PassengerName { get; set; }
        [Required]
        public long? ContactNo { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int? NoofTickets { get; set; }
        public decimal? TotalFare { get; set; }
        public string Ticketstatus { get; set; }

        [JsonIgnore]
        public virtual Flight Flight { get; set; }
        [JsonIgnore]
        public virtual User IdNavigation { get; set; }
    }
}
