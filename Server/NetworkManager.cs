using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using NETWORKLIST;

namespace SharpDisplayManager
{
	public class NetworkManager: INetworkListManagerEvents, IDisposable
    {
		public delegate void OnConnectivityChangedDelegate(NetworkManager aNetworkManager, NLM_CONNECTIVITY aConnectivity);
		public event OnConnectivityChangedDelegate OnConnectivityChanged;
		
		private int iCookie = 0;
        private IConnectionPoint iConnectionPoint;
        private INetworkListManager iNetworkListManager;


		public NetworkManager()
		{
			iNetworkListManager = new NetworkListManager();
			ConnectToNetworkListManagerEvents();
		}

		public void Dispose()
		{
			//Not sure why this is not working form here
			//Possibly because something is doing automatically before we get there
			//DisconnectFromNetworkListManagerEvents();
		}


		public INetworkListManager NetworkListManager
		{
			get { return iNetworkListManager; }
		}

		public void ConnectivityChanged(NLM_CONNECTIVITY newConnectivity)
		{
			//Fire our event
			OnConnectivityChanged(this, newConnectivity);
		}

		public void ConnectToNetworkListManagerEvents()
		{
			Debug.WriteLine("Subscribing to INetworkListManagerEvents");
			IConnectionPointContainer icpc = (IConnectionPointContainer)iNetworkListManager;
			//similar event subscription can be used for INetworkEvents and INetworkConnectionEvents
			Guid tempGuid = typeof(INetworkListManagerEvents).GUID;
			icpc.FindConnectionPoint(ref tempGuid, out iConnectionPoint);
			iConnectionPoint.Advise(this, out iCookie);
			
		}

		public void DisconnectFromNetworkListManagerEvents()
		{
			Debug.WriteLine("Un-subscribing to INetworkListManagerEvents");
			iConnectionPoint.Unadvise(iCookie);
		} 

		/// <summary>
		/// Indicates whether any network connection is available
		/// Filter connections below a specified speed, as well as virtual network cards.
		/// </summary>
		/// <returns>
		///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAvailable()
		{
			return IsAvailable(0);
		}

		/// <summary>
		/// Indicates whether any network connection is available.
		/// Filter connections below a specified speed, as well as virtual network cards.
		/// </summary>
		/// <param name="minimumSpeed">The minimum speed required. Passing 0 will not filter connection using speed.</param>
		/// <returns>
		///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAvailable(long minimumSpeed)
		{
			if (!NetworkInterface.GetIsNetworkAvailable())
				return false;

			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				// discard because of standard reasons
				if ((ni.OperationalStatus != OperationalStatus.Up) ||
					(ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
					(ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
					continue;

				// this allow to filter modems, serial, etc.
				// I use 10000000 as a minimum speed for most cases
				if (ni.Speed < minimumSpeed)
					continue;

				// discard virtual cards (virtual box, virtual pc, etc.)
				if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
					(ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
					continue;

				// discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
				if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
					continue;

				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool HasInternet()
		{
			return false;
			//ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
			//bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
			//return internet;
		}

	}
}
