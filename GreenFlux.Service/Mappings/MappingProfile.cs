using AutoMapper;
using GreenFlux.Api.Models;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Domain;

namespace GreenFlux.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Group, GroupModel>();
            CreateMap<ChargeStation, ChargeStationModel>();
            CreateMap<Connector, ConnectorModel>();

            CreateMap<ConnectorModel, Connector>();
            CreateMap<CreateConnector, Connector>();
            CreateMap<UpdateConnector, ConnectorModel>();
        }
    }
}
