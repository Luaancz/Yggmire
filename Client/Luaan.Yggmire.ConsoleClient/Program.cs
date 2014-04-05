using Luaan.Yggmire.OrleansInterfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Luaan.Yggmire.ConsoleClient
{
    class Program
    {
        static CancellationTokenSource cancellation;

        static void Main(string[] args)
        {
            cancellation = new CancellationTokenSource();

            Console.CancelKeyPress += Console_CancelKeyPress;

            OrleansClient.Initialize();

            try
            {
                MainAsync().Wait(cancellation.Token);
            }
            catch (OperationCanceledException)
            { }

            Console.WriteLine("That is all. Press any key...");
            Console.ReadLine();
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            cancellation.Cancel();
        }

        static async Task MainAsync()
        {
            var session = SessionGrainFactory.GetGrain(Guid.NewGuid());           
            var account = await session.CreateAccount("Luaan", "pwd");

            Console.WriteLine(account.Name);
        }
    }
}
