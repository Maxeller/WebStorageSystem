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

namespace WebStorageSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        List<string> list = new List<string>(){"test1", "test2"};

        [HttpGet("list")]
        public ActionResult<IEnumerable<string>> List()
        {
            return list;
        }

        [HttpGet("list/{id}", Name = "GetById")]
        public ActionResult<string> List(int id)
        {
            var item = list.ElementAtOrDefault(id);
            return item != null ? (ActionResult<string>) item : NotFound();
        }


        [HttpPost("list")]
        public ActionResult AddToList([FromBody] string item)
        {
            if (item == null) return BadRequest();
            list.Add(item);
            return CreatedAtRoute("GetById",new {id = list.Count-1}, item);
        }
    }
}
