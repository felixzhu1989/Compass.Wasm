using Compass.Wasm.Shared.DataService.Entities;

namespace Compass.DataService.Domain;

public class ModuleDataFactory
{
    private static List<ModuleData> _modules = new ();

    static ModuleDataFactory()
    {
        var baseType = typeof(ModuleData);
        var assembly = typeof(ModuleData).Assembly;
        var moduleTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));//获取所有继承自ModuleData的子类型
        foreach (var moduleType in moduleTypes)
        {
            ModuleData moduleData = (ModuleData)Activator.CreateInstance(moduleType)!;
            _modules.Add(moduleData);
        }
    }

    public static ModuleData? GetModuleData(string model)
    {
        foreach (var moduleData in _modules)
        {
            if (moduleData.Accept(model)) return moduleData;
        }
        return null;
    }
    
}