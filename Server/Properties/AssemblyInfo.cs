using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CIC")]
[assembly: AssemblyDescription("Command & Information Center")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Slions")]
[assembly: AssemblyProduct("Command & Information Center")]
[assembly: AssemblyCopyright("Copyright © 2014-2017, Stéphane Lenclud")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5da0f26b-76a6-41e8-832c-5b593b3a75b0")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
// Increasing this will cause our Visual Studio build server to publish a new Squirrel release
[assembly: AssemblyFileVersion("2.3.0")]
[assembly: NeutralResourcesLanguageAttribute("en")]

// To avoid default Squirrel behaviour such as create shortcuts for every EXE we need to be Squirrel Aware
[assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]
