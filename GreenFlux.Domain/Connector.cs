using System;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Domain
{
    [Owned]
    public class Connector
    {
        public int Id { get; set; }

        public int MaxCurrent { get; set; }

        public Guid ChargeStationId { get; set; }

        public void UpdateMaxCurrent(int maxCurrent)
        {
            MaxCurrent = maxCurrent;
        }
    }
}
