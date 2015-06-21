//
// Copyright (C) 2014-2015 Stéphane Lenclud.
//
// This file is part of SharpDisplayManager.
//
// SharpDisplayManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SharpDisplayManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SharpDisplayManager.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
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
	}
}
