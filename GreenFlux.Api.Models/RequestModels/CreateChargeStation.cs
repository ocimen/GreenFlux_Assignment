using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Api.Models.RequestModels
{
    public class CreateChargeStation
    {
        public string Name { get; set; }

        public ICollection<int> Connectors { get; set; }
    }
}
