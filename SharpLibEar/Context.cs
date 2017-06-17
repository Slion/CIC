
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.Ear
{
    /// <summary>
    /// Execution context
    /// </summary>
    public class Context
    {
        public Dictionary<string, string> Variables = new Dictionary<string, string>();

        /// <summary>
        /// Apply this context to the given string.
        /// Basically does variable substitution.
        /// </summary>
        /// <param name="aSource"></param>
        /// <returns></returns>
        public string Apply(string aSource)
        {
            foreach (KeyValuePair<string, string> kvp in Variables)
            {
                aSource.Replace(kvp.Key, kvp.Value);
            }

            return aSource;
        }
    }
}
