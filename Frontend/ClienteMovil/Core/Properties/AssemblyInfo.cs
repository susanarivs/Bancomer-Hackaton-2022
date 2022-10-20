using System.Reflection;
using System.Runtime.CompilerServices;
using WhiteLabel.Core;
using Xamarin.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Xamarin.Forms.Dependency(typeof(MirrorService))]
[assembly: AssemblyDescription("Core support library.")]
[assembly: AssemblyCopyright("Bancomer - 2022")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.Droid")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.iOS")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.UWP")]
[assembly: XmlnsDefinition("http://bancomer.com/uikit", "WhiteLabel.Core")]
