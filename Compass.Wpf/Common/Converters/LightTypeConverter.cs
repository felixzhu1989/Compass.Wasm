using Compass.Wasm.Shared.DataService;
using System.Windows.Data;

namespace Compass.Wpf.Common.Converters;
[ValueConversion(typeof(LightType_e), typeof(string))]
public class LightTypeConverter : EnumConverter<LightType_e>, IValueConverter
{
}