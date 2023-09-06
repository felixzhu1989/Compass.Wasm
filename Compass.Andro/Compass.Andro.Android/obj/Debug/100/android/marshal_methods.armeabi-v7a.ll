; ModuleID = 'obj\Debug\100\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Debug\100\android\marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [230 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 57
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 86
	i32 57263871, ; 2: Xamarin.Forms.Core.dll => 0x369c6ff => 81
	i32 101534019, ; 3: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 71
	i32 120558881, ; 4: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 71
	i32 165246403, ; 5: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 38
	i32 166922606, ; 6: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 22
	i32 172012715, ; 7: FastAndroidCamera.dll => 0xa40b4ab => 6
	i32 182336117, ; 8: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 72
	i32 209399409, ; 9: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 36
	i32 219130465, ; 10: Xamarin.Android.Support.v4 => 0xd0faa61 => 27
	i32 220171995, ; 11: System.Diagnostics.Debug => 0xd1f8edb => 108
	i32 230216969, ; 12: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 52
	i32 231814094, ; 13: System.Globalization => 0xdd133ce => 114
	i32 232815796, ; 14: System.Web.Services => 0xde07cb4 => 101
	i32 246610117, ; 15: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 103
	i32 261689757, ; 16: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 41
	i32 278686392, ; 17: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 56
	i32 280482487, ; 18: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 50
	i32 318968648, ; 19: Xamarin.AndroidX.Activity.dll => 0x13031348 => 28
	i32 321597661, ; 20: System.Numerics => 0x132b30dd => 16
	i32 334355562, ; 21: ZXing.Net.Mobile.Forms.dll => 0x13eddc6a => 89
	i32 342366114, ; 22: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 54
	i32 389971796, ; 23: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 23
	i32 441335492, ; 24: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 40
	i32 442521989, ; 25: Xamarin.Essentials => 0x1a605985 => 80
	i32 442565967, ; 26: System.Collections => 0x1a61054f => 106
	i32 450948140, ; 27: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 49
	i32 465846621, ; 28: mscorlib => 0x1bc4415d => 10
	i32 469710990, ; 29: System.dll => 0x1bff388e => 15
	i32 476646585, ; 30: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 50
	i32 486930444, ; 31: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 61
	i32 498788369, ; 32: System.ObjectModel => 0x1dbae811 => 109
	i32 501000162, ; 33: Prism.dll => 0x1ddca7e2 => 11
	i32 514659665, ; 34: Xamarin.Android.Support.Compat => 0x1ead1551 => 22
	i32 526420162, ; 35: System.Transactions.dll => 0x1f6088c2 => 95
	i32 545304856, ; 36: System.Runtime.Extensions => 0x2080b118 => 107
	i32 583021779, ; 37: Prism.DryIoc.Forms => 0x22c034d3 => 12
	i32 605376203, ; 38: System.IO.Compression.FileSystem => 0x24154ecb => 99
	i32 627609679, ; 39: Xamarin.AndroidX.CustomView => 0x2568904f => 45
	i32 663517072, ; 40: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 77
	i32 666292255, ; 41: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 33
	i32 690569205, ; 42: System.Xml.Linq.dll => 0x29293ff5 => 20
	i32 692692150, ; 43: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 21
	i32 775507847, ; 44: System.IO.Compression => 0x2e394f87 => 98
	i32 809851609, ; 45: System.Drawing.Common.dll => 0x30455ad9 => 97
	i32 843511501, ; 46: Xamarin.AndroidX.Print => 0x3246f6cd => 68
	i32 877678880, ; 47: System.Globalization.dll => 0x34505120 => 114
	i32 882883187, ; 48: Xamarin.Android.Support.v4.dll => 0x349fba73 => 27
	i32 928116545, ; 49: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 86
	i32 954320159, ; 50: ZXing.Net.Mobile.Forms => 0x38e1c51f => 89
	i32 958213972, ; 51: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 26
	i32 967690846, ; 52: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 54
	i32 974778368, ; 53: FormsViewGroup.dll => 0x3a19f000 => 7
	i32 992768348, ; 54: System.Collections.dll => 0x3b2c715c => 106
	i32 1012816738, ; 55: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 70
	i32 1035644815, ; 56: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 32
	i32 1042160112, ; 57: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 83
	i32 1052210849, ; 58: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 58
	i32 1080915870, ; 59: Compass.Andro.Android.dll => 0x406d779e => 0
	i32 1098259244, ; 60: System => 0x41761b2c => 15
	i32 1134191450, ; 61: ZXingNetMobile.dll => 0x439a635a => 91
	i32 1175144683, ; 62: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 75
	i32 1178241025, ; 63: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 65
	i32 1189007916, ; 64: Compass.Andro.dll => 0x46ded22c => 3
	i32 1204270330, ; 65: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 33
	i32 1267360935, ; 66: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 76
	i32 1293217323, ; 67: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 47
	i32 1364015309, ; 68: System.IO => 0x514d38cd => 112
	i32 1365406463, ; 69: System.ServiceModel.Internals.dll => 0x516272ff => 92
	i32 1376866003, ; 70: Xamarin.AndroidX.SavedState => 0x52114ed3 => 70
	i32 1395857551, ; 71: Xamarin.AndroidX.Media.dll => 0x5333188f => 62
	i32 1406073936, ; 72: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 42
	i32 1445445088, ; 73: Xamarin.Android.Support.Fragment => 0x5627bde0 => 25
	i32 1457743152, ; 74: System.Runtime.Extensions.dll => 0x56e36530 => 107
	i32 1460219004, ; 75: Xamarin.Forms.Xaml => 0x57092c7c => 84
	i32 1462112819, ; 76: System.IO.Compression.dll => 0x57261233 => 98
	i32 1469204771, ; 77: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 31
	i32 1543031311, ; 78: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 113
	i32 1571005899, ; 79: zxing.portable => 0x5da3a5cb => 90
	i32 1574652163, ; 80: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 24
	i32 1582372066, ; 81: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 46
	i32 1592978981, ; 82: System.Runtime.Serialization.dll => 0x5ef2ee25 => 2
	i32 1622152042, ; 83: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 60
	i32 1624863272, ; 84: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 79
	i32 1636350590, ; 85: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 44
	i32 1639515021, ; 86: System.Net.Http.dll => 0x61b9038d => 1
	i32 1639986890, ; 87: System.Text.RegularExpressions => 0x61c036ca => 113
	i32 1657153582, ; 88: System.Runtime => 0x62c6282e => 18
	i32 1658241508, ; 89: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 73
	i32 1658251792, ; 90: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 85
	i32 1663627514, ; 91: DryIoc => 0x6328f0fa => 4
	i32 1670060433, ; 92: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 41
	i32 1701541528, ; 93: System.Diagnostics.Debug.dll => 0x656b7698 => 108
	i32 1729485958, ; 94: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 37
	i32 1766324549, ; 95: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 72
	i32 1776026572, ; 96: System.Core.dll => 0x69dc03cc => 14
	i32 1788241197, ; 97: Xamarin.AndroidX.Fragment => 0x6a96652d => 49
	i32 1808609942, ; 98: Xamarin.AndroidX.Loader => 0x6bcd3296 => 60
	i32 1813201214, ; 99: Xamarin.Google.Android.Material => 0x6c13413e => 85
	i32 1818569960, ; 100: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 66
	i32 1849271627, ; 101: Prism.Forms.dll => 0x6e39a54b => 13
	i32 1867746548, ; 102: Xamarin.Essentials.dll => 0x6f538cf4 => 80
	i32 1878053835, ; 103: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 84
	i32 1885316902, ; 104: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 34
	i32 1904184254, ; 105: FastAndroidCamera => 0x717f8bbe => 6
	i32 1919157823, ; 106: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 63
	i32 2019465201, ; 107: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 58
	i32 2028864565, ; 108: Essential.Interfaces.dll => 0x78ee0435 => 5
	i32 2055257422, ; 109: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 55
	i32 2066202781, ; 110: Prism => 0x7b27c09d => 11
	i32 2079903147, ; 111: System.Runtime.dll => 0x7bf8cdab => 18
	i32 2090596640, ; 112: System.Numerics.Vectors => 0x7c9bf920 => 17
	i32 2097448633, ; 113: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 51
	i32 2126786730, ; 114: Xamarin.Forms.Platform.Android => 0x7ec430aa => 82
	i32 2166116741, ; 115: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 24
	i32 2166956942, ; 116: Compass.Andro.Android => 0x8129238e => 0
	i32 2193016926, ; 117: System.ObjectModel.dll => 0x82b6c85e => 109
	i32 2201231467, ; 118: System.Net.Http => 0x8334206b => 1
	i32 2217644978, ; 119: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 75
	i32 2244775296, ; 120: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 61
	i32 2256548716, ; 121: Xamarin.AndroidX.MultiDex => 0x8680336c => 63
	i32 2261435625, ; 122: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 53
	i32 2279755925, ; 123: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 69
	i32 2315684594, ; 124: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 29
	i32 2329204181, ; 125: zxing.portable.dll => 0x8ad4d5d5 => 90
	i32 2330457430, ; 126: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 23
	i32 2341995103, ; 127: ZXingNetMobile => 0x8b98025f => 91
	i32 2373288475, ; 128: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 25
	i32 2409053734, ; 129: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 67
	i32 2431243866, ; 130: ZXing.Net.Mobile.Core.dll => 0x90e9d65a => 87
	i32 2454642406, ; 131: System.Text.Encoding.dll => 0x924edee6 => 111
	i32 2465532216, ; 132: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 40
	i32 2471841756, ; 133: netstandard.dll => 0x93554fdc => 93
	i32 2475788418, ; 134: Java.Interop.dll => 0x93918882 => 8
	i32 2482213323, ; 135: ZXing.Net.Mobile.Forms.Android => 0x93f391cb => 88
	i32 2501346920, ; 136: System.Data.DataSetExtensions => 0x95178668 => 96
	i32 2505896520, ; 137: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 57
	i32 2538310050, ; 138: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 103
	i32 2581819634, ; 139: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 76
	i32 2620871830, ; 140: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 44
	i32 2624644809, ; 141: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 48
	i32 2633051222, ; 142: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 56
	i32 2693849962, ; 143: System.IO.dll => 0xa090e36a => 112
	i32 2701096212, ; 144: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 73
	i32 2715334215, ; 145: System.Threading.Tasks.dll => 0xa1d8b647 => 105
	i32 2732626843, ; 146: Xamarin.AndroidX.Activity => 0xa2e0939b => 28
	i32 2737747696, ; 147: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 31
	i32 2766581644, ; 148: Xamarin.Forms.Core => 0xa4e6af8c => 81
	i32 2778768386, ; 149: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 78
	i32 2791384523, ; 150: Compass.Andro => 0xa66125cb => 3
	i32 2792073544, ; 151: Prism.DryIoc.Forms.dll => 0xa66ba948 => 12
	i32 2810250172, ; 152: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 42
	i32 2819470561, ; 153: System.Xml.dll => 0xa80db4e1 => 19
	i32 2853208004, ; 154: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 78
	i32 2855708567, ; 155: Xamarin.AndroidX.Transition => 0xaa36a797 => 74
	i32 2903344695, ; 156: System.ComponentModel.Composition => 0xad0d8637 => 100
	i32 2905242038, ; 157: mscorlib.dll => 0xad2a79b6 => 10
	i32 2916838712, ; 158: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 79
	i32 2919462931, ; 159: System.Numerics.Vectors.dll => 0xae037813 => 17
	i32 2921128767, ; 160: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 30
	i32 2978675010, ; 161: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 47
	i32 3024354802, ; 162: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 52
	i32 3044182254, ; 163: FormsViewGroup => 0xb57288ee => 7
	i32 3057625584, ; 164: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 64
	i32 3075834255, ; 165: System.Threading.Tasks => 0xb755818f => 105
	i32 3092211740, ; 166: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 26
	i32 3111772706, ; 167: System.Runtime.Serialization => 0xb979e222 => 2
	i32 3204380047, ; 168: System.Data.dll => 0xbefef58f => 94
	i32 3211777861, ; 169: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 46
	i32 3220365878, ; 170: System.Threading => 0xbff2e236 => 110
	i32 3247949154, ; 171: Mono.Security => 0xc197c562 => 104
	i32 3258312781, ; 172: Xamarin.AndroidX.CardView => 0xc235e84d => 37
	i32 3267021929, ; 173: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 35
	i32 3299363146, ; 174: System.Text.Encoding => 0xc4a8494a => 111
	i32 3317135071, ; 175: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 45
	i32 3317144872, ; 176: System.Data => 0xc5b79d28 => 94
	i32 3340431453, ; 177: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 34
	i32 3346324047, ; 178: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 65
	i32 3353484488, ; 179: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 51
	i32 3362522851, ; 180: Xamarin.AndroidX.Core => 0xc86c06e3 => 43
	i32 3366347497, ; 181: Java.Interop => 0xc8a662e9 => 8
	i32 3374999561, ; 182: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 69
	i32 3404865022, ; 183: System.ServiceModel.Internals => 0xcaf21dfe => 92
	i32 3429136800, ; 184: System.Xml => 0xcc6479a0 => 19
	i32 3430777524, ; 185: netstandard => 0xcc7d82b4 => 93
	i32 3439690031, ; 186: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 21
	i32 3441283291, ; 187: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 48
	i32 3476120550, ; 188: Mono.Android => 0xcf3163e6 => 9
	i32 3486566296, ; 189: System.Transactions => 0xcfd0c798 => 95
	i32 3493954962, ; 190: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 39
	i32 3501239056, ; 191: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 35
	i32 3509114376, ; 192: System.Xml.Linq => 0xd128d608 => 20
	i32 3536029504, ; 193: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 82
	i32 3567349600, ; 194: System.ComponentModel.Composition.dll => 0xd4a16f60 => 100
	i32 3618140916, ; 195: Xamarin.AndroidX.Preference => 0xd7a872f4 => 67
	i32 3627220390, ; 196: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 68
	i32 3632359727, ; 197: Xamarin.Forms.Platform => 0xd881692f => 83
	i32 3633644679, ; 198: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 30
	i32 3641597786, ; 199: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 55
	i32 3672681054, ; 200: Mono.Android.dll => 0xdae8aa5e => 9
	i32 3676310014, ; 201: System.Web.Services.dll => 0xdb2009fe => 101
	i32 3682565725, ; 202: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 36
	i32 3684561358, ; 203: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 39
	i32 3689375977, ; 204: System.Drawing.Common => 0xdbe768e9 => 97
	i32 3718780102, ; 205: Xamarin.AndroidX.Annotation => 0xdda814c6 => 29
	i32 3724971120, ; 206: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 64
	i32 3758932259, ; 207: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 53
	i32 3786282454, ; 208: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 38
	i32 3822602673, ; 209: Xamarin.AndroidX.Media => 0xe3d849b1 => 62
	i32 3829621856, ; 210: System.Numerics.dll => 0xe4436460 => 16
	i32 3847036339, ; 211: ZXing.Net.Mobile.Forms.Android.dll => 0xe54d1db3 => 88
	i32 3885922214, ; 212: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 74
	i32 3896760992, ; 213: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 43
	i32 3920810846, ; 214: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 99
	i32 3921031405, ; 215: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 77
	i32 3931092270, ; 216: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 66
	i32 3945713374, ; 217: System.Data.DataSetExtensions.dll => 0xeb2ecede => 96
	i32 3955647286, ; 218: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 32
	i32 4054681211, ; 219: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 102
	i32 4073602200, ; 220: System.Threading.dll => 0xf2ce3c98 => 110
	i32 4085261167, ; 221: Prism.Forms => 0xf380236f => 13
	i32 4105002889, ; 222: Mono.Security.dll => 0xf4ad5f89 => 104
	i32 4147896353, ; 223: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 102
	i32 4151237749, ; 224: System.Core => 0xf76edc75 => 14
	i32 4165582995, ; 225: DryIoc.dll => 0xf849c093 => 4
	i32 4182413190, ; 226: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 59
	i32 4186595366, ; 227: ZXing.Net.Mobile.Core => 0xf98a6026 => 87
	i32 4292120959, ; 228: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 59
	i32 4293553716 ; 229: Essential.Interfaces => 0xffea6e34 => 5
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [230 x i32] [
	i32 57, i32 86, i32 81, i32 71, i32 71, i32 38, i32 22, i32 6, ; 0..7
	i32 72, i32 36, i32 27, i32 108, i32 52, i32 114, i32 101, i32 103, ; 8..15
	i32 41, i32 56, i32 50, i32 28, i32 16, i32 89, i32 54, i32 23, ; 16..23
	i32 40, i32 80, i32 106, i32 49, i32 10, i32 15, i32 50, i32 61, ; 24..31
	i32 109, i32 11, i32 22, i32 95, i32 107, i32 12, i32 99, i32 45, ; 32..39
	i32 77, i32 33, i32 20, i32 21, i32 98, i32 97, i32 68, i32 114, ; 40..47
	i32 27, i32 86, i32 89, i32 26, i32 54, i32 7, i32 106, i32 70, ; 48..55
	i32 32, i32 83, i32 58, i32 0, i32 15, i32 91, i32 75, i32 65, ; 56..63
	i32 3, i32 33, i32 76, i32 47, i32 112, i32 92, i32 70, i32 62, ; 64..71
	i32 42, i32 25, i32 107, i32 84, i32 98, i32 31, i32 113, i32 90, ; 72..79
	i32 24, i32 46, i32 2, i32 60, i32 79, i32 44, i32 1, i32 113, ; 80..87
	i32 18, i32 73, i32 85, i32 4, i32 41, i32 108, i32 37, i32 72, ; 88..95
	i32 14, i32 49, i32 60, i32 85, i32 66, i32 13, i32 80, i32 84, ; 96..103
	i32 34, i32 6, i32 63, i32 58, i32 5, i32 55, i32 11, i32 18, ; 104..111
	i32 17, i32 51, i32 82, i32 24, i32 0, i32 109, i32 1, i32 75, ; 112..119
	i32 61, i32 63, i32 53, i32 69, i32 29, i32 90, i32 23, i32 91, ; 120..127
	i32 25, i32 67, i32 87, i32 111, i32 40, i32 93, i32 8, i32 88, ; 128..135
	i32 96, i32 57, i32 103, i32 76, i32 44, i32 48, i32 56, i32 112, ; 136..143
	i32 73, i32 105, i32 28, i32 31, i32 81, i32 78, i32 3, i32 12, ; 144..151
	i32 42, i32 19, i32 78, i32 74, i32 100, i32 10, i32 79, i32 17, ; 152..159
	i32 30, i32 47, i32 52, i32 7, i32 64, i32 105, i32 26, i32 2, ; 160..167
	i32 94, i32 46, i32 110, i32 104, i32 37, i32 35, i32 111, i32 45, ; 168..175
	i32 94, i32 34, i32 65, i32 51, i32 43, i32 8, i32 69, i32 92, ; 176..183
	i32 19, i32 93, i32 21, i32 48, i32 9, i32 95, i32 39, i32 35, ; 184..191
	i32 20, i32 82, i32 100, i32 67, i32 68, i32 83, i32 30, i32 55, ; 192..199
	i32 9, i32 101, i32 36, i32 39, i32 97, i32 29, i32 64, i32 53, ; 200..207
	i32 38, i32 62, i32 16, i32 88, i32 74, i32 43, i32 99, i32 77, ; 208..215
	i32 66, i32 96, i32 32, i32 102, i32 110, i32 13, i32 104, i32 102, ; 216..223
	i32 14, i32 4, i32 59, i32 87, i32 59, i32 5 ; 224..229
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ a200af12c1e846626b8caebd926ac14c185f71ec"}
!llvm.linker.options = !{}
