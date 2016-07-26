using SharpLib.Ear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDisplayManager
{
    /// <summary>
    /// Used to populate our action type combobox with friendly names
    /// </summary>
    class ItemActionType
    {
        public Type Type;

        public ItemActionType(Type type)
        {
            this.Type = type;
        }

        public override string ToString()
        {
            //Get friendly action name from action attribute.
            //That we then show up in our combobox
            return SharpLib.Utils.Reflection.GetAttribute<AttributeAction>(Type).Name;
        }
    }
}
