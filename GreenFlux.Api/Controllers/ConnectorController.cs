using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Service;

namespace GreenFlux.Api.Controllers
{
    [Route(Routes.ConnectorsPath)]
    [ApiController]
    public class ConnectorController : ControllerBase
    {
        private readonly IConnectorService _connectorService;

        public ConnectorController(IConnectorService connectorService)
        {
            _connectorService = connectorService;
        }

        /// <summary>
        /// Action to get all connectors by charge station
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllConnectorsByChargeStation(Guid groupId, Guid chargeStationId)
        {
            var connectors = await _connectorService.GetAllConnectorsByChargeStation(groupId, chargeStationId);
            if (connectors?.Count > 0)
            {
                return Ok(connectors);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to get specific connector by group and charge station id
        /// </summary>
        /// <returns></returns>
        [HttpGet(Routes.ConnectorId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid groupId, Guid chargeStationId, int connectorId)
        {
            var connector = await _connectorService.GetConnectorDetail(groupId, chargeStationId, connectorId);
            if (connector != null)
            {
                return Ok(connector);
            }

            return NotFound();
        }

        /// <summary>
        /// Action to add new connector
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="chargeStationId"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid groupId, Guid chargeStationId, [FromBody] CreateConnector connector)
        {
            var createdConnector = await _connectorService.AddConnector(groupId, chargeStationId, connector);
            return Ok(createdConnector);
        }

        /// <summary>
        /// Action to update specific connector
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="chargeStationId"></param>
        /// <param name="connectorId"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
        [HttpPut(Routes.ConnectorId)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid groupId, Guid chargeStationId, int connectorId, [FromBody] UpdateConnector connector)
        {
            var updatedConnector = await _connectorService.UpdateConnector(groupId, chargeStationId, connectorId, connector);
            return Ok(updatedConnector);
        }

        /// <summary>
        /// Action to get delete specific connector under the related charge station
        /// </summary>
        /// <returns></returns>
        [HttpDelete(Routes.ConnectorId)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid groupId, Guid chargeStationId, int connectorId)
        {
            _connectorService.Remove(groupId, chargeStationId, connectorId);
            return NoContent();
        }
    }
}
