using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Behaviours
{
    public interface IItemBehaviour
    {

    }

    public interface IItemBehaviour<TState> : IItemBehaviour
        where TState : IItemBehaviourState
    {
        string Id { get; set; }
    }
}
