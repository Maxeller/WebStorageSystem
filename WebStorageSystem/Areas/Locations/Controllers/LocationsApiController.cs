using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Filters;

namespace WebStorageSystem.Areas.Locations.Controllers
{
    [Route("api")]
    [ApiController]
    [FormatFilter, ValidateModelFilter]
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

        /// <summary>
        /// Gets list of Location Types
        /// </summary>
        /// <param name="getDeleted">Gets soft deleted entities</param>
        /// <returns>List of Location Types</returns>
        /// <response code="200">Returns list of Location Types</response>  
        [HttpGet("location/type/", Name = "GetLocationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationTypeModel>>> GetLocationTypes([FromQuery] bool getDeleted)
        {
            var locationTypes = await _locationTypeService.GetLocationTypesAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            return Ok(models.ToList());
        }

        /// <summary>
        /// Gets Location Type based on provided ID
        /// </summary>
        /// <param name="id">ID of Location Type</param>
        /// <param name="getDeleted">Gets soft deleted entities</param>
        /// <returns>Location Type</returns>
        /// <response code="200">Returns Location Type of provided ID</response> 
        /// <response code="400">If ID wasnt provided</response> 
        /// <response code="404">If selected ID doesnt exist</response> 
        [HttpGet("location/type/{id?}", Name = "GetLocationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationTypeModel>> GetLocationType(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var locationType = await _locationTypeService.GetLocationTypeAsync((int)id, getDeleted);
            if (locationType == null) return NotFound();
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);
            return Ok(locationTypeModel);
        }

        /// <summary>
        /// Creates Location Type
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST /location/type/
        ///     {
        ///         "Name": "&lt;name&gt;",
        ///         "Description": "&lt;description&gt;",
        ///         "IsDeleted": false
        ///     }
        ///     
        /// </remarks>
        /// <param name="locationTypeModel"></param>
        /// <returns>Newly created Location Type</returns>
        /// <response code="201">Returns newly created Location Type</response> 
        /// <response code="400">If model is wrong</response> 
        [HttpPost("location/type/", Name = "InsertLocationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> InsertLocationType([Bind("Name,Description,IsDeleted")] LocationTypeModel locationTypeModel)
        {
            var locationType = _mapper.Map<LocationType>(locationTypeModel);
            await _locationTypeService.AddLocationTypeAsync(locationType);
            var model = _mapper.Map<LocationTypeModel>(locationType);
            return CreatedAtAction(nameof(GetLocationType), new { id = model.Id }, model);
        }

        /// <summary>
        /// Edit Location Type 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     PUT /location/type/&lt;id&gt;
        ///     {
        ///         "Name": "&lt;name&gt;",
        ///         "Description": "&lt;description&gt;",
        ///         "IsDeleted": false,
        ///         "RowVersion": "&lt;version&gt;"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">ID of Location Type</param>
        /// <param name="locationTypeModel"></param>
        /// <returns></returns>
        /// <response code="200">Returns updated Location Type</response> 
        /// <response code="400">If model is wrong</response> 
        /// <response code="404">If selected ID doesnt exist</response> 
        [HttpPut("location/type/{id?}", Name = "UpdateLocationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateLocationType(int? id, [Bind()] LocationTypeModel locationTypeModel)
        {
            if (id == null) return BadRequest();
            var locationType = _mapper.Map<LocationType>(locationTypeModel);
            (bool success, string errMsg) = await _locationTypeService.EditLocationTypeAsync(locationType);
            if(!success) return BadRequest(errMsg);
            var model = _mapper.Map<LocationTypeModel>(locationType);
            return Ok(model);
        }

        /// <summary>
        /// Delete Location Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">If Location Type has been deleted</response> 
        /// <response code="400">If error occured</response> 
        /// <response code="404">If selected ID doesnt exist</response> 
        [HttpDelete("location/type/{id?}", Name = "DeleteLocationType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteLocationType(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errMsg) = await _locationTypeService.DeleteLocationTypeAsync((int)id);
            if(success) return Ok();
            return errMsg.Contains("not found") ? NotFound(errMsg) : (ActionResult)BadRequest(errMsg);
        }

        /// <summary>
        /// Gets list of Locations
        /// </summary>
        /// <param name="getDeleted">Gets soft deleted entities</param>
        /// <returns>List of Locations</returns>
        /// <response code="200">Returns list of Locations</response>  
        [HttpGet("location/", Name = "GetLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationModel>>> GetLocations([FromQuery] bool getDeleted)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationModel>>(locations);
            return Ok(models);
        }

        /// <summary>
        /// Gets Location based on provided ID
        /// </summary>
        /// <param name="id">ID of Location</param>
        /// <param name="getDeleted">Gets soft deleted entities</param>
        /// <returns>Location Type</returns>
        /// <response code="200">Returns Location based on provided ID</response> 
        /// <response code="400">If ID wasnt provided</response> 
        /// <response code="404">If selected ID doesnt exist</response> 
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