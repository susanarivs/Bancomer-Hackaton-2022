using System.Reflection;
using System.Runtime.CompilerServices;
using WhiteLabel.Core;
using Xamarin.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyTitle("WhiteLabel.Core.Droid")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("${AuthorCopyright}")]
[assembly: AssemblyTrademark("")]
[assembly: ResolutionGroupName("WhiteLabel.Core")]
[assembly: Xamarin.Forms.Dependency(typeof(CultureServiceLocator))]
[assembly: Xamarin.Forms.Dependency(typeof(LayoutDirectionServiceLocator))]
[assembly: ExportEffect(typeof(BackgroundGradientEffect), "BackgroundGradientEffect")]
[assembly: ExportRenderer(typeof(GrialNavigationBar), typeof(GrialNavigationBarRenderer))]
[assembly: ExportEffect(typeof(CornerRadiusEffect), "CornerRadiusEffect")]
[assembly: ExportEffect(typeof(ShadowEffect), "ShadowEffect")]
[assembly: ExportRenderer(typeof(FormsVideoPlayer), typeof(FormsVideoPlayerRenderer))]
[assembly: AssemblyVersion("2.0.0.0")]
