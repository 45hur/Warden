using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Whalebone.Warden
{
    internal class Watcher : IDisposable
    {
        private EventLog eventLog;

        public Watcher()
        {
            if (!EventLog.SourceExists("Watcher"))
            {
                EventLog.CreateEventSource("Whalebone Watcher", "Whalebone");
            }

            eventLog = new EventLog {Source = "Whalebone Watcher" };

            eventLog.WriteEntry("Register AddressChangedCallback");
            NetworkChange.NetworkAddressChanged += AddressChangedCallback;
        }

        public void Dispose()
        {
            eventLog.WriteEntry("Unregister AddressChangedCallback");
            NetworkChange.NetworkAddressChanged -= AddressChangedCallback;
        }

        private void AddressChangedCallback(object sender, EventArgs e)
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var n in adapters)
            {
                eventLog.WriteEntry($"   {n.Name} is {n.OperationalStatus}");
            }
        }
    }
}
