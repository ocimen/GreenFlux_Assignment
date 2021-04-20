using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenFlux.Api.Models;

namespace GreenFlux.Service
{
    public class SuggestionService : ISuggestionService
    {
        private readonly IGroupService _groupService;

        public SuggestionService(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public async Task<List<List<ConnectorModel>>> SuggestRemovalConnectors(Guid groupId, ICollection<int> connectors, int diff)
        {
            var removalSuggestionList = new List<List<ConnectorModel>>();
            var connectorList = await GetAllConnectorsByGroup(groupId);

            //TODO: Find all subarrays with a given sum k in an array

            // Check if there is an exact match with the diff
            var exactMatch = connectorList.Where(x => x.MaxCurrent == diff).ToList();
            if (exactMatch.Count > 0)
            {
                removalSuggestionList.AddRange(exactMatch.Select(match => new List<ConnectorModel> {match}));
            }

            // Find connector pairs
            var connectorArray = connectorList.ToArray();
            for (int i = 0; i < connectorArray.Length; i++)
            {
                for (int j = 0; j < connectorArray.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (connectorArray[i].MaxCurrent + connectorArray[j].MaxCurrent == diff)
                    {
                        removalSuggestionList.Add(new List<ConnectorModel> { connectorArray[i], connectorArray[j] });
                    }

                    if (connectorArray[i].MaxCurrent + connectorArray[j].MaxCurrent > diff)
                    {
                        break;
                    }
                }
            }

            return removalSuggestionList;
        }

        private async Task<List<ConnectorModel>> GetAllConnectorsByGroup(Guid groupId)
        {
            var group = await _groupService.GetByIdAsync(groupId);

            var connectorList = group.ChargeStations
                .SelectMany(s => s.Connectors
                    .Select(c => new ConnectorModel{ Id = c.Id, MaxCurrent = c.MaxCurrent, ChargeStationId = c.ChargeStationId}))
                .OrderBy(o => o.MaxCurrent)
                .ToList();
            
            return connectorList;
        }
    }
}
