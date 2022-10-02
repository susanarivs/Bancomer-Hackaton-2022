using System.Reflection;
using Xamarin.Forms;
using WhiteLabel;
using WhiteLabel.Core;
using Android.App;

[assembly: AssemblyTitle(AssemblyGlobal.ProductLine + " - " + "Kit (Android)")]
[assembly: AssemblyConfiguration(AssemblyGlobal.Configuration)]
[assembly: AssemblyCompany(AssemblyGlobal.Company)]
[assembly: AssemblyProduct(AssemblyGlobal.ProductLine + " - " + "Kit (Android)")]
[assembly: AssemblyCopyright(AssemblyGlobal.Copyright)]

[assembly: GrialVersion("3.0.60.0")]

[assembly: ExportRenderer(typeof(Entry), typeof(EntryRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
[assembly: ExportRenderer(typeof(Switch), typeof(SwitchRenderer))]
[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(SwitchCell), typeof(SwitchCellRenderer))]
[assembly: ExportRenderer(typeof(TextCell), typeof(TextCellRenderer))]
[assembly: ExportRenderer(typeof(ImageCell), typeof(ImageCellRenderer))]
[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer))]
[assembly: ExportRenderer(typeof(EntryCell), typeof(EntryCellRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
[assembly: ExportRenderer(typeof(ExtendedCarouselViewControl), typeof(WhiteLabel.Droid.ExtendedCarouselViewRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(GrialNavigationPageRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]
[assembly: ExportRenderer(typeof(ScrollView), typeof(WhiteLabel.Droid.ScrollViewRendererOrientationFix))]
//[assembly: ExportRenderer(typeof(Slider), typeof(WhiteLabel.Core.SliderRenderer))]

[assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
// Needed for Picking photo/video
[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]

// Needed for Taking photo/video
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.Camera)]
[assembly: UsesFeature("android.hardware.camera", Required = true)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = true)]