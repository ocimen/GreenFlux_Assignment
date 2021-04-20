using System;
using System.Collections.Generic;
using System.Linq;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Data;
using GreenFlux.Domain;
using GreenFlux.Persistence.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GreenFlux.Service.UnitTests
{
    public class ChargeStationServiceTests : TestBase
    {
        private readonly IChargeStationService _chargeStationService;
        private Mock<IGenericRepository<Group>> mockGroupRepository;
        private readonly Group _group;

        public ChargeStationServiceTests()
        {
            mockGroupRepository = new Mock<IGenericRepository<Group>>();

            _group = GetGroup();
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_group);
            mockGroupRepository.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);

            var mockLogger = new Mock<ILogger<ChargeStationService>>();

            _chargeStationService = new ChargeStationService(mockGroupRepository.Object, mapper, mockLogger.Object);
        }

        [Fact]
        public async void Should_GetAllChargeStationsByGroup()
        {
            var result = await _chargeStationService.GetAllChargeStationsByGroup(_group.Id);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(Constants.ChargeStationName, result.FirstOrDefault()?.Name);
        }

        [Fact]
        public async void Should_Throw_Exception_If_Group_Not_Exists()
        {
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Group)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _chargeStationService.GetAllChargeStationsByGroup(Guid.NewGuid()));
        }

        [Fact]
        public async void Should_Get_Specific_Charge_Station_Detail()
        {
            var chargeStationId =_group.ChargeStations.FirstOrDefault().Id;
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_group);

            var result = await _chargeStationService.GetChargeStationDetail(_group.Id, chargeStationId);

            Assert.NotNull(result);
            Assert.Equal(Constants.ChargeStationName, result.Name);
        }

        [Fact]
        public async void Should__If_Charge_Station_Not_Exists()
        {
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_group);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _chargeStationService.GetChargeStationDetail(_group.Id, Guid.NewGuid()));
        }

        [Fact]
        public void Should_Add_Charge_Station()
        {
            var chargeStation = new CreateChargeStation {Name = "New Station", Connectors = new List<int> {1}};

            var result = _chargeStationService.AddChargeStation(_group.Id, chargeStation);

            Assert.NotNull(result);
            Assert.Equal(2, _group.ChargeStations.Count);
            Assert.Contains(_group.ChargeStations, a => a.Name == chargeStation.Name);
        }

        [Fact]
        public void Should_Throw_Exception_If_Group_Capacity_Exceeds_When_Add_Charge_Station()
        {
            var chargeStation = new CreateChargeStation { Name = "New Station", Connectors = new List<int> { 100 } };

            Assert.ThrowsAsync<GroupCapacityExceedsException>(() => _chargeStationService.AddChargeStation(_group.Id, chargeStation));
        }

        [Fact]
        public void Should_Remove_Charge_Station()
        {
            _chargeStationService.Remove(_group.Id, _group.ChargeStations.FirstOrDefault().Id);

            Assert.Equal(0, _group.ChargeStations.Count);
            Assert.Null(_group.ChargeStations.FirstOrDefault());
        }

        [Fact]
        public async void Should_Remove_All_Charge_Station_By_Group()
        {
            _chargeStationService.RemoveAllChargeStationsByGroup(_group.Id);

            Assert.Equal(0, _group.ChargeStations.Count);
            Assert.Null(_group.ChargeStations.FirstOrDefault());
        }

        [Fact]
        public async void Should_Update_Charge_Station()
        {
            var updateChargeStation = new UpdateChargeStation {Name = "Updated Station"};
            var updated = await _chargeStationService.Update(_group.Id, _group.ChargeStations.FirstOrDefault().Id, updateChargeStation);

            Assert.NotNull(updated);
            Assert.Equal(updated.Name, updateChargeStation.Name);
        }
    }
}
