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
    public class ChargeStationService : IChargeStationService
    {
        private readonly IGenericRepository<Group> _groupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ChargeStationService(IGenericRepository<Group> groupRepository, IMapper mapper, ILogger<ChargeStationService> logger)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ChargeStationModel>> GetAllChargeStationsByGroup(Guid groupId)
        {
            var group = await GetGroup(groupId);
            return _mapper.Map<ICollection<ChargeStationModel>>(group.ChargeStations);
        }

        public async Task<ChargeStationModel> GetChargeStationDetail(Guid groupId, Guid chargeStationId)
        {
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            return _mapper.Map<ChargeStationModel>(chargeStation);
        }

        public async Task<ChargeStationModel> AddChargeStation(Guid groupId, CreateChargeStation chargeStation)
        {
            var group = await GetGroup(groupId);
            var newChargeStation = new ChargeStation(group, chargeStation.Name, chargeStation.Connectors);
        
            group.AddChargeStation(newChargeStation);
            await _groupRepository.SaveChangesAsync();
            return _mapper.Map<ChargeStationModel>(newChargeStation);
        }

        public async Task Remove(Guid groupId, Guid chargeStationId)
        {
            var group = await GetGroup(groupId);
            group.RemoveChargeStation(chargeStationId);
            await _groupRepository.SaveChangesAsync();
            _logger.LogInformation($"Charge station with {chargeStationId} was removed from the group");
        }

        public async Task RemoveAllChargeStationsByGroup(Guid groupId)
        {
            var group = await GetGroup(groupId);
            group.ChargeStations = new List<ChargeStation>();
            
            _logger.LogInformation($"All Charge station was removed  under the group of {group.Id}");
            await _groupRepository.SaveChangesAsync();
        }

        public async Task<ChargeStationModel> Update(Guid groupId, Guid chargeStationId, UpdateChargeStation updateChargeStation)
        {
            var chargeStation = await GetChargeStation(groupId, chargeStationId);
            chargeStation.UpdateStationName(updateChargeStation.Name);
            
            await _groupRepository.SaveChangesAsync();
            return _mapper.Map<ChargeStationModel>(chargeStation);
        }

        private async Task<ChargeStation> GetChargeStation(Guid groupId, Guid chargeStationId)
        {
            var group = await GetGroup(groupId);
            var chargeStation = group.ChargeStations.FirstOrDefault(x => x.Id == chargeStationId);
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
