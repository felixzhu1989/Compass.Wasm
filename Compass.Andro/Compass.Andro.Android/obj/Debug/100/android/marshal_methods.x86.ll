; ModuleID = 'obj\Debug\100\android\marshal_methods.x86.ll'
source_filename = "obj\Debug\100\android\marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android"


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
@assembly_image_cache_hashes = local_unnamed_addr constant [250 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 67
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 96
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 14
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 91
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 81
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 81
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 48
	i32 166922606, ; 7: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 32
	i32 172012715, ; 8: FastAndroidCamera.dll => 0xa40b4ab => 8
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 82
	i32 209399409, ; 10: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 46
	i32 219130465, ; 11: Xamarin.Android.Support.v4 => 0xd0faa61 => 37
	i32 220171995, ; 12: System.Diagnostics.Debug => 0xd1f8edb => 118
	i32 230216969, ; 13: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 62
	i32 231814094, ; 14: System.Globalization => 0xdd133ce => 124
	i32 232815796, ; 15: System.Web.Services => 0xde07cb4 => 109
	i32 246610117, ; 16: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 112
	i32 260078871, ; 17: Compass.Dtos => 0xf807d17 => 2
	i32 261689757, ; 18: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 51
	i32 278686392, ; 19: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 66
	i32 280482487, ; 20: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 60
	i32 318968648, ; 21: Xamarin.AndroidX.Activity.dll => 0x13031348 => 38
	i32 321597661, ; 22: System.Numerics => 0x132b30dd => 23
	i32 334355562, ; 23: ZXing.Net.Mobile.Forms.dll => 0x13eddc6a => 99
	i32 342366114, ; 24: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 64
	i32 385762202, ; 25: System.Memory.dll => 0x16fe439a => 22
	i32 389971796, ; 26: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 33
	i32 441335492, ; 27: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 50
	i32 442521989, ; 28: Xamarin.Essentials => 0x1a605985 => 90
	i32 442565967, ; 29: System.Collections => 0x1a61054f => 116
	i32 450948140, ; 30: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 59
	i32 465846621, ; 31: mscorlib => 0x1bc4415d => 13
	i32 469710990, ; 32: System.dll => 0x1bff388e => 21
	i32 476646585, ; 33: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 60
	i32 486930444, ; 34: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 71
	i32 498788369, ; 35: System.ObjectModel => 0x1dbae811 => 119
	i32 501000162, ; 36: Prism.dll => 0x1ddca7e2 => 15
	i32 514659665, ; 37: Xamarin.Android.Support.Compat => 0x1ead1551 => 32
	i32 526420162, ; 38: System.Transactions.dll => 0x1f6088c2 => 103
	i32 545304856, ; 39: System.Runtime.Extensions => 0x2080b118 => 117
	i32 548916678, ; 40: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 11
	i32 583021779, ; 41: Prism.DryIoc.Forms => 0x22c034d3 => 16
	i32 605376203, ; 42: System.IO.Compression.FileSystem => 0x24154ecb => 107
	i32 627609679, ; 43: Xamarin.AndroidX.CustomView => 0x2568904f => 55
	i32 662205335, ; 44: System.Text.Encodings.Web.dll => 0x27787397 => 27
	i32 663517072, ; 45: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 87
	i32 666292255, ; 46: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 43
	i32 690569205, ; 47: System.Xml.Linq.dll => 0x29293ff5 => 30
	i32 692692150, ; 48: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 31
	i32 775507847, ; 49: System.IO.Compression => 0x2e394f87 => 106
	i32 809851609, ; 50: System.Drawing.Common.dll => 0x30455ad9 => 105
	i32 843511501, ; 51: Xamarin.AndroidX.Print => 0x3246f6cd => 78
	i32 877678880, ; 52: System.Globalization.dll => 0x34505120 => 124
	i32 882883187, ; 53: Xamarin.Android.Support.v4.dll => 0x349fba73 => 37
	i32 928116545, ; 54: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 96
	i32 954320159, ; 55: ZXing.Net.Mobile.Forms => 0x38e1c51f => 99
	i32 955402788, ; 56: Newtonsoft.Json => 0x38f24a24 => 14
	i32 958213972, ; 57: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 36
	i32 967690846, ; 58: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 64
	i32 974778368, ; 59: FormsViewGroup.dll => 0x3a19f000 => 9
	i32 992768348, ; 60: System.Collections.dll => 0x3b2c715c => 116
	i32 1012816738, ; 61: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 80
	i32 1035644815, ; 62: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 42
	i32 1042160112, ; 63: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 93
	i32 1052210849, ; 64: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 68
	i32 1080915870, ; 65: Compass.Andro.Android.dll => 0x406d779e => 0
	i32 1098259244, ; 66: System => 0x41761b2c => 21
	i32 1134191450, ; 67: ZXingNetMobile.dll => 0x439a635a => 101
	i32 1175144683, ; 68: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 85
	i32 1178241025, ; 69: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 75
	i32 1189007916, ; 70: Compass.Andro.dll => 0x46ded22c => 5
	i32 1204270330, ; 71: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 43
	i32 1267360935, ; 72: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 86
	i32 1293217323, ; 73: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 57
	i32 1364015309, ; 74: System.IO => 0x514d38cd => 122
	i32 1365406463, ; 75: System.ServiceModel.Internals.dll => 0x516272ff => 110
	i32 1376866003, ; 76: Xamarin.AndroidX.SavedState => 0x52114ed3 => 80
	i32 1395857551, ; 77: Xamarin.AndroidX.Media.dll => 0x5333188f => 72
	i32 1406073936, ; 78: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 52
	i32 1411638395, ; 79: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 25
	i32 1445445088, ; 80: Xamarin.Android.Support.Fragment => 0x5627bde0 => 35
	i32 1457743152, ; 81: System.Runtime.Extensions.dll => 0x56e36530 => 117
	i32 1460219004, ; 82: Xamarin.Forms.Xaml => 0x57092c7c => 94
	i32 1462112819, ; 83: System.IO.Compression.dll => 0x57261233 => 106
	i32 1469204771, ; 84: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 41
	i32 1543031311, ; 85: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 123
	i32 1571005899, ; 86: zxing.portable => 0x5da3a5cb => 100
	i32 1574652163, ; 87: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 34
	i32 1582372066, ; 88: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 56
	i32 1592978981, ; 89: System.Runtime.Serialization.dll => 0x5ef2ee25 => 4
	i32 1622152042, ; 90: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 70
	i32 1624863272, ; 91: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 89
	i32 1636350590, ; 92: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 54
	i32 1639515021, ; 93: System.Net.Http.dll => 0x61b9038d => 3
	i32 1639986890, ; 94: System.Text.RegularExpressions => 0x61c036ca => 123
	i32 1657153582, ; 95: System.Runtime => 0x62c6282e => 26
	i32 1658241508, ; 96: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 83
	i32 1658251792, ; 97: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 95
	i32 1663627514, ; 98: DryIoc => 0x6328f0fa => 6
	i32 1670060433, ; 99: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 51
	i32 1701541528, ; 100: System.Diagnostics.Debug.dll => 0x656b7698 => 118
	i32 1729485958, ; 101: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 47
	i32 1766324549, ; 102: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 82
	i32 1776026572, ; 103: System.Core.dll => 0x69dc03cc => 20
	i32 1788241197, ; 104: Xamarin.AndroidX.Fragment => 0x6a96652d => 59
	i32 1796167890, ; 105: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 11
	i32 1808609942, ; 106: Xamarin.AndroidX.Loader => 0x6bcd3296 => 70
	i32 1813201214, ; 107: Xamarin.Google.Android.Material => 0x6c13413e => 95
	i32 1818569960, ; 108: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 76
	i32 1849271627, ; 109: Prism.Forms.dll => 0x6e39a54b => 17
	i32 1867746548, ; 110: Xamarin.Essentials.dll => 0x6f538cf4 => 90
	i32 1878053835, ; 111: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 94
	i32 1885316902, ; 112: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 44
	i32 1904184254, ; 113: FastAndroidCamera => 0x717f8bbe => 8
	i32 1919157823, ; 114: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 73
	i32 2011961780, ; 115: System.Buffers.dll => 0x77ec19b4 => 19
	i32 2019465201, ; 116: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 68
	i32 2028864565, ; 117: Essential.Interfaces.dll => 0x78ee0435 => 7
	i32 2055257422, ; 118: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 65
	i32 2066202781, ; 119: Prism => 0x7b27c09d => 15
	i32 2079903147, ; 120: System.Runtime.dll => 0x7bf8cdab => 26
	i32 2090596640, ; 121: System.Numerics.Vectors => 0x7c9bf920 => 24
	i32 2097448633, ; 122: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 61
	i32 2126786730, ; 123: Xamarin.Forms.Platform.Android => 0x7ec430aa => 92
	i32 2166116741, ; 124: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 34
	i32 2166956942, ; 125: Compass.Andro.Android => 0x8129238e => 0
	i32 2193016926, ; 126: System.ObjectModel.dll => 0x82b6c85e => 119
	i32 2201231467, ; 127: System.Net.Http => 0x8334206b => 3
	i32 2217644978, ; 128: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 85
	i32 2244775296, ; 129: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 71
	i32 2256548716, ; 130: Xamarin.AndroidX.MultiDex => 0x8680336c => 73
	i32 2261435625, ; 131: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 63
	i32 2279755925, ; 132: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 79
	i32 2315684594, ; 133: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 39
	i32 2329204181, ; 134: zxing.portable.dll => 0x8ad4d5d5 => 100
	i32 2330457430, ; 135: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 33
	i32 2341995103, ; 136: ZXingNetMobile => 0x8b98025f => 101
	i32 2373288475, ; 137: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 35
	i32 2409053734, ; 138: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 77
	i32 2431243866, ; 139: ZXing.Net.Mobile.Core.dll => 0x90e9d65a => 97
	i32 2454642406, ; 140: System.Text.Encoding.dll => 0x924edee6 => 121
	i32 2465532216, ; 141: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 50
	i32 2471841756, ; 142: netstandard.dll => 0x93554fdc => 1
	i32 2475788418, ; 143: Java.Interop.dll => 0x93918882 => 10
	i32 2482213323, ; 144: ZXing.Net.Mobile.Forms.Android => 0x93f391cb => 98
	i32 2501346920, ; 145: System.Data.DataSetExtensions => 0x95178668 => 104
	i32 2505896520, ; 146: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 67
	i32 2538310050, ; 147: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 112
	i32 2570120770, ; 148: System.Text.Encodings.Web => 0x9930ee42 => 27
	i32 2581819634, ; 149: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 86
	i32 2620871830, ; 150: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 54
	i32 2624644809, ; 151: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 58
	i32 2633051222, ; 152: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 66
	i32 2693849962, ; 153: System.IO.dll => 0xa090e36a => 122
	i32 2701096212, ; 154: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 83
	i32 2715334215, ; 155: System.Threading.Tasks.dll => 0xa1d8b647 => 115
	i32 2732626843, ; 156: Xamarin.AndroidX.Activity => 0xa2e0939b => 38
	i32 2737747696, ; 157: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 41
	i32 2766581644, ; 158: Xamarin.Forms.Core => 0xa4e6af8c => 91
	i32 2778768386, ; 159: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 88
	i32 2791384523, ; 160: Compass.Andro => 0xa66125cb => 5
	i32 2792073544, ; 161: Prism.DryIoc.Forms.dll => 0xa66ba948 => 16
	i32 2810250172, ; 162: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 52
	i32 2819470561, ; 163: System.Xml.dll => 0xa80db4e1 => 29
	i32 2853208004, ; 164: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 88
	i32 2855708567, ; 165: Xamarin.AndroidX.Transition => 0xaa36a797 => 84
	i32 2903344695, ; 166: System.ComponentModel.Composition => 0xad0d8637 => 108
	i32 2905242038, ; 167: mscorlib.dll => 0xad2a79b6 => 13
	i32 2916838712, ; 168: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 89
	i32 2919462931, ; 169: System.Numerics.Vectors.dll => 0xae037813 => 24
	i32 2921128767, ; 170: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 40
	i32 2978675010, ; 171: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 57
	i32 3024354802, ; 172: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 62
	i32 3044182254, ; 173: FormsViewGroup => 0xb57288ee => 9
	i32 3057625584, ; 174: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 74
	i32 3075834255, ; 175: System.Threading.Tasks => 0xb755818f => 115
	i32 3092211740, ; 176: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 36
	i32 3111772706, ; 177: System.Runtime.Serialization => 0xb979e222 => 4
	i32 3124832203, ; 178: System.Threading.Tasks.Extensions => 0xba4127cb => 113
	i32 3204380047, ; 179: System.Data.dll => 0xbefef58f => 102
	i32 3211777861, ; 180: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 56
	i32 3220365878, ; 181: System.Threading => 0xbff2e236 => 120
	i32 3247949154, ; 182: Mono.Security => 0xc197c562 => 114
	i32 3249260365, ; 183: RestSharp.dll => 0xc1abc74d => 18
	i32 3258312781, ; 184: Xamarin.AndroidX.CardView => 0xc235e84d => 47
	i32 3265893370, ; 185: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 113
	i32 3267021929, ; 186: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 45
	i32 3299363146, ; 187: System.Text.Encoding => 0xc4a8494a => 121
	i32 3317135071, ; 188: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 55
	i32 3317144872, ; 189: System.Data => 0xc5b79d28 => 102
	i32 3340431453, ; 190: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 44
	i32 3346324047, ; 191: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 75
	i32 3353484488, ; 192: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 61
	i32 3358260929, ; 193: System.Text.Json => 0xc82afec1 => 28
	i32 3362522851, ; 194: Xamarin.AndroidX.Core => 0xc86c06e3 => 53
	i32 3366347497, ; 195: Java.Interop => 0xc8a662e9 => 10
	i32 3374999561, ; 196: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 79
	i32 3395150330, ; 197: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 25
	i32 3404865022, ; 198: System.ServiceModel.Internals => 0xcaf21dfe => 110
	i32 3429136800, ; 199: System.Xml => 0xcc6479a0 => 29
	i32 3430777524, ; 200: netstandard => 0xcc7d82b4 => 1
	i32 3439690031, ; 201: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 31
	i32 3441283291, ; 202: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 58
	i32 3476120550, ; 203: Mono.Android => 0xcf3163e6 => 12
	i32 3485117614, ; 204: System.Text.Json.dll => 0xcfbaacae => 28
	i32 3486566296, ; 205: System.Transactions => 0xcfd0c798 => 103
	i32 3493954962, ; 206: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 49
	i32 3501239056, ; 207: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 45
	i32 3509114376, ; 208: System.Xml.Linq => 0xd128d608 => 30
	i32 3536029504, ; 209: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 92
	i32 3567349600, ; 210: System.ComponentModel.Composition.dll => 0xd4a16f60 => 108
	i32 3618140916, ; 211: Xamarin.AndroidX.Preference => 0xd7a872f4 => 77
	i32 3627220390, ; 212: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 78
	i32 3632359727, ; 213: Xamarin.Forms.Platform => 0xd881692f => 93
	i32 3633644679, ; 214: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 40
	i32 3641597786, ; 215: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 65
	i32 3672681054, ; 216: Mono.Android.dll => 0xdae8aa5e => 12
	i32 3676310014, ; 217: System.Web.Services.dll => 0xdb2009fe => 109
	i32 3682565725, ; 218: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 46
	i32 3684561358, ; 219: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 49
	i32 3689375977, ; 220: System.Drawing.Common => 0xdbe768e9 => 105
	i32 3718780102, ; 221: Xamarin.AndroidX.Annotation => 0xdda814c6 => 39
	i32 3724971120, ; 222: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 74
	i32 3758932259, ; 223: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 63
	i32 3786282454, ; 224: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 48
	i32 3816437471, ; 225: RestSharp => 0xe37a36df => 18
	i32 3822602673, ; 226: Xamarin.AndroidX.Media => 0xe3d849b1 => 72
	i32 3829621856, ; 227: System.Numerics.dll => 0xe4436460 => 23
	i32 3847036339, ; 228: ZXing.Net.Mobile.Forms.Android.dll => 0xe54d1db3 => 98
	i32 3868628318, ; 229: Compass.Dtos.dll => 0xe696955e => 2
	i32 3885922214, ; 230: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 84
	i32 3896760992, ; 231: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 53
	i32 3920810846, ; 232: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 107
	i32 3921031405, ; 233: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 87
	i32 3931092270, ; 234: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 76
	i32 3945713374, ; 235: System.Data.DataSetExtensions.dll => 0xeb2ecede => 104
	i32 3955647286, ; 236: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 42
	i32 4025784931, ; 237: System.Memory => 0xeff49a63 => 22
	i32 4054681211, ; 238: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 111
	i32 4073602200, ; 239: System.Threading.dll => 0xf2ce3c98 => 120
	i32 4085261167, ; 240: Prism.Forms => 0xf380236f => 17
	i32 4105002889, ; 241: Mono.Security.dll => 0xf4ad5f89 => 114
	i32 4147896353, ; 242: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 111
	i32 4151237749, ; 243: System.Core => 0xf76edc75 => 20
	i32 4165582995, ; 244: DryIoc.dll => 0xf849c093 => 6
	i32 4182413190, ; 245: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 69
	i32 4186595366, ; 246: ZXing.Net.Mobile.Core => 0xf98a6026 => 97
	i32 4260525087, ; 247: System.Buffers => 0xfdf2741f => 19
	i32 4292120959, ; 248: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 69
	i32 4293553716 ; 249: Essential.Interfaces => 0xffea6e34 => 7
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [250 x i32] [
	i32 67, i32 96, i32 14, i32 91, i32 81, i32 81, i32 48, i32 32, ; 0..7
	i32 8, i32 82, i32 46, i32 37, i32 118, i32 62, i32 124, i32 109, ; 8..15
	i32 112, i32 2, i32 51, i32 66, i32 60, i32 38, i32 23, i32 99, ; 16..23
	i32 64, i32 22, i32 33, i32 50, i32 90, i32 116, i32 59, i32 13, ; 24..31
	i32 21, i32 60, i32 71, i32 119, i32 15, i32 32, i32 103, i32 117, ; 32..39
	i32 11, i32 16, i32 107, i32 55, i32 27, i32 87, i32 43, i32 30, ; 40..47
	i32 31, i32 106, i32 105, i32 78, i32 124, i32 37, i32 96, i32 99, ; 48..55
	i32 14, i32 36, i32 64, i32 9, i32 116, i32 80, i32 42, i32 93, ; 56..63
	i32 68, i32 0, i32 21, i32 101, i32 85, i32 75, i32 5, i32 43, ; 64..71
	i32 86, i32 57, i32 122, i32 110, i32 80, i32 72, i32 52, i32 25, ; 72..79
	i32 35, i32 117, i32 94, i32 106, i32 41, i32 123, i32 100, i32 34, ; 80..87
	i32 56, i32 4, i32 70, i32 89, i32 54, i32 3, i32 123, i32 26, ; 88..95
	i32 83, i32 95, i32 6, i32 51, i32 118, i32 47, i32 82, i32 20, ; 96..103
	i32 59, i32 11, i32 70, i32 95, i32 76, i32 17, i32 90, i32 94, ; 104..111
	i32 44, i32 8, i32 73, i32 19, i32 68, i32 7, i32 65, i32 15, ; 112..119
	i32 26, i32 24, i32 61, i32 92, i32 34, i32 0, i32 119, i32 3, ; 120..127
	i32 85, i32 71, i32 73, i32 63, i32 79, i32 39, i32 100, i32 33, ; 128..135
	i32 101, i32 35, i32 77, i32 97, i32 121, i32 50, i32 1, i32 10, ; 136..143
	i32 98, i32 104, i32 67, i32 112, i32 27, i32 86, i32 54, i32 58, ; 144..151
	i32 66, i32 122, i32 83, i32 115, i32 38, i32 41, i32 91, i32 88, ; 152..159
	i32 5, i32 16, i32 52, i32 29, i32 88, i32 84, i32 108, i32 13, ; 160..167
	i32 89, i32 24, i32 40, i32 57, i32 62, i32 9, i32 74, i32 115, ; 168..175
	i32 36, i32 4, i32 113, i32 102, i32 56, i32 120, i32 114, i32 18, ; 176..183
	i32 47, i32 113, i32 45, i32 121, i32 55, i32 102, i32 44, i32 75, ; 184..191
	i32 61, i32 28, i32 53, i32 10, i32 79, i32 25, i32 110, i32 29, ; 192..199
	i32 1, i32 31, i32 58, i32 12, i32 28, i32 103, i32 49, i32 45, ; 200..207
	i32 30, i32 92, i32 108, i32 77, i32 78, i32 93, i32 40, i32 65, ; 208..215
	i32 12, i32 109, i32 46, i32 49, i32 105, i32 39, i32 74, i32 63, ; 216..223
	i32 48, i32 18, i32 72, i32 23, i32 98, i32 2, i32 84, i32 53, ; 224..231
	i32 107, i32 87, i32 76, i32 104, i32 42, i32 22, i32 111, i32 120, ; 232..239
	i32 17, i32 114, i32 111, i32 20, i32 6, i32 69, i32 97, i32 19, ; 240..247
	i32 69, i32 7 ; 248..249
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
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


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"NumRegisterParameters", i32 0}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ a200af12c1e846626b8caebd926ac14c185f71ec"}
!llvm.linker.options = !{}
