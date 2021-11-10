using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Areas.Locations.Controllers
{
    [Route("api")]
    [ApiController]
    [FormatFilter]
    [Produces("application/json", "application/xml")]
    public class LocationsApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LocationTypeService _locationTypeService;
        private readonly LocationService _locationService;

        public LocationsApiController(IMapper mapper, LocationTypeService locationTypeService, LocationService locationService)
        {
            _mapper = mapper;
            _locationTypeService = locationTypeService;
            _locationService = locationService;
        }
        
        [HttpGet("location/type/", Name = "GetLocationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationTypeModel>>> GetLocationTypes([FromQuery] bool getDeleted)
        {
            var locationTypes = await _locationTypeService.GetLocationTypesAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            return Ok(models.ToList());
        }
        
        [HttpGet("location/type/{id?}", Name = "GetLocationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationTypeModel>> GetLocationType(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var locationType = await _locationTypeService.GetLocationTypeAsync((int)id, getDeleted);
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);
            if (locationTypeModel == null) return NotFound();
            return Ok(locationTypeModel);
        }

        [HttpGet("location/", Name = "GetLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationModel>>> GetLocations([FromQuery] bool getDeleted)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationModel>>(locations);
            return Ok(models);
        }

        [HttpGet("location/{id?}", Name = "GetLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationModel>> GetLocation(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var location = await _locationService.GetLocationAsync((int)id, getDeleted);
            var locationModel = _mapper.Map<LocationModel>(location);
            if (locationModel == null) return NotFound();
            return Ok(locationModel);
        }
    }
}