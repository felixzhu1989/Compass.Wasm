; ModuleID = 'obj\Release\100\android\marshal_methods.x86.ll'
source_filename = "obj\Release\100\android\marshal_methods.x86.ll"
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
@assembly_image_cache_hashes = local_unnamed_addr constant [138 x i32] [
	i32 34715100, ; 0: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 56
	i32 39109920, ; 1: Newtonsoft.Json.dll => 0x254c520 => 65
	i32 57263871, ; 2: Xamarin.Forms.Core.dll => 0x369c6ff => 52
	i32 166922606, ; 3: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 13
	i32 172012715, ; 4: FastAndroidCamera.dll => 0xa40b4ab => 44
	i32 182336117, ; 5: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 33
	i32 219130465, ; 6: Xamarin.Android.Support.v4 => 0xd0faa61 => 18
	i32 232815796, ; 7: System.Web.Services => 0xde07cb4 => 38
	i32 318968648, ; 8: Xamarin.AndroidX.Activity.dll => 0x13031348 => 49
	i32 321597661, ; 9: System.Numerics => 0x132b30dd => 8
	i32 334355562, ; 10: ZXing.Net.Mobile.Forms.dll => 0x13eddc6a => 59
	i32 342366114, ; 11: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 28
	i32 389971796, ; 12: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 14
	i32 442521989, ; 13: Xamarin.Essentials => 0x1a605985 => 51
	i32 450948140, ; 14: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 26
	i32 465846621, ; 15: mscorlib => 0x1bc4415d => 4
	i32 469710990, ; 16: System.dll => 0x1bff388e => 7
	i32 501000162, ; 17: Prism.dll => 0x1ddca7e2 => 46
	i32 514659665, ; 18: Xamarin.Android.Support.Compat => 0x1ead1551 => 13
	i32 548916678, ; 19: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 64
	i32 583021779, ; 20: Prism.DryIoc.Forms => 0x22c034d3 => 47
	i32 627609679, ; 21: Xamarin.AndroidX.CustomView => 0x2568904f => 24
	i32 662205335, ; 22: System.Text.Encodings.Web.dll => 0x27787397 => 67
	i32 690569205, ; 23: System.Xml.Linq.dll => 0x29293ff5 => 11
	i32 692692150, ; 24: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 12
	i32 809851609, ; 25: System.Drawing.Common.dll => 0x30455ad9 => 37
	i32 882883187, ; 26: Xamarin.Android.Support.v4.dll => 0x349fba73 => 18
	i32 928116545, ; 27: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 56
	i32 954320159, ; 28: ZXing.Net.Mobile.Forms => 0x38e1c51f => 59
	i32 955402788, ; 29: Newtonsoft.Json => 0x38f24a24 => 65
	i32 958213972, ; 30: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 17
	i32 967690846, ; 31: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 28
	i32 974778368, ; 32: FormsViewGroup.dll => 0x3a19f000 => 45
	i32 1012816738, ; 33: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 50
	i32 1035644815, ; 34: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 20
	i32 1042160112, ; 35: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 54
	i32 1052210849, ; 36: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 30
	i32 1080915870, ; 37: Compass.Andro.Android.dll => 0x406d779e => 62
	i32 1098259244, ; 38: System => 0x41761b2c => 7
	i32 1134191450, ; 39: ZXingNetMobile.dll => 0x439a635a => 61
	i32 1189007916, ; 40: Compass.Andro.dll => 0x46ded22c => 63
	i32 1293217323, ; 41: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 25
	i32 1365406463, ; 42: System.ServiceModel.Internals.dll => 0x516272ff => 39
	i32 1376866003, ; 43: Xamarin.AndroidX.SavedState => 0x52114ed3 => 50
	i32 1406073936, ; 44: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 22
	i32 1411638395, ; 45: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 9
	i32 1445445088, ; 46: Xamarin.Android.Support.Fragment => 0x5627bde0 => 16
	i32 1460219004, ; 47: Xamarin.Forms.Xaml => 0x57092c7c => 55
	i32 1469204771, ; 48: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 19
	i32 1571005899, ; 49: zxing.portable => 0x5da3a5cb => 60
	i32 1574652163, ; 50: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 15
	i32 1592978981, ; 51: System.Runtime.Serialization.dll => 0x5ef2ee25 => 1
	i32 1622152042, ; 52: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 31
	i32 1639515021, ; 53: System.Net.Http.dll => 0x61b9038d => 0
	i32 1658251792, ; 54: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 35
	i32 1663627514, ; 55: DryIoc => 0x6328f0fa => 42
	i32 1729485958, ; 56: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 21
	i32 1766324549, ; 57: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 33
	i32 1776026572, ; 58: System.Core.dll => 0x69dc03cc => 6
	i32 1788241197, ; 59: Xamarin.AndroidX.Fragment => 0x6a96652d => 26
	i32 1796167890, ; 60: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 64
	i32 1808609942, ; 61: Xamarin.AndroidX.Loader => 0x6bcd3296 => 31
	i32 1813201214, ; 62: Xamarin.Google.Android.Material => 0x6c13413e => 35
	i32 1849271627, ; 63: Prism.Forms.dll => 0x6e39a54b => 48
	i32 1867746548, ; 64: Xamarin.Essentials.dll => 0x6f538cf4 => 51
	i32 1878053835, ; 65: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 55
	i32 1904184254, ; 66: FastAndroidCamera => 0x717f8bbe => 44
	i32 2011961780, ; 67: System.Buffers.dll => 0x77ec19b4 => 5
	i32 2019465201, ; 68: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 30
	i32 2028864565, ; 69: Essential.Interfaces.dll => 0x78ee0435 => 43
	i32 2055257422, ; 70: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 29
	i32 2066202781, ; 71: Prism => 0x7b27c09d => 46
	i32 2097448633, ; 72: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 27
	i32 2126786730, ; 73: Xamarin.Forms.Platform.Android => 0x7ec430aa => 53
	i32 2166116741, ; 74: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 15
	i32 2166956942, ; 75: Compass.Andro.Android => 0x8129238e => 62
	i32 2201231467, ; 76: System.Net.Http => 0x8334206b => 0
	i32 2279755925, ; 77: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 32
	i32 2329204181, ; 78: zxing.portable.dll => 0x8ad4d5d5 => 60
	i32 2330457430, ; 79: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 14
	i32 2341995103, ; 80: ZXingNetMobile => 0x8b98025f => 61
	i32 2373288475, ; 81: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 16
	i32 2431243866, ; 82: ZXing.Net.Mobile.Core.dll => 0x90e9d65a => 57
	i32 2475788418, ; 83: Java.Interop.dll => 0x93918882 => 2
	i32 2482213323, ; 84: ZXing.Net.Mobile.Forms.Android => 0x93f391cb => 58
	i32 2570120770, ; 85: System.Text.Encodings.Web => 0x9930ee42 => 67
	i32 2732626843, ; 86: Xamarin.AndroidX.Activity => 0xa2e0939b => 49
	i32 2737747696, ; 87: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 19
	i32 2766581644, ; 88: Xamarin.Forms.Core => 0xa4e6af8c => 52
	i32 2778768386, ; 89: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 34
	i32 2791384523, ; 90: Compass.Andro => 0xa66125cb => 63
	i32 2792073544, ; 91: Prism.DryIoc.Forms.dll => 0xa66ba948 => 47
	i32 2810250172, ; 92: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 22
	i32 2819470561, ; 93: System.Xml.dll => 0xa80db4e1 => 10
	i32 2853208004, ; 94: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 34
	i32 2905242038, ; 95: mscorlib.dll => 0xad2a79b6 => 4
	i32 2978675010, ; 96: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 25
	i32 3044182254, ; 97: FormsViewGroup => 0xb57288ee => 45
	i32 3092211740, ; 98: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 17
	i32 3111772706, ; 99: System.Runtime.Serialization => 0xb979e222 => 1
	i32 3124832203, ; 100: System.Threading.Tasks.Extensions => 0xba4127cb => 40
	i32 3204380047, ; 101: System.Data.dll => 0xbefef58f => 36
	i32 3247949154, ; 102: Mono.Security => 0xc197c562 => 41
	i32 3249260365, ; 103: RestSharp.dll => 0xc1abc74d => 66
	i32 3258312781, ; 104: Xamarin.AndroidX.CardView => 0xc235e84d => 21
	i32 3265893370, ; 105: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 40
	i32 3317135071, ; 106: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 24
	i32 3317144872, ; 107: System.Data => 0xc5b79d28 => 36
	i32 3353484488, ; 108: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 27
	i32 3358260929, ; 109: System.Text.Json => 0xc82afec1 => 68
	i32 3362522851, ; 110: Xamarin.AndroidX.Core => 0xc86c06e3 => 23
	i32 3366347497, ; 111: Java.Interop => 0xc8a662e9 => 2
	i32 3374999561, ; 112: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 32
	i32 3395150330, ; 113: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 9
	i32 3404865022, ; 114: System.ServiceModel.Internals => 0xcaf21dfe => 39
	i32 3429136800, ; 115: System.Xml => 0xcc6479a0 => 10
	i32 3439690031, ; 116: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 12
	i32 3476120550, ; 117: Mono.Android => 0xcf3163e6 => 3
	i32 3485117614, ; 118: System.Text.Json.dll => 0xcfbaacae => 68
	i32 3509114376, ; 119: System.Xml.Linq => 0xd128d608 => 11
	i32 3536029504, ; 120: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 53
	i32 3632359727, ; 121: Xamarin.Forms.Platform => 0xd881692f => 54
	i32 3641597786, ; 122: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 29
	i32 3672681054, ; 123: Mono.Android.dll => 0xdae8aa5e => 3
	i32 3676310014, ; 124: System.Web.Services.dll => 0xdb2009fe => 38
	i32 3689375977, ; 125: System.Drawing.Common => 0xdbe768e9 => 37
	i32 3816437471, ; 126: RestSharp => 0xe37a36df => 66
	i32 3829621856, ; 127: System.Numerics.dll => 0xe4436460 => 8
	i32 3847036339, ; 128: ZXing.Net.Mobile.Forms.Android.dll => 0xe54d1db3 => 58
	i32 3896760992, ; 129: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 23
	i32 3955647286, ; 130: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 20
	i32 4085261167, ; 131: Prism.Forms => 0xf380236f => 48
	i32 4105002889, ; 132: Mono.Security.dll => 0xf4ad5f89 => 41
	i32 4151237749, ; 133: System.Core => 0xf76edc75 => 6
	i32 4165582995, ; 134: DryIoc.dll => 0xf849c093 => 42
	i32 4186595366, ; 135: ZXing.Net.Mobile.Core => 0xf98a6026 => 57
	i32 4260525087, ; 136: System.Buffers => 0xfdf2741f => 5
	i32 4293553716 ; 137: Essential.Interfaces => 0xffea6e34 => 43
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [138 x i32] [
	i32 56, i32 65, i32 52, i32 13, i32 44, i32 33, i32 18, i32 38, ; 0..7
	i32 49, i32 8, i32 59, i32 28, i32 14, i32 51, i32 26, i32 4, ; 8..15
	i32 7, i32 46, i32 13, i32 64, i32 47, i32 24, i32 67, i32 11, ; 16..23
	i32 12, i32 37, i32 18, i32 56, i32 59, i32 65, i32 17, i32 28, ; 24..31
	i32 45, i32 50, i32 20, i32 54, i32 30, i32 62, i32 7, i32 61, ; 32..39
	i32 63, i32 25, i32 39, i32 50, i32 22, i32 9, i32 16, i32 55, ; 40..47
	i32 19, i32 60, i32 15, i32 1, i32 31, i32 0, i32 35, i32 42, ; 48..55
	i32 21, i32 33, i32 6, i32 26, i32 64, i32 31, i32 35, i32 48, ; 56..63
	i32 51, i32 55, i32 44, i32 5, i32 30, i32 43, i32 29, i32 46, ; 64..71
	i32 27, i32 53, i32 15, i32 62, i32 0, i32 32, i32 60, i32 14, ; 72..79
	i32 61, i32 16, i32 57, i32 2, i32 58, i32 67, i32 49, i32 19, ; 80..87
	i32 52, i32 34, i32 63, i32 47, i32 22, i32 10, i32 34, i32 4, ; 88..95
	i32 25, i32 45, i32 17, i32 1, i32 40, i32 36, i32 41, i32 66, ; 96..103
	i32 21, i32 40, i32 24, i32 36, i32 27, i32 68, i32 23, i32 2, ; 104..111
	i32 32, i32 9, i32 39, i32 10, i32 12, i32 3, i32 68, i32 11, ; 112..119
	i32 53, i32 54, i32 29, i32 3, i32 38, i32 37, i32 66, i32 8, ; 120..127
	i32 58, i32 23, i32 20, i32 48, i32 41, i32 6, i32 42, i32 57, ; 128..135
	i32 5, i32 43 ; 136..137
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
