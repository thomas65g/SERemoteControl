using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SERemoteControl")]
[assembly: AssemblyDescription("SevenExcellence Remote Control\r\nSevenExcellence™ is the modular benchtop meter for precise analysis of pH, conductivity, dissolved oxygen and ions, and stands for unmatched compliance and efficiency. Automate your pH analysis with innovative remote control functionalities.\r\nSevenExcellence was built on Lance Platform")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Mettler Toledo Gmbh")]
[assembly: AssemblyProduct("SERemoteControl")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("cc4759d9-332e-4d51-a4ec-bd004486aaf9")]

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
[assembly: AssemblyVersion("0.9.0.0")]
[assembly: AssemblyFileVersion("0.9.0.0")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]