using System;
using System.Collections.Generic;
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
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> _groupRepository;
        private readonly IChargeStationService _chargeStationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GroupService(IGenericRepository<Group> groupRepository, IChargeStationService chargeStationService, IMapper mapper, ILogger<GroupService> logger)
        {
            _groupRepository = groupRepository;
            _chargeStationService = chargeStationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GroupModel> Create(string name, int capacity)
        {
            Group group = new Group(name, capacity);
            _groupRepository.Add(group);
            var result = await _groupRepository.SaveChangesAsync();
            if (result > 0)
            {
                return _mapper.Map<GroupModel>(group);
            }

            var exceptionMessage = "The group could not save";
            _logger.LogError(exceptionMessage);
            throw new GreenFluxException(exceptionMessage);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            var allGroups = _groupRepository.GetAll();
            return _mapper.Map<IEnumerable<GroupModel>>(allGroups);
        }

        public async Task<GroupModel> GetByIdAsync(Guid id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group != null)
            {
                _logger.LogInformation($"Group was queried which id is: {group.Id}");
                return _mapper.Map<GroupModel>(group);
            }

            return null;
        }

        public async Task<GroupModel> Update(Guid groupId, UpdateGroup updateGroup)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (!string.IsNullOrEmpty(updateGroup.Name))
            {
                group.UpdateName(updateGroup.Name);
            }

            if (updateGroup.Capacity.HasValue)
            {
                group.UpdateCapacity(updateGroup.Capacity.Value);
            }

            await _groupRepository.SaveChangesAsync();
            return _mapper.Map<GroupModel>(group);
        }

        public async Task<bool> Remove(Guid id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group != null)
            {
                // Remove related charge stations
                await _chargeStationService.RemoveAllChargeStationsByGroup(id);
                _logger.LogInformation($"All charge stations were removed which are under the group of: {group.Id}");
                
                // Remove group
                var deletedGroup = _groupRepository.Remove(group);
                if (deletedGroup != null)
                {
                    _logger.LogInformation($"Group was removed which id is : {group.Id}");
                    await _groupRepository.SaveChangesAsync();
                    return true;
                }
            }

            throw new EntityNotFoundException($"Group not found with id: {id}");
        }
    }
}
