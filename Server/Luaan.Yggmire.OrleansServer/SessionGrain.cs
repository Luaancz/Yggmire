using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansServerInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
using Luaan.Yggmire.OrleansServerInterfaces.Account;
using Luaan.Yggmire.OrleansServerInterfaces.Chat;
using Luaan.Yggmire.OrleansInterfaces.Chat;

namespace Luaan.Yggmire.OrleansServer
{
    /// <summary>
    /// Orleans grain implementation class SessionGrain
    /// </summary>
    public class SessionGrain : Orleans.GrainBase, ISessionGrain
    {
        IAccountGrain loggedAccount;
        ICharacterGrain activeCharacter;

        bool isCharacterComplete;
        string name;

        ObserverSubscriptionManager<ISessionObserver> observers;

        void CheckLogged()
        {
            if (loggedAccount == null)
                throw new UnauthorizedAccessException("Not logged in or session expired.");
        }

        void CheckCharacter()
        {
            CheckLogged();

            if (activeCharacter == null)
                throw new UnauthorizedAccessException("No character selected.");
        }

        public override Task ActivateAsync()
        {
            observers = new ObserverSubscriptionManager<ISessionObserver>();
            mySubscriptions = new List<Tuple<int, IChatObserver>>();

            return base.ActivateAsync();
        }

        /// <summary>
        /// When the session dies, we have to clean up a bit.
        /// </summary>
        /// <returns></returns>
        public async override Task DeactivateAsync()
        {
            if (loggedAccount != null)
                await loggedAccount.CaptureSession(null);

            if (mySubscriptions.Count > 0)
            {
                var tasks = 
                    mySubscriptions.Select
                        (
                            i => 
                                {
                                    var chat = ChatGrainFactory.GetGrain(i.Item1);

                                    return chat.Unsubscribe(name, i.Item2);
                                }
                        ).ToArray();

                await Task.WhenAll(tasks);
            }
        }

        async Task<AccountInformation> CompleteLogin(IAccountGrain account)
        {
            await account.CaptureSession(this);
            loggedAccount = account;

            var state = await account.GetState();
            name = state.Name;

            return state;
        }

        /// <summary>
        /// Checks the account credentials, and if they are valid, completes the login process.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        async Task<AccountInformation> ISessionGrain.Authorize(string name, string password)
        {
            var account = AccountGrainFactory.GetGrain(0, name);

            if (account != null && await account.ValidatePassword(password))
            {
                return await CompleteLogin(account);
            }
            else
                throw new UnauthorizedAccessException("Wrong username or password.");
        }

        /// <summary>
        /// Creates a new account with the specified name and password, and if everything goes well, completes the login process.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        async Task<AccountInformation> ISessionGrain.CreateAccount(string name, string password)
        {
            var account = AccountGrainFactory.GetGrain(0, name);
            
            await account.Create(password);

            return await CompleteLogin(account);
        }

        async Task<CharacterInformation> ISessionGrain.CreateCharacter()
        {
            CheckLogged();

            activeCharacter = await loggedAccount.CreateCharacter();
            isCharacterComplete = false;

            return await activeCharacter.GetInfo();
        }

        async Task<CharacterInformation> ISessionGrain.SelectCharacter(string name)
        {
            CheckLogged();

            activeCharacter = await loggedAccount.SelectCharacter(name);
            isCharacterComplete = true;

            var info = await activeCharacter.GetInfo();
            this.name = info.Name;

            return info;
        }

        Task<AccountInformation> ISessionGrain.GetAccount()
        {
            CheckLogged();

            return loggedAccount.GetState();
        }

        Task<string> ISessionGrain.CharacterName
        {
            get
            {
                CheckCharacter();

                return Task.FromResult(name);
            }
        }

        async Task ISessionGrain.SendChatMessage(int channel, string message)
        {
            CheckCharacter();

            if (!isCharacterComplete)
                throw new InvalidOperationException("Character not yet ready for chat!");

            var chat = ChatGrainFactory.GetGrain(channel);

            await chat.SendMessage(name, message);
        }

        private List<Tuple<int, IChatObserver>> mySubscriptions;

        Task ISessionGrain.SubscribeForChat(int channel, IChatObserver observer)
        {
            CheckCharacter();

            if (!isCharacterComplete)
                throw new InvalidOperationException("Character not yet ready for chat!");

            var chat = ChatGrainFactory.GetGrain(channel);

            if (mySubscriptions.Any(i => i.Item1 == channel))
                throw new InvalidOperationException("Already subscribed to channel " + channel);

            mySubscriptions.Add(new Tuple<int, IChatObserver>(channel, observer));
            return chat.Subscribe(name, observer);
        }

        Task ISessionGrain.UnsubscribeFromChat(int channel, IChatObserver observer)
        {
            CheckCharacter();

            if (!isCharacterComplete)
                throw new InvalidOperationException("Character not yet ready for chat!");

            var chat = ChatGrainFactory.GetGrain(channel);

            if (mySubscriptions.RemoveAll(i => i.Item1 == channel) == 0)
                throw new InvalidOperationException("Not subscribed to channel " + channel);

            return chat.Unsubscribe(name, observer);
        }

        /// <summary>
        /// Disconnects from the session.
        /// </summary>
        /// <returns></returns>
        Task ISessionGrain.Disconnect()
        {
            observers.Notify(o => o.Disconnected());

            DeactivateOnIdle();

            return TaskDone.Done;
        }

        int responseId = 1;
        Dictionary<int, Func<string, Task>> expectedResponses = new Dictionary<int,Func<string, Task>>();

        async Task ISessionGrain.Respond(int responseId, string response)
        {
            if (!expectedResponses.ContainsKey(responseId))
                throw new InvalidOperationException("Already responded.");

            await expectedResponses[responseId](response);

            // If there's no exception, everything went fine.
            expectedResponses.Remove(responseId);
        }

        Task ISessionGrain.RegisterObserver(ISessionObserver observer)
        {
            observers.Subscribe(observer);

            // This is a good time to handle enter-game notifications. Later.
            if (!isCharacterComplete)
            {
                // Let's ask for a name to get this character finished!
                expectedResponses.
                    Add
                    (
                        responseId,
                        async (response) =>
                        {
                            if (!string.IsNullOrEmpty(response))
                            {
                                await activeCharacter.SetName(response);
                                await activeCharacter.Complete();
                                isCharacterComplete = true;

                                observers.Notify(o => o.ReadyForChat());
                            }
                        }
                    );

                observer.ShowInputDialog(responseId++, "Please, enter your character's name: ");
            }
            else
            {
                observers.Notify(o => o.ReadyForChat());
            }

            return TaskDone.Done;
        }
    }
}
