using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using SharpLib.Display;

namespace SharpDisplayManager
{
    /// <summary>
    /// A UI thread copy of a client relevant data.
    /// Keeping this copy in the UI thread helps us deal with threading issues.
    /// </summary>
    public class ClientData
    {
        public ClientData(string aSessionId, ICallback aCallback)
        {
            SessionId = aSessionId;
            Name = "";
            Fields = new List<DataField>();
            Layout = new TableLayout(1, 2); //Default to one column and two rows
            Callback = aCallback;
            HasNewLayout = true;            
        }

        public string SessionId { get; set; }
        public string Name { get; set; }
        public List<DataField> Fields { get; set; }
        public TableLayout Layout { get; set; }
        public ICallback Callback { get; set; }

        public bool HasNewLayout { get; set; }

        //Client management
        public DateTime LastSwitchTime { get; set; }

        /// <summary>
        /// Look up the corresponding field in our field collection.
        /// </summary>
        /// <param name="aField"></param>
        /// <returns></returns>
        public DataField FindSameFieldAs(DataField aField)
        {
            foreach (DataField field in Fields)
            {
                if (field.IsSameAs(aField))
                {
                    return field;
                }                
            }

            return null;
        }


        /// <summary>
        /// Look up the corresponding field in our field collection.
        /// </summary>
        /// <param name="aField"></param>
        /// <returns></returns>
        public int FindSameFieldIndex(DataField aField)
        {
            int i = 0;
            foreach (DataField field in Fields)
            {
                if (field.IsSameAs(aField))
                {
                    return i;
                }
                i++;
            }

            return -1;
        }


    }
}
