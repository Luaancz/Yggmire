using Luaan.Yggmire.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Behaviours
{
    public interface IItemBehaviourWithActions : IItemBehaviour
    {
        ItemAction[] GetAvailableActions(WorldItem parent);
    }
}
