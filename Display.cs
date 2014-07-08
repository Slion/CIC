using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;

namespace SharpDisplayManager
{
    class Display
    {

        //Constructor
        public Display()
        {
            iDevice = IntPtr.Zero;
        }

        //
        public bool Open()
        {
            if (iDevice == IntPtr.Zero)
            {
                iDevice = MiniDisplayOpen();
            }
            return iDevice != IntPtr.Zero;
        }

        public void Close()
        {
            MiniDisplayClose(iDevice);
            iDevice = IntPtr.Zero;
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

        public void SetPixel(int aX, int aY, int aValue)
        {
            MiniDisplaySetPixel(iDevice,aX,aY,aValue);
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

        //Our display device handle
        IntPtr iDevice;

        public enum TMiniDisplayRequest
        {
            EMiniDisplayRequestNone,
            EMiniDisplayRequestDeviceId,
            EMiniDisplayRequestFirmwareRevision,
            EMiniDisplayRequestPowerSupplyStatus
        }

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MiniDisplayOpen();

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayClose(IntPtr aDevice);

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
        public static extern int MiniDisplaySetPixel(IntPtr aDevice, int aX, int aY, int aValue);

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
        public static extern bool MiniDisplayPowerSupplyStatus(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayRequestDeviceId(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayRequestFirmwareRevision(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayRequestPowerSupplyStatus(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern TMiniDisplayRequest MiniDisplayAttemptRequestCompletion(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern TMiniDisplayRequest MiniDisplayCurrentRequest(IntPtr aDevice);

        [DllImport("MiniDisplay.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiniDisplayCancelRequest(IntPtr aDevice);


        
        


    }
}
