using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GreenFlux.Api.Models;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Data;
using GreenFlux.Domain;
using GreenFlux.Persistence.Exceptions;
using Microsoft.Extensions.Logging;

namespace GreenFlux.Service
{
    public class ConnectorService : IConnectorService
    {
        private readonly IGenericRepository<Group> _groupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ConnectorService(IGenericRepository<Group> groupRepository, IMapper mapper, ILogger<ConnectorService> logger)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ConnectorModel>> GetAllConnectorsByChargeStation(Guid groupId, Guid chargeStationId)
        {
            var chargeStation = await GetChargeStation(groupId ,chargeStationId);
            return _mapper.Map<ICollection<ConnectorModel>>(chargeStation.Connectors);
        }

        public async Task<ConnectorModel> GetConnectorDetail(Guid groupId, Guid chargeStationId,int connectorId)
        {
            var connector = await GetConnector(groupId, chargeStationId, connectorId);
            return _mapper.Map<ConnectorModel>(connector);
        }

        public async Task<ChargeStationModel> AddConnector(Guid groupId, Guid chargeStationId, CreateConnector createConnector)
        {
            var group = await GetGroup(groupId);
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            var connectorList = new List<int> { createConnector.MaxCurrent};
            
            chargeStation.AddConnectors(group, connectorList);
            await _groupRepository.SaveChangesAsync();
            return _mapper.Map<ChargeStationModel>(chargeStation);
        }

        public async Task Remove(Guid groupId, Guid chargeStationId, int connectorId)
        {
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            chargeStation.RemoveConnector(connectorId);
            await _groupRepository.SaveChangesAsync();
            _logger.LogInformation($"Connector with {connectorId} was removed from the charge station");
        }

        public async Task RemoveAllConnectorsByChargeStation(Guid groupId, Guid chargeStationId)
        {
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            chargeStation.Connectors = new List<Connector>();
            
            _logger.LogInformation($"All Connector under the charge station of {chargeStation.Id} was removed.");
            await _groupRepository.SaveChangesAsync();
        }

        public async Task<ConnectorModel> UpdateConnector(Guid groupId, Guid chargeStationId, int id, UpdateConnector updateConnector)
        {
            var connector = await GetConnector(groupId, chargeStationId, id);
            var group = await GetGroup(groupId);

            
            var updatedGroupCapacity = group.CurrentCapacity - connector.MaxCurrent + updateConnector.MaxCurrent;

            if (group.Capacity >= updatedGroupCapacity)
            {
                connector.UpdateMaxCurrent(updateConnector.MaxCurrent);
                await _groupRepository.SaveChangesAsync();
                return _mapper.Map<ConnectorModel>(connector);
            }

            var diff = updatedGroupCapacity - group.Capacity;
            throw new GroupCapacityExceedsException(groupId, new List<int> { updateConnector.MaxCurrent}, diff);
        }

        private async Task<Connector> GetConnector(Guid groupId, Guid chargeStationId, int id)
        {
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            var connector = chargeStation.Connectors.FirstOrDefault(x => x.Id == id);
            if (connector != null)
            {
                return connector;
            }

            throw new EntityNotFoundException($"The connector with {id} not found under {chargeStation.Id} charge station");
        }

        private async Task<ChargeStation> GetChargeStation(Guid groupId, Guid chargeStationId)
        {
            var group = await GetGroup(groupId);
            var chargeStation = group.ChargeStations.FirstOrDefault(f => f.Id == chargeStationId);
            if (chargeStation != null)
            {
                return chargeStation;
            }

            throw new EntityNotFoundException($"The charge station with {chargeStationId} not found");
        }

        private async Task<Group> GetGroup(Guid groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group != null)
            {
                return group;
            }

            throw new EntityNotFoundException($"The group with {groupId} not found");
        }
    }
}
