using System;
using System.Collections.Generic;

namespace GreenFlux.Api.Models
{
    public class GroupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public ICollection<ChargeStationModel> ChargeStations { get; set; }
    }
}
