using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharpLib.Win32;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SharpLib.Ear
{
    [DataContract]
    [AttributeObject(Id = "Action.OpticalDrive.Eject", Name = "Eject", Description = "Eject media from an optical drive.")]
    public class ActionOpticalDriveEject : Action
    {
        [DataMember]
        [AttributeObjectProperty
            (
            Id = "Action.OpticalDrive.Eject.Drive",
            Name = "Drive to eject",
            Description = "Select the drive you want to eject."
            )
        ]
        public PropertyComboBox Drive { get; set; } = new PropertyComboBox();


        protected override void DoConstruct()
        {
            base.DoConstruct();
            PopulateOpticalDrives();
            CheckCurrentItem();
        }


        public override string Brief()
        {
            return Name + " " + Drive.CurrentItem ;
        }

        public override bool IsValid()
        {   
            //This object is valid if our current item is contained in our drive list
            return Drive.Items.Contains(Drive.CurrentItem);
        }

        protected override void DoExecute()
        {
            DriveEject(Drive.CurrentItem);
        }


        private void CheckCurrentItem()
        {
            if (!Drive.Items.Contains(Drive.CurrentItem) && Drive.Items.Count>0)
            {
                //Current item unknown, reset it then
                Drive.CurrentItem = Drive.Items[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateOpticalDrives()
        {
            //Reset our list of drives
            Drive.Items = new List<string>();
            //Go through each drives on our system and collected the optical ones in our list
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                Debug.WriteLine("Drive " + d.Name);
                Debug.WriteLine("  Drive type: {0}", d.DriveType);

                if (d.DriveType == DriveType.CDRom)
                {
                    //This is an optical drive, add it now
                    Drive.Items.Add(d.Name.Substring(0, 2));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPrefix"></param>
        static private void CheckLastError(string aPrefix)
        {
            string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
            Debug.WriteLine(aPrefix + Marshal.GetLastWin32Error().ToString() + ": " + errorMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static private IntPtr MarshalToPointer(object data)
        {
            IntPtr buf = Marshal.AllocHGlobal(
                Marshal.SizeOf(data));
            Marshal.StructureToPtr(data,
                buf, false);
            return buf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static private SafeFileHandle OpenVolume(string aDriveName)
        {
            return Function.CreateFile("\\\\.\\" + aDriveName,
                               SharpLib.Win32.FileAccess.GENERIC_READ,
                               SharpLib.Win32.FileShare.FILE_SHARE_READ | SharpLib.Win32.FileShare.FILE_SHARE_WRITE,
                               IntPtr.Zero,
                               CreationDisposition.OPEN_EXISTING,
                               0,
                               IntPtr.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aVolume"></param>
        /// <returns></returns>
        static private bool LockVolume(SafeFileHandle aVolume)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();

            int tries = 0;
            const int KMaxTries = 100;
            const int KSleepTime = 10;
            bool success = false;

            while (!success && tries < KMaxTries)
            {
                success = Function.DeviceIoControl(aVolume, Const.FSCTL_LOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, dwBytesReturned, ref overlapped);
                System.Threading.Thread.Sleep(KSleepTime);
                tries++;
            }

            CheckLastError("Lock volume: ");

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aVolume"></param>
        /// <returns></returns>
        static private bool DismountVolume(SafeFileHandle aVolume)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();

            bool res = Function.DeviceIoControl(aVolume, Const.FSCTL_DISMOUNT_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, dwBytesReturned, ref overlapped);
            CheckLastError("Dismount volume: ");
            return res;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="aVolume"></param>
        /// <param name="aPreventRemoval"></param>
        /// <returns></returns>
        static private bool PreventRemovalOfVolume(SafeFileHandle aVolume, bool aPreventRemoval)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();
            //
            PREVENT_MEDIA_REMOVAL preventMediaRemoval = new PREVENT_MEDIA_REMOVAL();
            preventMediaRemoval.PreventMediaRemoval = Convert.ToByte(aPreventRemoval);
            IntPtr preventMediaRemovalParam = MarshalToPointer(preventMediaRemoval);

            bool result = Function.DeviceIoControl(aVolume, Const.IOCTL_STORAGE_MEDIA_REMOVAL, preventMediaRemovalParam, Convert.ToUInt32(Marshal.SizeOf(preventMediaRemoval)), IntPtr.Zero, 0, dwBytesReturned, ref overlapped);
            CheckLastError("Media removal: ");
            Marshal.FreeHGlobal(preventMediaRemovalParam);

            return result;
        }

        /// <summary>
        /// Eject optical drive media opening the tray if any.
        /// </summary>
        /// <param name="aVolume"></param>
        /// <returns></returns>
        static private bool MediaEject(SafeFileHandle aVolume)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();

            bool res = Function.DeviceIoControl(aVolume, Const.IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, dwBytesReturned, ref overlapped);
            CheckLastError("Media eject: ");
            return res;
        }

        /// <summary>
        /// Close an optical drive tray.
        /// </summary>
        /// <param name="aVolume"></param>
        /// <returns></returns>
        static private bool MediaLoad(SafeFileHandle aVolume)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();

            bool res = Function.DeviceIoControl(aVolume, Const.IOCTL_STORAGE_LOAD_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, dwBytesReturned, ref overlapped);
            CheckLastError("Media load: ");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aVolume"></param>
        /// <returns></returns>
        static private bool StorageCheckVerify(SafeFileHandle aVolume)
        {
            //Hope that's doing what I think it does
            IntPtr dwBytesReturned = new IntPtr();
            //Should not be needed but I'm not sure how to pass NULL in there.
            OVERLAPPED overlapped = new OVERLAPPED();

            bool res = Function.DeviceIoControl(aVolume, Const.IOCTL_STORAGE_CHECK_VERIFY2, IntPtr.Zero, 0, IntPtr.Zero, 0, dwBytesReturned, ref overlapped);

            CheckLastError("Check verify: ");

            return res;
        }


        /// <summary>
        /// Perform media ejection.
        /// </summary>
        static private void DriveEject(string aDrive)
        {
            string drive = aDrive;
            if (drive.Length != 2)
            {
                //Not a proper drive spec.
                //Probably 'None' selected.
                return;
            }

            SafeFileHandle handle = OpenVolume(drive);
            if (handle.IsInvalid)
            {
                CheckLastError("ERROR: Failed to open volume: ");
                return;
            }

            if (LockVolume(handle) && DismountVolume(handle))
            {
                Debug.WriteLine("Volume was dismounted.");

                if (PreventRemovalOfVolume(handle, false))
                {
                    //StorageCheckVerify(handle);

                    DateTime before;
                    before = DateTime.Now;
                    bool ejectSuccess = MediaEject(handle);
                    double ms = (DateTime.Now - before).TotalMilliseconds;

                    //We assume that if it take more than a certain time to for eject to execute it means we actually ejected.
                    //If our eject completes too rapidly we assume the tray is already open and we will try to close it. 
                    if (ejectSuccess && ms > 100)
                    {
                        Debug.WriteLine("Media was ejected");
                    }
                    else if (MediaLoad(handle))
                    {
                        Debug.WriteLine("Media was loaded");
                    }
                }
            }
            else
            {
                Debug.WriteLine("Volume lock or dismount failed.");
            }

            //This is needed to make sure we can open the volume next time around
            handle.Dispose();
        }

    }
}
