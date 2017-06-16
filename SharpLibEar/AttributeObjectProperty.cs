using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// To expose an action property thus enabling user to edit it.
    /// TODO: Fix our types?
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AttributeObjectProperty : System.Attribute
    {
        public string Id;
        public string Name;
        public string Description;
        // For numerics
        public double Minimum;
        public double Maximum;
        public double Increment;
        public int DecimalPlaces = 0;
        // For file dialog
        public string Filter;
    }




}
