using System;

namespace GreenFlux.Api.Models
{
    public class ConnectorModel
    {
        public int Id { get; set; }

        public int MaxCurrent { get; set; }

        public Guid ChargeStationId { get; set; }
    }
}
