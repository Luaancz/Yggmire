using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansInterfaces
{
    public interface ISessionObserver : IGrainObserver
    {
        void ReadyForChat();
        void Disconnected();

        void SystemMessage(string message);

        void ShowDialog(string message);
        void ShowInputDialog(int responseId, string message);
    }
}
