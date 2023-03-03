using System.Windows.Data;
using Compass.Wasm.Shared.DataService;

namespace Compass.Wpf.Common.Converters;

[ValueConversion(typeof(SidePanel_e), typeof(string))]
public class SidePanelConverter :EnumConverter<SidePanel_e>, IValueConverter
{
}