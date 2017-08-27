//------------------------------------------------------------------------------
//------------------------------------------------------------------------------

namespace SharpDisplayManager.Properties
{
    /// <summary>
    /// Putting here non-generated part of our settings
    /// </summary>
    internal partial class Settings
    {
        /// <summary>
        /// Allow access to plain text FRITZ!Box password
        /// </summary>
        /// <returns></returns>
        public string DecryptFritzBoxPassword()
        {
            return Secure.ToInsecureString(Secure.DecryptString(FritzBoxPassword));
        }
    }
}
