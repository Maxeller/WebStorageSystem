using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LocationTypeService _locationTypeService;
        public ApiController(IMapper mapper, LocationTypeService locationTypeService)
        {
            _mapper = mapper;
            _locationTypeService = locationTypeService;
        }

        [HttpGet("version", Name = "Version")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Version()
        {
            return "0.5"; //TODO: Move to DB or config
        }

        [HttpGet("location/type/", Name = "GetLocationTypes")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationTypeModel>>> GetLocationTypes([FromQuery] bool getDeleted)
        {
            var locationTypes = await _locationTypeService.GetLocationTypesAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            return models.ToList();
        }

        [HttpGet("location/type/{id?}", Name = "GetLocationType")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationTypeModel>> GetLocationType(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var locationType = await _locationTypeService.GetLocationTypeAsync((int) id, getDeleted);
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);
            if (locationTypeModel == null) return NotFound();
            return locationTypeModel;
        }
    }
}