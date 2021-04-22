using System;
using System.Linq;
using System.Threading.Tasks;
using GreenFlux.Api.Models.RequestModels;
using GreenFlux.Data;
using GreenFlux.Domain;
using GreenFlux.Persistence.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GreenFlux.Service.UnitTests
{
    public class ConnectorServiceTests : TestBase
    {
        private readonly IConnectorService _connectorService;
        private Mock<IGenericRepository<Group>> mockGroupRepository;
        private readonly Group _group;
        private readonly ChargeStation _chargeStation;

        public ConnectorServiceTests()
        {
            mockGroupRepository = new Mock<IGenericRepository<Group>>();
            _group = GetGroup();
            _chargeStation = _group.ChargeStations.FirstOrDefault();

            var mockLogger = new Mock<ILogger<ConnectorService>>();
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_group);
            mockGroupRepository.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);

            _connectorService = new ConnectorService(mockGroupRepository.Object, mapper, mockLogger.Object);
        }

        [Fact]
        public async Task Should_Get_All_Connectors_By_Charge_Station()
        {
            var result = await _connectorService.GetAllConnectorsByChargeStation(_group.Id, _chargeStation.Id);

            Assert.NotNull(result);
            Assert.Equal(result.Count, _chargeStation.Connectors.Count);
            Assert.Equal(result.First().Id, _chargeStation.Connectors.First().Id);
            Assert.Equal(result.First().MaxCurrent, _chargeStation.Connectors.First().MaxCurrent);
        }

        [Fact]
        public async Task Should_Get_Connector_Detail()
        {
            var connectorId = 1;
            var connector = _chargeStation.Connectors.FirstOrDefault(f => f.Id == connectorId);
            var result = await _connectorService.GetConnectorDetail(_group.Id, _chargeStation.Id, connectorId);

            Assert.NotNull(result);
            Assert.Equal(result.Id, connector.Id);
            Assert.Equal(result.MaxCurrent, connector.MaxCurrent);
        }

        [Fact]
        public async Task Should_Add_Connector()
        {
            var createConnector = new CreateConnector {MaxCurrent = 3};
            var result = await _connectorService.AddConnector(_group.Id, _chargeStation.Id, createConnector);

            Assert.NotNull(result);
            Assert.Equal(4, _chargeStation.Connectors.Count);
        }

        [Fact]
        public async Task Should_Not_Add_Connector_If_Group_Capacity_Exceeds()
        {
            var createConnector = new CreateConnector { MaxCurrent = _group.Capacity };
            await Assert.ThrowsAsync<GroupCapacityExceedsException>(() => _connectorService.AddConnector(_group.Id, _chargeStation.Id, createConnector));
        }

        [Fact]
        public async Task Should_Not_Add_More_Than_Max_Connector()
        {
            var createConnector = new CreateConnector { MaxCurrent = 1 };

            await _connectorService.AddConnector(_group.Id, _chargeStation.Id, createConnector);
            await _connectorService.AddConnector(_group.Id, _chargeStation.Id, createConnector);

            await Assert.ThrowsAsync<MaxConnectorExceedsException>(() => _connectorService.AddConnector(_group.Id, _chargeStation.Id, createConnector));
        }

        [Fact]
        public void Should_Remove_Connector()
        {
            _connectorService.Remove(_group.Id, _chargeStation.Id, 1);

            Assert.Equal(2, _chargeStation.Connectors.Count);
            Assert.DoesNotContain(_chargeStation.Connectors, x => x.Id == 1);
        }

        [Fact]
        public void Should_Remove_All_connectors_By_Charge_Station()
        {
            _connectorService.RemoveAllConnectorsByChargeStation(_group.Id, _chargeStation.Id);

            Assert.NotNull(_chargeStation.Connectors);
            Assert.Equal(0, _chargeStation.Connectors.Count);
        }

        [Fact]
        public async Task Should_Update_Connector()
        {
            var connectorId = 1;
            var updateConnector = new UpdateConnector {MaxCurrent = 2};
            await _connectorService.UpdateConnector(_group.Id, _chargeStation.Id, connectorId, updateConnector);

            var updatedConnector = _chargeStation.Connectors.FirstOrDefault(f => f.Id == connectorId);
            Assert.Equal(updatedConnector.MaxCurrent, updateConnector.MaxCurrent);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Updated_Connector_Capacity_Exceeds_Group_Capacity()
        {
            var connectorId = 1;
            var updateConnector = new UpdateConnector { MaxCurrent = 20 };

            await Assert.ThrowsAsync<GroupCapacityExceedsException>(() => _connectorService.UpdateConnector(_group.Id, _chargeStation.Id, connectorId, updateConnector));
        }

    }
}
