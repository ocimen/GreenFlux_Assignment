using System;
using System.Collections.Generic;

namespace GreenFlux.Api.Models
{
    public class ChargeStationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GroupId { get; set; }
        public ICollection<ConnectorModel> Connectors { get; set; }
    }
}
