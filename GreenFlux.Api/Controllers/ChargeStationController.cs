using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Service;

namespace GreenFlux.Api.Controllers
{
    [Route(Routes.ChargeStationsPath)]
    [ApiController]
    public class ChargeStationController : ControllerBase
    {
        private readonly IChargeStationService _chargeStationService;

        public ChargeStationController(IChargeStationService chargeStationService)
        {
            _chargeStationService = chargeStationService;
        }

        /// <summary>
        /// Action to get all charge station by group
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllChargeStationsByGroup(Guid groupId)
        {
            var chargeStations = await _chargeStationService.GetAllChargeStationsByGroup(groupId);
            if (chargeStations?.Count > 0)
            {
                return Ok(chargeStations);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to get specific charge station by group and charge station id
        /// </summary>
        /// <returns></returns>
        [HttpGet(Routes.ChargeStationId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid groupId, Guid chargeStationId)
        {
            var chargeStations = await _chargeStationService.GetChargeStationDetail(groupId, chargeStationId);
            if (chargeStations != null)
            {
                return Ok(chargeStations);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to create new charge station for specific group
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid groupId, CreateChargeStation createChargeStation)
        {
            var chargeStation = await _chargeStationService.AddChargeStation(groupId, createChargeStation);
            if (chargeStation != null)
            {
                return Created(new Uri($"http://localhost:40356/api/groups/{groupId}/chargeStations/{chargeStation.Id}"), chargeStation);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to update specific charge station
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="chargeStationId"></param>
        /// <param name="updateChargeStation"></param>
        /// <returns></returns>
        [HttpPut(Routes.ChargeStationId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid groupId, Guid chargeStationId, [FromBody] UpdateChargeStation updateChargeStation)
        {
            var updatedChargeStation = await _chargeStationService.Update(groupId, chargeStationId, updateChargeStation);
            return Ok(updatedChargeStation);
        }

        /// <summary>
        /// Action to get delete specific charge station under the related group
        /// </summary>
        /// <returns></returns>
        [HttpDelete(Routes.ChargeStationId)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid groupId, Guid chargeStationId)
        {
            _chargeStationService.Remove(groupId, chargeStationId);
            return NoContent();
        }
    }
}
