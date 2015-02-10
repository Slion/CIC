using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;
//using System.Runtime.Serialization;

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
			return MiniDisplayTypeCount();
		}

		public static string TypeName(TMiniDisplayType aType)
		{
			IntPtr ptr = MiniDisplayTypeName(aType);
			string str = Marshal.PtrToStringUni(ptr);
			return str;
		}

        //Constructor
        public Display()
        {
            iDevice = IntPtr.Zero;
        }

        //
        public bool Open(TMiniDisplayType aType)
        {
			if (IsOpen())
			{
				//Already open return an error
				return false;
			}

            iDevice = MiniDisplayOpen(aType);

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
            MiniDisplayClose(iDevice);
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
            MiniDisplayClear(iDevice);
        }

        public void Fill()
        {
            MiniDisplayFill(iDevice);
        }

        public void SwapBuffers()
        {
            MiniDisplaySwapBuffers(iDevice);
        }

        public int MaxBrightness()
        {
            return MiniDisplayMaxBrightness(iDevice);
        }

        public int MinBrightness()
        {
            return MiniDisplayMinBrightness(iDevice);
        }

        public void SetBrightness(int aBrightness)
        {
            if (!IsOpen()) return;

            MiniDisplaySetBrightness(iDevice, aBrightness);
        }

        public int WidthInPixels()
        {
            return MiniDisplayWidthInPixels(iDevice);
        }

        public int HeightInPixels()
        {
            return MiniDisplayHeightInPixels(iDevice);
        }

        public void SetPixel(int aX, int aY, uint aValue)
        {
            MiniDisplaySetPixel(iDevice,aX,aY,aValue);
        }

        public void RequestPowerSupplyStatus()
        {
            MiniDisplayRequest(iDevice, TMiniDisplayRequest.EMiniDisplayRequestPowerSupplyStatus);
        }

        public void RequestDeviceId()
        {
            MiniDisplayRequest(iDevice, TMiniDisplayRequest.EMiniDisplayRequestDeviceId);
        }

        public void RequestFirmwareRevision()
        {
            MiniDisplayRequest(iDevice, TMiniDisplayRequest.EMiniDisplayRequestFirmwareRevision);
        }

        public void PowerOn()
        {
            MiniDisplayPowerOn(iDevice);
        }

        public void PowerOff()
        {
            MiniDisplayPowerOff(iDevice);
        }

        public bool SupportPowerOnOff()
        {
            return MiniDisplaySupportPowerOnOff(iDevice);
        }

        public void ShowClock()
        {
            MiniDisplayShowClock(iDevice);
        }

        public void HideClock()
        {
            MiniDisplayHideClock(iDevice);
        }

        public bool SupportClock()
        {
            return MiniDisplaySupportClock(iDevice);
        }

        public bool PowerSupplyStatus()
        {
            bool res = MiniDisplayPowerSupplyStatus(iDevice);
            return res;
        }

        public TMiniDisplayRequest AttemptRequestCompletion()
        {
            return MiniDisplayAttemptRequestCompletion(iDevice);
        }

        public TMiniDisplayRequest CurrentRequest()
        {
            return MiniDisplayCurrentRequest(iDevice);
        }

        public bool IsRequestPending()
        {
            return CurrentRequest() != TMiniDisplayRequest.EMiniDisplayRequestNone;
        }

		//
		public int IconCount(TMiniDisplayIconType aIcon)
		{
			return MiniDisplayIconCount(iDevice,aIcon);
		}

		public int IconStatusCount(TMiniDisplayIconType aIcon)
		{
			return MiniDisplayIconStatusCount(iDevice, aIcon);
		}

		public void SetIconStatus(TMiniDisplayIconType aIcon, int aIndex, int aStatus)
		{
			MiniDisplaySetIconStatus(iDevice, aIcon, aIndex, aStatus);
		}

		public void SetIconOn(TMiniDisplayIconType aIcon, int aIndex)
		{
			MiniDisplaySetIconStatus(iDevice, aIcon, aIndex, IconStatusCount(aIcon) - 1);
		}

		public void SetIconOff(TMiniDisplayIconType aIcon, int aIndex)
		{
			MiniDisplaySetIconStatus(iDevice, aIcon, aIndex, 0);
		}


		public void SetAllIconsStatus(int aStatus)
		{
			foreach (TMiniDisplayIconType icon in Enum.GetValues(typeof(TMiniDisplayIconType)))
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
		public void SetIconStatus(TMiniDisplayIconType aIcon, int aStatus)
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
		public void SetIconOnOff(TMiniDisplayIconType aIcon, bool aOn)
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
		public void SetIconOn(TMiniDisplayIconType aIcon)
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
		public void SetIconOff(TMiniDisplayIconType aIcon)
		{
			int iconCount = IconCount(aIcon);
			for (int i = 0; i < iconCount; i++)
			{
				SetIconStatus(aIcon, i, 0);
			}
		}



        public string Vendor()
        {
            IntPtr ptr = MiniDisplayVendor(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string Product()
        {
            IntPtr ptr = MiniDisplayProduct(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string SerialNumber()
        {
            IntPtr ptr = MiniDisplaySerialNumber(iDevice);
            string str = Marshal.PtrToStringUni(ptr);
            return str;
        }

        public string DeviceId()
        {
            IntPtr ptr = MiniDisplayDeviceId(iDevice);
            string str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }

        public string FirmwareRevision()
        {
            IntPtr ptr = MiniDisplayFirmwareRevision(iDevice);
            string str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }

        //[Serializable]
        public enum TMiniDisplayType
        {
            EMiniDisplayAutoDetect, /*Not yet implemented*/
            //[EnumMember(Value = "EMiniDisplayFutabaGP1212A01")]
            EMiniDisplayFutabaGP1212A01,
            //[EnumMember(Value = "EMiniDisplayFutabaGP1212A01")]
            EMiniDisplayFutabaGP1212A02
        };

		/// <summary>
		/// 
		/// </summary>
        public enum TMiniDisplayRequest
        {
            EMiniDisplayRequestNone,
            EMiniDisplayRequestDeviceId,
            EMiniDisplayRequestFirmwareRevision,
            EMiniDisplayRequestPowerSupplyStatus
        };

			
		/// <summary>
		/// Define the various type of icons we support.
		/// For binary compatibility new entries must be added at the end.
		/// </summary>
		public enum TMiniDisplayIconType
		{
			EMiniDisplayIconNetworkSignal=0,
			EMiniDisplayIconInternet,
			EMiniDisplayIconEmail,
			EMiniDisplayIconMute,
			EMiniDisplayIconVolume,
			EMiniDisplayIconVolumeLabel,
			EMiniDisplayIconPlay,
			EMiniDisplayIconPause,
			EMiniDisplayIconRecording
		};

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayOpen(TMiniDisplayType aType);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayClose(IntPtr aDevice);

		[DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern int MiniDisplayTypeCount();

		[DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr MiniDisplayTypeName(TMiniDisplayType aType);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayClear(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayFill(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplaySwapBuffers(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplaySetBrightness(IntPtr aDevice, int aBrightness);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiniDisplayMinBrightness(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiniDisplayMaxBrightness(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiniDisplayWidthInPixels(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiniDisplayHeightInPixels(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiniDisplaySetPixel(IntPtr aDevice, int aX, int aY, uint aValue);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayVendor(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayProduct(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplaySerialNumber(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayDeviceId(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayFirmwareRevision(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool MiniDisplayPowerSupplyStatus(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayRequest(IntPtr aDevice, TMiniDisplayRequest aRequest);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern TMiniDisplayRequest MiniDisplayAttemptRequestCompletion(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern TMiniDisplayRequest MiniDisplayCurrentRequest(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayCancelRequest(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayPowerOn(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayPowerOff(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool MiniDisplaySupportPowerOnOff(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayShowClock(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayHideClock(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool MiniDisplaySupportClock(IntPtr aDevice);

		[DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern int MiniDisplayIconCount(IntPtr aDevice, TMiniDisplayIconType aIcon);

		[DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern int MiniDisplayIconStatusCount(IntPtr aDevice, TMiniDisplayIconType aIcon);
		
		[DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
		public static extern void MiniDisplaySetIconStatus(IntPtr aDevice, TMiniDisplayIconType aIcon, int aIndex, int aStatus);

    }
}
