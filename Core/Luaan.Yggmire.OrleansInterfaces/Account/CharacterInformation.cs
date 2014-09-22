using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Concurrency;

namespace Luaan.Yggmire.OrleansInterfaces.Account
{
    [Serializable]
    [Immutable]
    public class CharacterInformation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
