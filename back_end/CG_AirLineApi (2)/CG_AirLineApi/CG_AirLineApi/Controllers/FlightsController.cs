using CG_AirLineApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CG_AirLineApi.Controllers
{ 
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightsController : ControllerBase
    {
        CG_AirlinesContext context;
        public FlightsController(CG_AirlinesContext context)
        {
            this.context = context;
        }
        // GET: api/<FlightController>
        [HttpGet]
        public IEnumerable<Flight> GetFlights()
        {
            var list = context.Flights.ToList();
            return list;
        }
        // GET api/<FlightController>/5
        [HttpGet("{FlightId}")]
        public Flight Get(int FlightId)
        {
            var flight = context.Flights.Find(FlightId);
            return flight;
        }

        // POST api/<FlightController>
        [HttpPost]
        public ActionResult Post([FromBody] Flight flight)
        {
            //validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //add
            context.Flights.Add(flight);
            try
            {
                int recordsAffected = context.SaveChanges();
                if (recordsAffected > 0)
                {
                    return Created("Flight", flight);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
                }

            }
            catch (SqlException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
            }
        }

        // PUT api/<FlightController>/5
        [HttpPut("{FlightId}")]
        public ActionResult Put(int FlightId, [FromBody] Flight flight)
        {
            //validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (FlightId != flight.FlightId)
            {
                return BadRequest("Ids do not match");
            }
            context.Flights.Update(flight);
            try
            {
                int recordsAffected = context.SaveChanges();
                if (recordsAffected > 0)
                {
                    return Ok(new
                    {
                        message = "Flight Updated Successfully"
                    });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
                }

            }
            catch (SqlException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
            }

        }

        // DELETE api/<FlightController>/5
        [HttpDelete("{FlightId}")]
        public ActionResult Delete(int FlightId)
        {
            var flight = context.Flights.Find(FlightId);
            if (flight == null)
            {
                return NotFound();
            }
            context.Flights.Remove(flight);
            try
            {
                int recordsAffected = context.SaveChanges();
                if (recordsAffected > 0)
                {
                    return Ok(new
                    {
                        Message = "Flight deleted Successfully"
                    });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
                }

            }
            catch (SqlException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
            }


        }

    }
}

    


      
