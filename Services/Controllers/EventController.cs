using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Services.Context;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using common.Models;

namespace Services.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class EventController : Controller
    {
        private EventContext _eventContext;
        private RegisterContext _registerContext;
        private IConfiguration _configuration;

        public EventController(EventContext eventContext, RegisterContext registerContext, IConfiguration configuration)
        {
            _eventContext = eventContext;
            _registerContext = registerContext;
            _configuration = configuration;
        }


        [HttpGet]
        // GET: EventController
        public List<Event> Get()
        {
            return _eventContext.Events.ToList();
        }

        [HttpGet("{id}")]

        public IActionResult GetByUserId(int id)
        {
            var mycon = _configuration.GetConnectionString("EventConnStr");
            SqlConnection con = new SqlConnection(mycon.ToString());
            con.Open();
            string qry = "select * from dbo.Event where UserId='" + id + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            List<EventDTO> eventsdto = new List<EventDTO>();
            while (sdr.Read())
            {
                EventDTO dto = new EventDTO();
                dto.Title = Convert.ToString(sdr[2]);
                dto.Date = Convert.ToDateTime(sdr[3]);
                dto.StartTime = Convert.ToDateTime(sdr[5]);
                dto.Location = Convert.ToString(sdr[4]);
                dto.Details = Convert.ToString(sdr[9]);
                dto.Description = Convert.ToString(sdr[8]);
                dto.Invite = Convert.ToString(sdr[10]);
                dto.Type = Convert.ToString(sdr[6]);
                dto.Duration = Convert.ToString(sdr[7]);
                dto.UserId = Convert.ToInt32(sdr[1]);
                dto.EventId = Convert.ToInt32(sdr[0]);
                eventsdto.Add(dto);
            }              
             return Ok(eventsdto);
        }

        [HttpPost]
        public IActionResult UserData([FromBody]Register UserData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }
            _registerContext.Users.Add(UserData);
            _registerContext.SaveChanges();

            return Ok();
        }

        [HttpPost("/VerifyLogin")]

        public IActionResult VerifyLogin([FromBody]Register LoginData)
        {
            var mycon = _configuration.GetConnectionString("EventConnStr");
            SqlConnection con = new SqlConnection(mycon.ToString());
            string email = LoginData.Email;
            string pass = LoginData.Password;
            con.Open();
            string qry = "select * from dbo.Users where Email='" + email + "' and Password='" + pass + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                return Ok(sdr[0]);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult EventDetails(int id)
        {
            var detail = _eventContext.Events.FindAsync(id);

            if (detail == null)
            {
                return NotFound();
            }
            else
            {
                var result = detail.Result;
                EventDTO eventsdto = new EventDTO()
                { Title = result.Title,
                    Date = result.Date,
                    StartTime = result.StartTime,
                    Location = result.Location,
                    Details = result.Details,
                    Description = result.Description,
                    Invite = result.Invite,
                    Type = result.Type,
                    Duration = result.Duration,
                    UserId = result.UserId,
                    EventId = result.EventId
                };
                return Ok(eventsdto);
            }
        }    
        
        [HttpPost]
        public List<Event> RegisterEvent([FromBody]Event EventDetails)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            _eventContext.Events.Add(EventDetails);
            _eventContext.SaveChanges();

            return _eventContext.Events.ToList();
        }
        [HttpPost]
        public List<Event> UpdateEvent([FromBody]Event EventDetails)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            _eventContext.Events.Update(EventDetails);
            _eventContext.SaveChanges();

            return _eventContext.Events.ToList();
        }


    }
}
