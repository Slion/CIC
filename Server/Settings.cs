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

namespace SharpDisplayManager.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        public Settings() : base("default") {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
             this.SettingChanging += this.SettingChangingEventHandler;
            //
             this.SettingsSaving += this.SettingsSavingEventHandler;
            //
            this.SettingsLoaded += this.SettingsLoadedEventHandler;
            //
            this.PropertyChanged += this.PropertyChangedEventHandler;
        }

        private void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine($"Settings: property changed {e.PropertyName}");
            Default.Save();
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
            Console.WriteLine($"Settings: changing {e.SettingKey}.{e.SettingName}");
        }

        private void SettingsLoadedEventHandler(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            Console.WriteLine($"Settings: loaded {e.Provider.ApplicationName}");
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
            Console.WriteLine("Settings: saving");
        }
    }
}
