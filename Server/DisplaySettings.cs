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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Drawing;
using SharpLib.Utils;

namespace SharpDisplayManager
{
    /// <summary>
    /// Display settings for the specified hardware type
    /// </summary>
    [DataContract]
    public class DisplaySettings
    {
        public DisplaySettings()
        {
            Brightness = 1;
            DisplayType = 0;
            TimerInterval = 150;
            ReverseScreen = false;
            InverseColors = true;
            ShowBorders = false;
			ShowVolumeLabel = false;
            FontName = "Microsoft Sans Serif, 9.75pt";
            ScaleToFit = true;
            MinFontSize = 15.0f;
            Separator = "   ";
			ScrollingSpeedInPixelsPerSecond = 64;
        }


		[DataMember]
		public bool ShowVolumeLabel { get; set; }

        [DataMember]
        public int Brightness { get; set; }

        /// <summary>
        /// See Display.TMiniDisplayType
        /// </summary>
        [DataMember]
        public int DisplayType { get; set; }

        [DataMember]
        public int TimerInterval { get; set; }

		[DataMember]
		public int ScrollingSpeedInPixelsPerSecond { get; set; }

        [DataMember]
        public bool ReverseScreen { get; set; }

        [DataMember]
        public bool InverseColors { get; set; }

        [DataMember]
        public bool ShowBorders { get; set; }

        [DataMember]
        public bool ScaleToFit { get; set; }

        [DataMember]
        public float MinFontSize { get; set; }

        [DataMember]
        public string Separator { get; set; }

        [DataMember]
        public string FontName { get; set; }

        public Font Font
        {
            get
            {
                FontConverter cvt = new FontConverter();
                Font font = cvt.ConvertFromInvariantString(FontName) as Font;
                return font;
            }

            set
            {
                FontConverter cvt = new FontConverter();
                FontName = cvt.ConvertToInvariantString(value);
            }
        }
    };


    /// <summary>
    /// Contain settings for each of our display type.
    /// </summary>
    [TypeConverter(typeof(TypeConverterJson<DisplaysSettings>))]
    [DataContract]
    public class DisplaysSettings
    {
        private List<DisplaySettings> iDisplays;

        public DisplaysSettings()
        {
            Init();
        }

        public void Init()
        {
            if (iDisplays == null)
            {
                iDisplays = new List<DisplaySettings>();
            }
        }

        //[DataMember]
        //public int CurrentSettingsIndex { get; set; }

        [DataMember]
        public List<DisplaySettings> Displays { get { Init(); return iDisplays; } private set { iDisplays = value; } }


    }
}

