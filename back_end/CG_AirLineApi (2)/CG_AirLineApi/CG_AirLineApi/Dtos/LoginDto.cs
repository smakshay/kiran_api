using CG_AirLineApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG_AirLineApi.Dtos
{
    public class LoginDto
    {
        public string username { get; set; }
        public int id { get; set; }
        public string Token { get; set; }
        public string RoleName { get; set; }

    }
}
