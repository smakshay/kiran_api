using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace CG_AirLineApi.Models
{
    public partial class User
    {
        public User()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

       [JsonIgnore]
        public int? RoleId { get; set; }
        [JsonIgnore]

        public virtual Role Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
