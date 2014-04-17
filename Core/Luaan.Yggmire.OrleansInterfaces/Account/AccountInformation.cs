using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansInterfaces.Account
{
    [Serializable]
    public class AccountInformation
    {
        public string Name { get; set; }

        public List<CharacterInformation> Characters { get; set; }
    }
}
