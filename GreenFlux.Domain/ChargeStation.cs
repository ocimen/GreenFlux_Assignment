using System;
using System.Collections.Generic;
using System.Linq;
using GreenFlux.Persistence.Exceptions;

namespace GreenFlux.Domain
{
    public class ChargeStation
    {
        private const int MinConnectors = 1;
        private const int MaxConnectors = 5;

        internal int TotalCurrent
        {
            get
            {
                return Connectors?.Sum(s => s.MaxCurrent) ?? 0;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GroupId { get; set; }
        public ICollection<Connector> Connectors { get; set; }

        public ChargeStation()
        {}

        public ChargeStation(Group group, string name, ICollection<int> connectorCapacity)
        {
            Id = Guid.NewGuid();
            Name = name;
            GroupId = group.Id;
            AddConnectors(group, connectorCapacity);
        }

        public void UpdateStationName(string name)
        {
            Name = name;
        }

        public void AddConnectors(Group group, ICollection<int> connectors)
        {
            Connectors ??= new List<Connector>();
            var connectorsSum = connectors.Sum(s => s);
            if (group.CurrentCapacity + connectorsSum > group.Capacity)
            {
                var diff = group.Capacity - (group.CurrentCapacity + connectorsSum);
                throw new GroupCapacityExceedsException(group.Id, connectors.ToList(), Math.Abs(diff));
            }

            foreach (var connector in connectors)
            {
                var nextConnectorId = GetAvailableNextConnectorId();
                if (nextConnectorId > MaxConnectors)
                {
                    throw new MaxConnectorExceedsException($"You can not add more than {MaxConnectors} connectors to the ChargeStation");
                }

                Connectors.Add(new Connector
                {
                    Id = nextConnectorId,
                    MaxCurrent = connector,
                    ChargeStationId = Id
                });
            }
        }

        public void RemoveConnector(int id)
        {
            var connector = Connectors.FirstOrDefault(f => f.Id == id);
            Connectors.Remove(connector);
        }

        private int GetAvailableNextConnectorId()
        {
            if (Connectors == null)
            {
                return 1;
            }

            int i = MinConnectors;
            for (;i <= MaxConnectors; i++)
            {
                if (Connectors.Any(x => x.Id == i))
                {
                    continue;
                }

                return ++i;

            }
            return ++i;
        }
    }
}
