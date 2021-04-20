using System.Collections.Generic;
using AutoMapper;
using GreenFlux.Domain;
using GreenFlux.Service.Mappings;

namespace GreenFlux.Service.UnitTests
{
    public class TestBase
    {
        public readonly IMapper mapper;

        public TestBase()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            mapper = mapConfig.CreateMapper();
        }

        public Group GetGroup()
        {
            var group = new Group(Constants.GroupId, "Name", Constants.GroupCapacity);
            var chargeStation = new ChargeStation(group, Constants.ChargeStationName, new List<int> {1, 2, 3});
            group.ChargeStations = new List<ChargeStation> {chargeStation};
            return group;
        }

        public Group GetSuggestInstanceGroup()
        {
            var group = new Group(Constants.GroupId, "Suggest Group", 50);
            var firstStation = new ChargeStation(group, "First Station", new List<int> { 5, 10, 12  });
            var secondStation = new ChargeStation(group, "Second Station", new List<int> { 2, 5, 8  });
            group.ChargeStations = new List<ChargeStation> { firstStation, secondStation };
            return group;
        }
    }
}
