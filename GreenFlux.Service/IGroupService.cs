using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFlux.Api.Models;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Service
{
    public interface IGroupService
    {
        IEnumerable<GroupModel> GetAll();

        Task<GroupModel> GetByIdAsync(Guid id);

        Task<GroupModel> Create(string name, int capacity);

        Task<bool> Remove(Guid id);

        Task<GroupModel> Update(Guid groupId, UpdateGroup updateGroup);
    }
}