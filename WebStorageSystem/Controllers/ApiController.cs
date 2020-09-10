using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Models;

namespace WebStorageSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("version", Name = "Version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Version()
        {
            return "0.1"; //TODO: Move to DB or config
        }
    }
}
