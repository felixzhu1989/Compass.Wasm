﻿using Compass.Wasm.Shared.Data.Ceilings;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;
public interface ICeilingService
{
	#region 排风腔

    void KcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix,string module, KcjData data);





    #endregion








}