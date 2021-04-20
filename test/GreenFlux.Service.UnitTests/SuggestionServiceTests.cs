using System;
using System.Collections.Generic;
using System.Linq;
using GreenFlux.Api.Models;
using GreenFlux.Domain;
using Moq;
using Xunit;

namespace GreenFlux.Service.UnitTests
{
    public class SuggestionServiceTests : TestBase
    {
        private readonly ISuggestionService _suggestionService;
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly Group _group;

        public SuggestionServiceTests()
        {
            _group = GetSuggestInstanceGroup();

            var groupModel = mapper.Map<GroupModel>(_group);
            _mockGroupService = new Mock<IGroupService>();
            _mockGroupService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(groupModel);

            _suggestionService = new SuggestionService(_mockGroupService.Object);
        }

        [Fact]
        public async void Should_Suggest_Exact_Match()
        {
            var connectors = new List<int> {5, 10, 5};

            var result = await _suggestionService.SuggestRemovalConnectors(_group.Id, connectors, 5);

            Assert.Single(result.FirstOrDefault());
        }

        [Fact]
        public async void Should_Suggest_Connector_Pairs()
        {
            var connectors = new List<int> { 5, 20, 5 };

            var result = await _suggestionService.SuggestRemovalConnectors(_group.Id, connectors, 15);

            Assert.True(result.FirstOrDefault()?.Count > 1);
        }
    }
}
