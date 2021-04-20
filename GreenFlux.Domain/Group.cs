using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenFlux.Domain
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public ICollection<ChargeStation> ChargeStations { get; set; }

        public int CurrentCapacity
        {
            get
            {
               return ChargeStations?.Sum(s => s.TotalCurrent) ?? 0;
            }
        }

        public Group(Guid id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }

        public Group(string name, int capacity)
        {
            Id = Guid.NewGuid();
            UpdateCapacity(capacity);
            UpdateName(name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateCapacity(int capacity)
        {
            Capacity = capacity;
        }

        public void AddChargeStation(ChargeStation chargeStation)
        {
            ChargeStations.Add(chargeStation);
        }

        public void RemoveChargeStation(Guid chargeStationId)
        {
            var chargeStation = ChargeStations.FirstOrDefault(f => f.Id == chargeStationId);
            ChargeStations.Remove(chargeStation);
        }
    }
}
