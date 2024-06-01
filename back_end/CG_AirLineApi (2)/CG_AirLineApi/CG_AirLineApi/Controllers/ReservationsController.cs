using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CG_AirLineApi.Models;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace CG_AirLineApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
     [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly CG_AirlinesContext _context;

        public ReservationsController(CG_AirlinesContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        /* [HttpGet("FlightList")]
         public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
         {
            return  await _context.Flights.ToListAsync();

         }*/

        // GET: api/Reservations/5
        /*[HttpGet("TicketStatus /{TicketNo}")]
        public async Task<ActionResult<Reservation>> GetReservation(int TicketNo)
        {
            var reservation = await _context.Reservations.FindAsync(TicketNo);

            if (reservation == null)
            {
                return NotFound();
            }
            try
            {

                if (reservation.JourneyDate < DateTime.Now)
                {
                    reservation.Ticketstatus = "Ticket expired";
                    _context.Reservations.Update(reservation);
                    _context.SaveChanges();

                    return Ok(reservation);
                }
                else return Ok(reservation);

            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/




        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*  [HttpPut("{id}")]
          public async Task<IActionResult> PutReservation(int id, Reservation reservation)
          {
              if (id != reservation.TicketNo)
              {
                  return BadRequest();
              }

              _context.Entry(reservation).State = EntityState.Modified;

              try
              {
                  await _context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException)
              {
                  if (!ReservationExists(id))
                  {
                      return NotFound();
                  }
                  else
                  {
                      throw;
                  }
              }

              return NoContent();
          } */

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            //validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             var id = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            try
            {
                if (reservation.DateofBooking < reservation.JourneyDate)
                {

                    Flight FlightObject = await _context.Flights.FindAsync(reservation.FlightId);
                    //calculation of total fare amount 
                    reservation.TotalFare = (decimal)FlightObject.Fare * (int)reservation.NoofTickets;
                    reservation.Ticketstatus = "booked";
                    reservation.Id =Convert.ToInt32(id);
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                    return Ok(reservation);
                }
                return Ok($"journeydate{reservation.JourneyDate}should be greater than date of booking{reservation.DateofBooking}");
                

            }

            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Conflicted with foreign key"))
                {
                    return BadRequest("Invalid");
                }
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, "server error");
        }
        // DELETE: api/Reservations/5
        [HttpDelete("Cancellation/{TicketNo}")]
        public async Task<IActionResult> DeleteReservation(int TicketNo)
        {
            var reservation = await _context.Reservations.FindAsync(TicketNo);
            if (reservation == null)
            {
                return NotFound();
            }
            try
            {


                if (reservation.JourneyDate > DateTime.Today && reservation.DateofBooking < reservation.JourneyDate)
                {
                    if (reservation.Ticketstatus == "ticket cancelled and 40% amount refunded")
                    {
                        return Ok(reservation);
                    }

                    reservation.Ticketstatus = "ticket cancelled and 40% amount refunded";
                    //calculaton 40% of total fare if user cancells ticket before journey date
                    reservation.TotalFare = (decimal)(float)0.4 * (int)reservation.TotalFare;
                    _context.Reservations.Update(reservation);
                    _context.SaveChanges();
                    return Ok(reservation);
                }
                else
                {
                    if (reservation.Ticketstatus == "ticket cancelled and no amount refunded journey date passed")
                    {
                        return Ok(reservation);
                    }
                    else
                    {

                        reservation.Ticketstatus = "ticket cancelled and no amount refunded journey date passed";
                    }
                    _context.Reservations.Update(reservation);
                    _context.SaveChanges();
                    return Ok(reservation);
                }


            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        private bool ReservationExists(int TicketNo)
        {
            return _context.Reservations.Any(e => e.TicketNo == TicketNo);
        }
    }
}
