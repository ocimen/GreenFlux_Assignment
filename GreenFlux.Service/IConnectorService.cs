using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFlux.Api.Models;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Service
{
    public interface IConnectorService
    {
        Task<ICollection<ConnectorModel>> GetAllConnectorsByChargeStation(Guid groupId, Guid chargeStationId);
        Task<ConnectorModel> GetConnectorDetail(Guid groupId, Guid chargeStationId,int connectorId);
        Task<ChargeStationModel> AddConnector(Guid groupId, Guid chargeStationId, CreateConnector createConnector);
        void Remove(Guid groupId, Guid chargeStationId, int connectorId);
        void RemoveAllConnectorsByChargeStation(Guid groupId, Guid chargeStationId);
        Task<ConnectorModel> UpdateConnector(Guid groupId, Guid chargeStationId, int id, UpdateConnector updateConnector);
    }
}