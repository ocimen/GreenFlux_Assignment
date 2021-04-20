using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenFlux.Api.Models;

namespace GreenFlux.Service
{
    public interface ISuggestionService
    {
        Task<List<List<ConnectorModel>>> SuggestRemovalConnectors(Guid groupId, ICollection<int> connectors, int diff);
    }
}