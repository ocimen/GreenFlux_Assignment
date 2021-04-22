using System;
using System.Collections.Generic;
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
    public class GroupServiceTests : TestBase
    {
        private readonly IGroupService _groupService;
        private Mock<IGenericRepository<Group>> mockGroupRepository;
        private Mock<IChargeStationService> mockChargeStationService;

        public GroupServiceTests()
        {
            mockGroupRepository = new Mock<IGenericRepository<Group>>();
            mockChargeStationService = new Mock<IChargeStationService>();
            
            var mockLogger = new Mock<ILogger<GroupService>>();
            _groupService = new GroupService(mockGroupRepository.Object, mockChargeStationService.Object, mapper, mockLogger.Object);
        }

        [Fact]
        public async Task Should_Create_Group()
        {
            mockGroupRepository.Setup(s => s.Add(It.IsAny<Group>())).Returns(1);
            mockGroupRepository.Setup(s => s.SaveChangesAsync()).ReturnsAsync(1);

            var name = "Name";
            var capacity = 10;
            var result = await _groupService.Create(name, capacity);

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(capacity, result.Capacity);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Create_Group_Fails()
        {
            mockGroupRepository.Setup(s => s.Add(It.IsAny<Group>())).Returns(1);
            mockGroupRepository.Setup(s => s.SaveChangesAsync()).ReturnsAsync(0);

            await Assert.ThrowsAsync<Exception>(() => _groupService.Create("Name", 10));
        }

        [Fact]
        public void Should_Get_All_Groups()
        {
            var group = GetGroup();
            mockGroupRepository.Setup(s => s.GetAll()).Returns(new List<Group>{group});

            var result = _groupService.GetAll();

            Assert.NotNull(result);
            var groupModels = result.ToList();
            Assert.NotEmpty(groupModels);
            Assert.Equal(group.Name, groupModels.FirstOrDefault()?.Name);
            Assert.Equal(group.Capacity, groupModels.FirstOrDefault()?.Capacity);
        }

        [Fact]
        public async Task Should_Get_Specific_Group()
        {
            var group = GetGroup();
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(group);
            var result = await _groupService.GetByIdAsync(Guid.NewGuid());

            Assert.NotNull(result);
            Assert.Equal(group.Id, result.Id);
            Assert.Equal(group.Name, result.Name);
            Assert.Equal(group.Capacity, result.Capacity);
        }

        [Fact]
        public async Task Should_Not_Found_Specific_Group()
        {
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Group)null);
            var result = await _groupService.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task Should_Update_Group_Name()
        {
            var group = GetGroup();
            var updateGroup = new UpdateGroup{Name = "Updated Name"};
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(group);
            var result = await _groupService.Update(Constants.GroupId, updateGroup);

            Assert.NotNull(result);
            Assert.Equal(updateGroup.Name, result.Name);
            Assert.Equal(group.Capacity, result.Capacity);
        }

        [Fact]
        public async Task Should_Update_Group_Capacity()
        {
            var group = GetGroup();
            var updateGroup = new UpdateGroup { Capacity = 15};
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(group);
            var result = await _groupService.Update(Constants.GroupId, updateGroup);

            Assert.NotNull(result);
            Assert.Equal(updateGroup.Capacity.Value, result.Capacity);
            Assert.Equal(group.Name, result.Name);
        }

        [Fact]
        public async Task Should_Remove_Specific_Group()
        {
            var group = GetGroup();
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(group);
            mockGroupRepository.Setup(s => s.Remove(It.IsAny<Group>())).Returns(group);
            mockChargeStationService.Setup(s => s.RemoveAllChargeStationsByGroup(It.IsAny<Guid>()));

            var result = await _groupService.Remove(group.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Throw_Exception_If_Remove_Group_Fails()
        {
            mockGroupRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Group)null);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _groupService.Remove(Guid.NewGuid()));
        }

       
    }
}
