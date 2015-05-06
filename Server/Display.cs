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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;
//using System.Runtime.Serialization;

using MiniDisplayInterop;

namespace SharpDisplayManager
{


    /// <summary>
    /// Provide access to our display hardware through MiniDisplay API.
    /// </summary>
    public class Display
    {
		public delegate void OnOpenedHandler(Display aDisplay);
		public event OnOpenedHandler OnOpened;

		public delegate void OnClosedHandler(Display aDisplay);
		public event OnClosedHandler OnClosed;

		//Our display device handle
		IntPtr iDevice;

		//static functions
		public static int TypeCount()
		{
			return MiniDisplay.TypeCount();
		}

		public static string TypeName(MiniDisplay.Type aType)
		{
			IntPtr ptr = MiniDisplay.TypeName(aType);
			string str = Marshal.PtrToStringUni(ptr);
			return str;
		}

        //Constructor
        public Display()
        {
            iDevice = IntPtr.Zero;
        }

        //
        public bool Open(MiniDisplay.Type aType)
        {
			if (IsOpen())
			{
				//Already open return an error
				return false;
			}

            iDevice = MiniDisplay.Open(aType);

            bool success = iDevice != IntPtr.Zero;
			if (success)
			{
				//Broadcast opened event
				OnOpened(this);
			}

			return success;
        }

        public void Close()
        {
			if (!IsOpen())
			{
				//Pointless
				return;
			}

			//
            MiniDisplay.Close(iDevice);
            iDevice = IntPtr.Zero;
			//Broadcast closed event
			OnClosed(this);
        }

        public bool IsOpen()
        {
            return iDevice != IntPtr.Zero;
        }

        public void Clear()
        {
            MiniDisplay.Clear(iDevice);
        }

        public void Fill()
        {
            MiniDisplay.Fill(iDevice);
        }

        public void SwapBuffers()
        {
            MiniDisplay.SwapBuffers(iDevice);
        }

        public int MaxBrightness()
        {
            return MiniDisplay.MaxBrightness(iDevice);
        }

        public int MinBrightness()
        {
            return MiniDisplay.MinBrightness(iDevice);
        }

        public void SetBrightness(int aBrightness)
        {
            if (!IsOpen()) return;

            MiniDisplay.SetBrightness(iDevice, aBrightness);
        }

        public int WidthInPixels()
        {
            return MiniDisplay.WidthInPixels(iDevice);
        }

        public int HeightInPixels()
        {
            return MiniDisplay.HeightInPixels(iDevice);
        }

        public void SetPixel(int aX, int aY, uint aValue)
        {
            MiniDisplay.SetPixel(iDevice,aX,aY,aValue);
        }

        public void RequestPowerSupplyStatus()
        {
            MiniDisplay.SendRequest(iDevice, MiniDisplay.Request.PowerSupplyStatus);
        }

        public void RequestDeviceId()
        {
            MiniDisplay.SendRequest(iDevice, MiniDisplay.Request.DeviceId);
        }

        public void RequestFirmwareRevision()
        {
            MiniDisplay.SendRequest(iDevice, MiniDisplay.Request.FirmwareRevision);
        }

        public void PowerOn()
        {
            MiniDisplay.PowerOn(iDevice);
        }

        public void PowerOff()
        {
            MiniDisplay.PowerOff(iDevice);
        }

        public bool SupportPowerOnOff()
        {
            return MiniDisplay.SupportPowerOnOff(iDevice);
        }

        public void ShowClock()
        {
            MiniDisplay.ShowClock(iDevice);
        }

        public void HideClock()
        {
            MiniDisplay.HideClock(iDevice);
        }

        public bool SupportClock()
        {
            return MiniDisplay.SupportClock(iDevice);
        }

        public bool PowerSupplyStatus()
        {
            bool res = MiniDisplay.PowerSupplyStatus(iDevice);
            return res;
        }

        public MiniDisplay.Request AttemptRequestCompletion()
        {
            return MiniDisplay.AttemptRequestCompletion(iDevice);
        }

        public MiniDisplay.Request CurrentRequest()
        {
            return MiniDisplay.CurrentRequest(iDevice);
        }

        public bool IsRequestPending()
        {
            return CurrentRequest() != MiniDisplay.Request.None;
        }

		//
		public int IconCount(MiniDisplay.IconType aIcon)
		{
			return MiniDisplay.IconCount(iDevice,aIcon);
		}

		public int IconStatusCount(MiniDisplay.IconType aIcon)
		{
			return MiniDisplay.IconStatusCount(iDevice, aIcon);
		}

		public void SetIconStatus(MiniDisplay.IconType aIcon, int aIndex, int aStatus)
		{
			MiniDisplay.SetIconStatus(iDevice, aIcon, aIndex, aStatus);
		}

		public void SetIconOn(MiniDisplay.IconType aIcon, int aIndex)
		{
			MiniDisplay.SetIconStatus(iDevice, aIcon, aIndex, IconStatusCount(aIcon) - 1);
		}

		public void SetIconOff(MiniDisplay.IconType aIcon, int aIndex)
		{
			MiniDisplay.SetIconStatus(iDevice, aIcon, aIndex, 0);
		}


		public void SetAllIconsStatus(int aStatus)
		{
			foreach (MiniDisplay.IconType icon in Enum.GetValues(typeof(MiniDisplay.IconType)))
			{
				int count=IconCount(icon);
				for (int i = 0; i < count; i++)
				{
					SetIconStatus(icon,i,aStatus);
				}
			}
		}

		/// <summary>
		/// Set all elements of an icon to the given status.
		/// </summary>
		/// <param name="aIcon"></param>
		/// <param name="aStatus"></param>
		public void SetIconStatus(MiniDisplay.IconType aIcon, int aStatus)
		{
			int iconCount = IconCount(aIcon);
			for (int i = 0; i < iconCount; i++)
			{
				SetIconStatus(aIcon, i, aStatus);
			}
		}

		/// <summary>
		/// Set all elements of an icon to be either on or off.
		/// </summary>
		/// <param name="aIcon"></param>
		/// <param name="aOn"></param>		
		public void SetIconOnOff(MiniDisplay.IconType aIcon, bool aOn)
		{
			if (aOn)
			{
				SetIconOn(aIcon);
			}
			else
			{
				SetIconOff(aIcon);
			}
		}

		/// <summary>
		/// Set all elements of an icon to there maximum status.
		/// </summary>
		/// <param name="aIcon"></param>
		public void SetIconOn(MiniDisplay.IconType aIcon)
		{
			int iconCount = IconCount(aIcon);
			for (int i = 0; i < iconCount; i++)
			{
				SetIconStatus(aIcon, i, IconStatusCount(aIcon) - 1);
			}
		}

		/// <summary>
		/// Turn off all elements of an icon.
		/// </summary>
		/// <param name="aIcon"></param>
		public void SetIconOff(MiniDisplay.IconType aIcon)
		{
			int iconCount = IconCount(aIcon);
			for (int i = 0; i < iconCount; i++)
			{
				SetIconStatus(aIcon, i, 0);
			}
		}



        public string Vendor()
        {
            IntPtr ptr = MiniDisplay.Vendor(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string Product()
        {
            IntPtr ptr = MiniDisplay.Product(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string SerialNumber()
        {
            IntPtr ptr = MiniDisplay.SerialNumber(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string DeviceId()
        {
            IntPtr ptr = MiniDisplay.DeviceId(iDevice);
            string str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }

        public string FirmwareRevision()
        {
            IntPtr ptr = MiniDisplay.FirmwareRevision(iDevice);
            string str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }


    }
}
