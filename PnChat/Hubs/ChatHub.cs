using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PnChat.SignalRMappings;
using System.Timers;
using PnChat.Models;
using System.Threading.Tasks;

namespace PnChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private static int counter = 0;
        private static int index = 0;
        static object[,] usageData = new object[50, 3];

        public void SendChatMessage(string to, string from, string message)
        {
            //string name = Context.User.Identity.Name;

            if (from == "")
            {
                from = "Guest";
            }

            if (to == "")
            {
                Clients.All.addChatMessage(from + ": " + message);
            }
            else
            {
                foreach (var connectionId in _connections.GetConnections(to))
                {
                    Clients.Client(connectionId).addChatMessage(from + ": " + message);
                }
            }
        }

        public void AddUser(string name)
        {
            _connections.Add(name, Context.ConnectionId);

            Clients.All.addChatMessage(name + " connected");
        }

        public void GetDateTime()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Clients.All.displayDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public void ServerMessage()
        {
            Timer timer = new Timer(20000);
            timer.Elapsed += PerUserMessage;
            timer.Start();
        }

        void PerUserMessage(object sender, ElapsedEventArgs e)
        {
            List<string> connections = _connections.GetConnections();

            if (counter >= connections.Count)
            {
                counter = 0;
            }

            try
            {
                string connectionId = connections[counter];

                string msg = "Server: Payment Required User: " + connectionId;
                Console.WriteLine(msg);
                Clients.Client(connectionId).addChatMessage(msg);

                counter++;
            }
            catch
            {

            }
        }

        public void DrawRandomChart()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += RandomChart_Elapsed;
            timer.Start();
        }

        void RandomChart_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (index == 0 || index >= 50)
            {
                index = 1;

                usageData[0, 0] = "Time";
                usageData[0, 1] = "CPU";
                usageData[0, 2] = "RAM";
            }

            Usage usage = new Usage();

            usageData[index, 0] = "" + usage.Year;
            usageData[index, 1] = usage.Sales;
            usageData[index, 2] = usage.Expenses;

            index++;

            Clients.All.drawGoogleChart(usageData);
        }

        public override Task OnConnected()
        {
            //string name = Context.User.Identity.Name;
            //_connections.Add(name, Context.ConnectionId);

            GetDateTime();
            //ServerMessage();
            DrawRandomChart();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //string name = Context.User.Identity.Name;
            //_connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //string name = Context.User.Identity.Name;

            //if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            //{
            //    _connections.Add(name, Context.ConnectionId);
            //}

            return base.OnReconnected();
        }
    }
}