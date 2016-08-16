using SharpLib.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ear = SharpLib.Ear;

namespace SharpDisplayManager
{
    /// <summary>
    /// Allow saving our EAR manager as JSON within our application settings.
    /// </summary>
    [DataContract]
    [TypeConverter(typeof(TypeConverterJson<EarManager>))]
    public class EarManager: Ear.Manager
    {
    }
}
