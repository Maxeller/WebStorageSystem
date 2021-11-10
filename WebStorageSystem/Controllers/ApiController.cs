using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebStorageSystem.Controllers
{
    [Route("api")]
    [ApiController]
    [FormatFilter]
    [Produces("application/json", "application/xml")]
    public class ApiController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ApiController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("version", Name = "Version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Version()
        {
            return Ok("0.5"); //TODO: Move to DB or config
        }

        [HttpGet("error", Name = "Error")]
        public ActionResult Error()
        {
            return Problem("Something went wrong!");
        }
    }
}