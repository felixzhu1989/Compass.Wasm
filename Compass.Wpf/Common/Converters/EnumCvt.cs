using Compass.Wasm.Shared.Data;
using System.Globalization;
using System.Windows.Data;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wpf.Common.Converters;

/// <summary>
/// WPF中，枚举与字符串的相互转换
/// </summary>
/// <typeparam name="T"></typeparam>
public class EnumCvt<T> : IValueConverter where T : Enum
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (T)Enum.Parse(typeof(T), value.ToString());
    }
}

public class ProductCvt : EnumCvt<Product_e> { }
public class UnitCvt : EnumCvt<Unit_e>{}

public class SidePanelCvt : EnumCvt<SidePanel_e> { }
public class LightTypeCvt : EnumCvt<LightType_e> { }

public class DrainTypeCvt : EnumCvt<DrainType_e> { }
public class WaterInletCvt : EnumCvt<WaterInlet_e> { }

public class AnsulSideCvt : EnumCvt<AnsulSide_e> { }
public class AnsulDetectorCvt : EnumCvt<AnsulDetector_e> { }
public class AnsulDetectorEndCvt : EnumCvt<AnsulDetectorEnd_e> { }

public class UvLightTypeCvt : EnumCvt<UvLightType_e> { }


public class FilterTypeCvt:EnumCvt<FilterType_e>{}
public class FilterSideCvt : EnumCvt<FilterSide_e> {}

public class CeilingLightTypeCvt : EnumCvt<CeilingLightType_e> { }
public class LightCableCvt : EnumCvt<LightCable_e> { }
public class HclSideCvt : EnumCvt<HclSide_e> { }

public class DpSideCvt : EnumCvt<DpSide_e> { }
public class CeilingWaterInletCvt : EnumCvt<CeilingWaterInlet_e> { }