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

        /// <summary>
        /// Returns version of API
        /// </summary>
        /// <returns>Version of API</returns>
        [HttpGet("version", Name = "Version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Version()
        {
            return Ok("v1"); //TODO: Move to DB or config
        }

        /// <summary>
        /// Returns HTTP Status Code 500 for testing purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet("error", Name = "Error")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Error()
        {
            return Problem("Something went wrong!");
        }
    }
}