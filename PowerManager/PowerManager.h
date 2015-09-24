// PowerManager.h

#pragma once

using namespace System;
using namespace System::Windows::Forms;

namespace PowerManager
{
    public delegate void PowerManagerDelegate();

	public ref class SettingNotifier
	{
    public:
        //Constructors
        SettingNotifier(IntPtr aHandle, Boolean aService);
        SettingNotifier(IntPtr aHandle);
        //
        void WndProc(Message% aMessage);

        event PowerManagerDelegate^ OnMonitorPowerOn
        {
            void add(PowerManagerDelegate^ d);
            void remove(PowerManagerDelegate^ d);
        private:
            void raise();
        }

        event PowerManagerDelegate^ OnMonitorPowerOff
        {
            void add(PowerManagerDelegate^ d);
            void remove(PowerManagerDelegate^ d);
        private:
            void raise();
        }

    private:
        void Construct(IntPtr aHandle, Boolean aService);
        //
        HPOWERNOTIFY RegisterPowerSettingNotification(LPCGUID aGuid);

    private:
        PowerManagerDelegate^ iMonitorPowerOnDelegate;
        PowerManagerDelegate^ iMonitorPowerOffDelegate;


    private:
        /// Window or Service handle
        IntPtr iHandle;
        /// Specify whether we run as Window or a Service
        Boolean iIsService;
        ///
        int iMonitorPowerObserverCount;
        HPOWERNOTIFY iMonitorPowerHandle;
	};
}
