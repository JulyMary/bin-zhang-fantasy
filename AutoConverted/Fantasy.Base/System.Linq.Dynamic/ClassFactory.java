package System.Linq.Dynamic;

public class ClassFactory
{
	public static final ClassFactory Instance = new ClassFactory();

	static // Trigger lazy initialization of static fields
	{
	}

	private ModuleBuilder module;
	private java.util.HashMap<Signature, java.lang.Class> classes;
	private int classCount;
	private ReaderWriterLock rwLock;

	private ClassFactory()
	{
		AssemblyName name = new AssemblyName("DynamicClasses");
		AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if ENABLE_LINQ_PARTIAL_TRUST
		new ReflectionPermission(PermissionState.Unrestricted).Assert();
//#endif
		try
		{
			module = assembly.DefineDynamicModule("Module");
		}
		finally
		{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if ENABLE_LINQ_PARTIAL_TRUST
			PermissionSet.RevertAssert();
//#endif
		}
		classes = new java.util.HashMap<Signature, java.lang.Class>();
		rwLock = new ReaderWriterLock();
	}

	public final java.lang.Class GetDynamicClass(Iterable<DynamicProperty> properties)
	{
		rwLock.AcquireReaderLock(Timeout.Infinite);
		try
		{
			Signature signature = new Signature(properties);
			java.lang.Class type = null;
			if (!((type = classes.get(signature)) != null))
			{
				type = CreateDynamicClass(signature.properties);
				classes.put(signature, type);
			}
			return type;
		}
		finally
		{
			rwLock.ReleaseReaderLock();
		}
	}

	private java.lang.Class CreateDynamicClass(DynamicProperty[] properties)
	{
		LockCookie cookie = rwLock.UpgradeToWriterLock(Timeout.Infinite);
		try
		{
			String typeName = "DynamicClass" + (classCount + 1);
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if ENABLE_LINQ_PARTIAL_TRUST
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
//#endif
			try
			{
				TypeBuilder tb = this.module.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, DynamicClass.class);
				java.lang.reflect.Field[] fields = GenerateProperties(tb, properties);
				GenerateEquals(tb, fields);
				GenerateGetHashCode(tb, fields);
				java.lang.Class result = tb.CreateType();
				classCount++;
				return result;
			}
			finally
			{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if ENABLE_LINQ_PARTIAL_TRUST
				PermissionSet.RevertAssert();
//#endif
			}
		}
		finally
		{
			RefObject<LockCookie> tempRef_cookie = new RefObject<LockCookie>(cookie);
			rwLock.DowngradeFromWriterLock(tempRef_cookie);
			cookie = tempRef_cookie.argvalue;
		}
	}

	private java.lang.reflect.Field[] GenerateProperties(TypeBuilder tb, DynamicProperty[] properties)
	{
		java.lang.reflect.Field[] fields = new FieldBuilder[properties.length];
		for (int i = 0; i < properties.length; i++)
		{
			DynamicProperty dp = properties[i];
			FieldBuilder fb = tb.DefineField("_" + dp.getName(), dp.getType(), FieldAttributes.Private);
			PropertyBuilder pb = tb.DefineProperty(dp.getName(), PropertyAttributes.HasDefault, dp.getType(), null);
			MethodBuilder mbGet = tb.DefineMethod("get_" + dp.getName(), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, dp.getType(), java.lang.Class.EmptyTypes);
			ILGenerator genGet = mbGet.GetILGenerator();
			genGet.Emit(OpCodes.Ldarg_0);
			genGet.Emit(OpCodes.Ldfld, fb);
			genGet.Emit(OpCodes.Ret);
			MethodBuilder mbSet = tb.DefineMethod("set_" + dp.getName(), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new java.lang.Class[] { dp.getType() });
			ILGenerator genSet = mbSet.GetILGenerator();
			genSet.Emit(OpCodes.Ldarg_0);
			genSet.Emit(OpCodes.Ldarg_1);
			genSet.Emit(OpCodes.Stfld, fb);
			genSet.Emit(OpCodes.Ret);
			pb.SetGetMethod(mbGet);
			pb.SetSetMethod(mbSet);
			fields[i] = fb;
		}
		return fields;
	}

	private void GenerateEquals(TypeBuilder tb, java.lang.reflect.Field[] fields)
	{
		MethodBuilder mb = tb.DefineMethod("Equals", MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig, Boolean.class, new java.lang.Class[] { Object.class });
		ILGenerator gen = mb.GetILGenerator();
		LocalBuilder other = gen.DeclareLocal(tb);
		Label next = gen.DefineLabel();
		gen.Emit(OpCodes.Ldarg_1);
		gen.Emit(OpCodes.Isinst, tb);
		gen.Emit(OpCodes.Stloc, other);
		gen.Emit(OpCodes.Ldloc, other);
		gen.Emit(OpCodes.Brtrue_S, next);
		gen.Emit(OpCodes.Ldc_I4_0);
		gen.Emit(OpCodes.Ret);
		gen.MarkLabel(next);
		for (java.lang.reflect.Field field : fields)
		{
			java.lang.Class ft = field.FieldType;
			java.lang.Class ct = EqualityComparer<>.class.MakeGenericType(ft);
			next = gen.DefineLabel();
			gen.EmitCall(OpCodes.Call, ct.getMethod("get_Default"), null);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Ldfld, field);
			gen.Emit(OpCodes.Ldloc, other);
			gen.Emit(OpCodes.Ldfld, field);
			gen.EmitCall(OpCodes.Callvirt, ct.getMethod("Equals", new java.lang.Class[] { ft, ft }), null);
			gen.Emit(OpCodes.Brtrue_S, next);
			gen.Emit(OpCodes.Ldc_I4_0);
			gen.Emit(OpCodes.Ret);
			gen.MarkLabel(next);
		}
		gen.Emit(OpCodes.Ldc_I4_1);
		gen.Emit(OpCodes.Ret);
	}

	private void GenerateGetHashCode(TypeBuilder tb, java.lang.reflect.Field[] fields)
	{
		MethodBuilder mb = tb.DefineMethod("GetHashCode", MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig, Integer.class, java.lang.Class.EmptyTypes);
		ILGenerator gen = mb.GetILGenerator();
		gen.Emit(OpCodes.Ldc_I4_0);
		for (java.lang.reflect.Field field : fields)
		{
			java.lang.Class ft = field.FieldType;
			java.lang.Class ct = EqualityComparer<>.class.MakeGenericType(ft);
			gen.EmitCall(OpCodes.Call, ct.getMethod("get_Default"), null);
			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Ldfld, field);
			gen.EmitCall(OpCodes.Callvirt, ct.getMethod("GetHashCode", new java.lang.Class[] { ft }), null);
			gen.Emit(OpCodes.Xor);
		}
		gen.Emit(OpCodes.Ret);
	}
}