// This is the main DLL file.

#include "stdafx.h"

#include "PowerManager.h"

using namespace System::Diagnostics;

namespace PowerManager
{
    ///
    SettingNotifier::SettingNotifier(IntPtr aHandle, Boolean aService)
    {
        Construct(aHandle, aService);
    }

    ///
    SettingNotifier::SettingNotifier(IntPtr aHandle)
    {
        //By default we assume we run as a Window
        Construct(aHandle, false);
    }

    ///
    void SettingNotifier::Construct(IntPtr aHandle, Boolean aService)
    {
        iHandle = aHandle;
        iIsService = aService;
        iMonitorPowerOnDelegate = nullptr;
        iMonitorPowerOffDelegate = nullptr;
        iMonitorPowerObserverCount = 0;
        iMonitorPowerHandle = NULL;
    }

    ///
    HPOWERNOTIFY SettingNotifier::RegisterPowerSettingNotification(LPCGUID aGuid)
	{
        HANDLE handle = iHandle.ToPointer();
        return ::RegisterPowerSettingNotification(handle, aGuid, (iIsService?DEVICE_NOTIFY_SERVICE_HANDLE:DEVICE_NOTIFY_WINDOW_HANDLE));
	}

    ///
    void SettingNotifier::WndProc(Message% aMessage)
    {
        POWERBROADCAST_SETTING* setting;

        if (aMessage.Msg == WM_POWERBROADCAST && aMessage.WParam.ToInt32() == PBT_POWERSETTINGCHANGE)
        {
            setting=(POWERBROADCAST_SETTING*)aMessage.LParam.ToPointer();
            if (setting->PowerSetting == GUID_MONITOR_POWER_ON)
            {
                if (setting->Data[0] == 0x0)
                {
                    Debug::WriteLine(L"POWERBROADCAST: Monitor Power Off");
                    OnMonitorPowerOff();
                }
                else if (setting->Data[0] == 0x1)
                {
                    Debug::WriteLine(L"POWERBROADCAST: Monitor Power On");
                    OnMonitorPowerOn();
                }
            }
        }
    }

    ///
    void SettingNotifier::OnMonitorPowerOn::add(PowerManagerDelegate^ d)
    {
        iMonitorPowerOnDelegate += d;
        iMonitorPowerObserverCount++;
        //iMonitorPowerOnDelegate->GetInvocationList()->GetLength(0)
        if (iMonitorPowerObserverCount == 1)
        {
            //Register for monitor power notifications
            iMonitorPowerHandle=RegisterPowerSettingNotification(&GUID_MONITOR_POWER_ON);
        }

    }

    ///
    void SettingNotifier::OnMonitorPowerOn::remove(PowerManagerDelegate^ d)
    {
        iMonitorPowerOnDelegate -= d;
        iMonitorPowerObserverCount--;
        if (iMonitorPowerObserverCount==0)
        {
            //Unregister from corresponding power setting notification
            UnregisterPowerSettingNotification(iMonitorPowerHandle);
        }
    }

    //
    void SettingNotifier::OnMonitorPowerOn::raise()
    {        
        if (iMonitorPowerOnDelegate != nullptr)
        {
            iMonitorPowerOnDelegate->Invoke();
        }
    }

    ///
    void SettingNotifier::OnMonitorPowerOff::add(PowerManagerDelegate^ d)
    {
        iMonitorPowerOffDelegate += d;
        iMonitorPowerObserverCount++;
        if (iMonitorPowerObserverCount == 1)
        {
            //Register for monitor power notifications
            iMonitorPowerHandle = RegisterPowerSettingNotification(&GUID_MONITOR_POWER_ON);
        }
    }

    ///
    void SettingNotifier::OnMonitorPowerOff::remove(PowerManagerDelegate^ d)
    {
        iMonitorPowerOffDelegate -= d;
        iMonitorPowerObserverCount--;
        if (iMonitorPowerObserverCount == 0)
        {
            //Unregister from corresponding power setting notification
            UnregisterPowerSettingNotification(iMonitorPowerHandle);
        }
    }

    //
    void SettingNotifier::OnMonitorPowerOff::raise()
    {
        if (iMonitorPowerOffDelegate != nullptr)
        {
            iMonitorPowerOffDelegate->Invoke();
        }        
    }



}