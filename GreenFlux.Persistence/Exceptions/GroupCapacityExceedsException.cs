using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GreenFlux.Persistence.Exceptions
{
    [Serializable]
    public class GroupCapacityExceedsException : GreenFluxException
    {
        public Guid GroupId { get; }

        public int Diff { get; }

        public List<int> Connector { get; }
        
        public GroupCapacityExceedsException(Guid groupId, List<int> connector, int diff) : base("You can not add more than {group.Capacity} capacity to the related Group")
        {
            GroupId = groupId;
            Connector = connector;
            Diff = diff;
        }

        protected GroupCapacityExceedsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
