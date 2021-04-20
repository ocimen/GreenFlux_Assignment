using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFlux.Api.Models;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Service
{
    public interface IChargeStationService
    {
        Task<ICollection<ChargeStationModel>> GetAllChargeStationsByGroup(Guid groupId);
        Task<ChargeStationModel> GetChargeStationDetail(Guid groupId, Guid chargeStationId);
        Task<ChargeStationModel> AddChargeStation(Guid groupId, CreateChargeStation chargeStation);
        Task<ChargeStationModel> Update(Guid groupId, Guid chargeStationId, UpdateChargeStation updateChargeStation);
        void Remove(Guid groupId, Guid chargeStationId);
        void RemoveAllChargeStationsByGroup(Guid groupId);
    }
}