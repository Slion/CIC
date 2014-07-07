using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;
using System.Text;

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

        //Our display device handle
        IntPtr iDevice;

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


    }
}
