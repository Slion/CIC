// This is the main DLL file.

#include "stdafx.h"

#include "PowerManager.h"

using namespace System::Diagnostics;

namespace PowerManager
{
    ///
    PowerSettingNotifier::PowerSettingNotifier(IntPtr aHandle, Boolean aService)
    {
        Construct(aHandle, aService);
    }

    ///
    PowerSettingNotifier::PowerSettingNotifier(IntPtr aHandle)
    {
        //By default we assume we run as a Window
        Construct(aHandle, false);
    }

    ///
    void PowerSettingNotifier::Construct(IntPtr aHandle, Boolean aService)
    {
        iHandle = aHandle;
        iIsService = aService;
        iMonitorPowerOnDelegate = nullptr;
        iMonitorPowerOffDelegate = nullptr;
        iMonitorPowerObserverCount = 0;
    }

    ///
    Boolean PowerSettingNotifier::RegisterPowerSettingNotification(IntPtr aHandle, Boolean aService)
	{
        HANDLE handle = aHandle.ToPointer();        
        HPOWERNOTIFY res=::RegisterPowerSettingNotification(handle, &GUID_MONITOR_POWER_ON, (aService?DEVICE_NOTIFY_SERVICE_HANDLE:DEVICE_NOTIFY_WINDOW_HANDLE));
        return (res != NULL);
	};

    /// 
    Boolean PowerSettingNotifier::RegisterPowerSettingNotification(IntPtr aHandle)
    {
        return RegisterPowerSettingNotification(aHandle,false);
    };

    ///
    void PowerSettingNotifier::WndProc(Message% aMessage)
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
    void PowerSettingNotifier::OnMonitorPowerOn::add(PowerManagerDelegate^ d)
    {
        iMonitorPowerOnDelegate += d;
        iMonitorPowerObserverCount++;
        //iMonitorPowerOnDelegate->GetInvocationList()->GetLength(0)
        if (iMonitorPowerObserverCount == 1)
        {
            //TODO: register
            RegisterPowerSettingNotification(iHandle,iIsService);
        }

    }

    ///
    void PowerSettingNotifier::OnMonitorPowerOn::remove(PowerManagerDelegate^ d)
    {
        iMonitorPowerOnDelegate -= d;
        iMonitorPowerObserverCount--;
        if (iMonitorPowerObserverCount==0)
        {
            //TODO: unregister
        }
    }

    //
    void PowerSettingNotifier::OnMonitorPowerOn::raise()
    {        
        if (iMonitorPowerOnDelegate != nullptr)
        {
            iMonitorPowerOnDelegate->Invoke();
        }
    }

    ///
    void PowerSettingNotifier::OnMonitorPowerOff::add(PowerManagerDelegate^ d)
    {
        iMonitorPowerOffDelegate += d;
        iMonitorPowerObserverCount++;
        if (iMonitorPowerObserverCount == 1)
        {
            //TODO: register
            RegisterPowerSettingNotification(iHandle, iIsService);
        }
    }

    ///
    void PowerSettingNotifier::OnMonitorPowerOff::remove(PowerManagerDelegate^ d)
    {
        iMonitorPowerOffDelegate -= d;
        iMonitorPowerObserverCount--;
        if (iMonitorPowerObserverCount == 0)
        {
            //TODO: unregister
        }
    }

    //
    void PowerSettingNotifier::OnMonitorPowerOff::raise()
    {
        if (iMonitorPowerOffDelegate != nullptr)
        {
            iMonitorPowerOffDelegate->Invoke();
        }        
    }



}