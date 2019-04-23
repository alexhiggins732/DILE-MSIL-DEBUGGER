using System;
using System.Collections.Generic;
using System.Text;

using Dile.Properties;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Dile.Metadata
{
	public sealed class OpCodeGroups
	{
		private static bool isInitialized = false;
		public static bool IsInitialized
		{
			get
			{
				return OpCodeGroups.isInitialized;
			}

			set
			{
				OpCodeGroups.isInitialized = value;
			}
		}

		private static List<OpCode> parameterless = new List<OpCode>();
		public static List<OpCode> Parameterless
		{
			get
			{
				return OpCodeGroups.parameterless;
			}

			set
			{
				OpCodeGroups.parameterless = value;
			}
		}

		private static List<OpCode> fieldParameter = new List<OpCode>();
		public static List<OpCode> FieldParameter
		{
			get
			{
				return OpCodeGroups.fieldParameter;
			}

			set
			{
				OpCodeGroups.fieldParameter = value;
			}
		}

		private static List<OpCode> methodParameter = new List<OpCode>();
		public static List<OpCode> MethodParameter
		{
			get
			{
				return OpCodeGroups.methodParameter;
			}

			set
			{
				OpCodeGroups.methodParameter = value;
			}
		}

		private static List<OpCode> stringParameter = new List<OpCode>();
		public static List<OpCode> StringParameter
		{
			get
			{
				return OpCodeGroups.stringParameter;
			}

			set
			{
				OpCodeGroups.stringParameter = value;
			}
		}

		private static List<OpCode> typeParameter = new List<OpCode>();
		public static List<OpCode> TypeParameter
		{
			get
			{
				return OpCodeGroups.typeParameter;
			}

			set
			{
				OpCodeGroups.typeParameter = value;
			}
		}

		private static List<OpCode> sbyteLocationParameter = new List<OpCode>();
		public static List<OpCode> SbyteLocationParameter
		{
			get
			{
				return OpCodeGroups.sbyteLocationParameter;
			}

			set
			{
				OpCodeGroups.sbyteLocationParameter = value;
			}
		}

		private static List<OpCode> intLocationParameter = new List<OpCode>();
		public static List<OpCode> IntLocationParameter
		{
			get
			{
				return OpCodeGroups.intLocationParameter;
			}

			set
			{
				OpCodeGroups.intLocationParameter = value;
			}
		}

		private static List<OpCode> byteParameter = new List<OpCode>();
		public static List<OpCode> ByteParameter
		{
			get
			{
				return OpCodeGroups.byteParameter;
			}

			set
			{
				OpCodeGroups.byteParameter = value;
			}
		}

		private static List<OpCode> ushortParameter = new List<OpCode>();
		public static List<OpCode> UshortParameter
		{
			get
			{
				return OpCodeGroups.ushortParameter;
			}

			set
			{
				OpCodeGroups.ushortParameter = value;
			}
		}

		private static List<OpCode> sbyteParameter = new List<OpCode>();
		public static List<OpCode> SbyteParameter
		{
			get
			{
				return OpCodeGroups.sbyteParameter;
			}

			set
			{
				OpCodeGroups.sbyteParameter = value;
			}
		}

		private static List<OpCode> intParameter = new List<OpCode>();
		public static List<OpCode> IntParameter
		{
			get
			{
				return OpCodeGroups.intParameter;
			}

			set
			{
				OpCodeGroups.intParameter = value;
			}
		}

		private static List<OpCode> longParameter = new List<OpCode>();
		public static List<OpCode> LongParameter
		{
			get
			{
				return OpCodeGroups.longParameter;
			}

			set
			{
				OpCodeGroups.longParameter = value;
			}
		}

		private static List<OpCode> floatParameter = new List<OpCode>();
		public static List<OpCode> FloatParameter
		{
			get
			{
				return OpCodeGroups.floatParameter;
			}

			set
			{
				OpCodeGroups.floatParameter = value;
			}
		}

		private static List<OpCode> doubleParameter = new List<OpCode>();
		public static List<OpCode> DoubleParameter
		{
			get
			{
				return OpCodeGroups.doubleParameter;
			}

			set
			{
				OpCodeGroups.doubleParameter = value;
			}
		}

		private static List<OpCode> byteArgumentParameter = new List<OpCode>();
		public static List<OpCode> ByteArgumentParameter
		{
			get
			{
				return OpCodeGroups.byteArgumentParameter;
			}

			set
			{
				OpCodeGroups.byteArgumentParameter = value;
			}
		}

		private static List<OpCode> ushortArgumentParameter = new List<OpCode>();
		public static List<OpCode> UshortArgumentParameter
		{
			get
			{
				return OpCodeGroups.ushortArgumentParameter;
			}

			set
			{
				OpCodeGroups.ushortArgumentParameter = value;
			}
		}

		private static List<OpCode> byteVariableParameter = new List<OpCode>();
		public static List<OpCode> ByteVariableParameter
		{
			get
			{
				return OpCodeGroups.byteVariableParameter;
			}

			set
			{
				OpCodeGroups.byteVariableParameter = value;
			}
		}

		private static List<OpCode> ushortVariableParameter = new List<OpCode>();
		public static List<OpCode> UshortVariableParameter
		{
			get
			{
				return OpCodeGroups.ushortVariableParameter;
			}

			set
			{
				OpCodeGroups.ushortVariableParameter = value;
			}
		}

		private static Dictionary<short, OpCode> opCodesByValue = new Dictionary<short, OpCode>();
		public static Dictionary<short, OpCode> OpCodesByValue
		{
			get
			{
				return OpCodeGroups.opCodesByValue;
			}

			set
			{
				OpCodeGroups.opCodesByValue = value;
			}
		}

		private static Dictionary<Type, string> nativeTypeNames = new Dictionary<Type, string>();
		public static Dictionary<Type, string> NativeTypeNames
		{
			get
			{
				return OpCodeGroups.nativeTypeNames;
			}

			set
			{
				OpCodeGroups.nativeTypeNames = value;
			}
		}

		private static Dictionary<string, Type> nameOfNativeTypes = new Dictionary<string, Type>();
		public static Dictionary<string, Type> NameOfNativeTypes
		{
			get
			{
				return OpCodeGroups.nameOfNativeTypes;
			}

			set
			{
				OpCodeGroups.nameOfNativeTypes = value;
			}
		}

		private static List<string> keywords = new List<string>();
		public static List<string> Keywords
		{
			get
			{
				return keywords;
			}
			set
			{
				keywords = value;
			}
		}

		private static IDictionary<OpCode, OpCodeItem> opCodeItemsByOpCode;
		public static IDictionary<OpCode, OpCodeItem> OpCodeItemsByOpCode
		{
			get
			{
				return opCodeItemsByOpCode;
			}
			private set
			{
				opCodeItemsByOpCode = value;
			}
		}

		private OpCodeGroups()
		{
		}

		private static void InitializeOpCodeItemsByOpCode()
		{
			IDictionary<string, OpCode> opCodesByFieldName = typeof(OpCodes).GetFields()
				.ToDictionary(field => field.Name, field => (OpCode)field.GetValue(null));

			using (StringReader opCodesReader = new StringReader(Resources.OpCodes))
			{
				XDocument opCodesDoc = XDocument.Load(opCodesReader);
				OpCodeItemsByOpCode = new Dictionary<OpCode, OpCodeItem>();

				foreach (XElement opCodeElement in opCodesDoc.Root.Elements())
				{
					string opCodeFieldName = opCodeElement.Attribute("name").Value;
					string description = opCodeElement.Value.Trim();

					OpCode opCode = opCodesByFieldName[opCodeFieldName];
					OpCodeItem opCodeItem = new OpCodeItem(opCodeFieldName, opCode, description);
					OpCodeItemsByOpCode.Add(opCode, opCodeItem);
				}
			}
		}

		private static void InitializeFields()
		{
			ByteParameter.Add(OpCodes.Unaligned);
			Parameterless.Add(OpCodes.Tailcall);
			Parameterless.Add(OpCodes.Volatile);

			Parameterless.Add(OpCodes.Add);
			Parameterless.Add(OpCodes.Add_Ovf);
			Parameterless.Add(OpCodes.Add_Ovf_Un);
			Parameterless.Add(OpCodes.And);
			Parameterless.Add(OpCodes.Arglist);
			Parameterless.Add(OpCodes.Break);
			Parameterless.Add(OpCodes.Ceq);
			Parameterless.Add(OpCodes.Cgt);
			Parameterless.Add(OpCodes.Cgt_Un);
			Parameterless.Add(OpCodes.Ckfinite);
			Parameterless.Add(OpCodes.Clt);
			Parameterless.Add(OpCodes.Clt_Un);
			Parameterless.Add(OpCodes.Conv_I1);
			Parameterless.Add(OpCodes.Conv_I2);
			Parameterless.Add(OpCodes.Conv_I4);
			Parameterless.Add(OpCodes.Conv_I8);
			Parameterless.Add(OpCodes.Conv_R4);
			Parameterless.Add(OpCodes.Conv_R8);
			Parameterless.Add(OpCodes.Conv_U1);
			Parameterless.Add(OpCodes.Conv_U2);
			Parameterless.Add(OpCodes.Conv_U4);
			Parameterless.Add(OpCodes.Conv_U8);
			Parameterless.Add(OpCodes.Conv_I);
			Parameterless.Add(OpCodes.Conv_U);
			Parameterless.Add(OpCodes.Conv_R_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_I1);
			Parameterless.Add(OpCodes.Conv_Ovf_I2);
			Parameterless.Add(OpCodes.Conv_Ovf_I4);
			Parameterless.Add(OpCodes.Conv_Ovf_I8);
			Parameterless.Add(OpCodes.Conv_Ovf_U1);
			Parameterless.Add(OpCodes.Conv_Ovf_U2);
			Parameterless.Add(OpCodes.Conv_Ovf_U4);
			Parameterless.Add(OpCodes.Conv_Ovf_U8);
			Parameterless.Add(OpCodes.Conv_Ovf_I);
			Parameterless.Add(OpCodes.Conv_Ovf_U);
			Parameterless.Add(OpCodes.Conv_Ovf_I1_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_I2_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_I4_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_I8_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_U1_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_U2_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_U4_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_U8_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_I_Un);
			Parameterless.Add(OpCodes.Conv_Ovf_U_Un);
			Parameterless.Add(OpCodes.Cpblk);
			Parameterless.Add(OpCodes.Div);
			Parameterless.Add(OpCodes.Div_Un);
			Parameterless.Add(OpCodes.Dup);
			Parameterless.Add(OpCodes.Endfilter);
			Parameterless.Add(OpCodes.Endfinally);
			Parameterless.Add(OpCodes.Initblk);
			Parameterless.Add(OpCodes.Ldarg_0);
			Parameterless.Add(OpCodes.Ldarg_1);
			Parameterless.Add(OpCodes.Ldarg_2);
			Parameterless.Add(OpCodes.Ldarg_3);
			Parameterless.Add(OpCodes.Ldc_I4_0);
			Parameterless.Add(OpCodes.Ldc_I4_1);
			Parameterless.Add(OpCodes.Ldc_I4_2);
			Parameterless.Add(OpCodes.Ldc_I4_3);
			Parameterless.Add(OpCodes.Ldc_I4_4);
			Parameterless.Add(OpCodes.Ldc_I4_5);
			Parameterless.Add(OpCodes.Ldc_I4_6);
			Parameterless.Add(OpCodes.Ldc_I4_7);
			Parameterless.Add(OpCodes.Ldc_I4_8);
			Parameterless.Add(OpCodes.Ldc_I4_M1);
			Parameterless.Add(OpCodes.Ldind_I1);
			Parameterless.Add(OpCodes.Ldind_I2);
			Parameterless.Add(OpCodes.Ldind_I4);
			Parameterless.Add(OpCodes.Ldind_I8);
			Parameterless.Add(OpCodes.Ldind_U1);
			Parameterless.Add(OpCodes.Ldind_U2);
			Parameterless.Add(OpCodes.Ldind_U4);
			Parameterless.Add(OpCodes.Ldind_R4);
			Parameterless.Add(OpCodes.Ldind_R8);
			Parameterless.Add(OpCodes.Ldind_I);
			Parameterless.Add(OpCodes.Ldind_Ref);
			Parameterless.Add(OpCodes.Ldloc_0);
			Parameterless.Add(OpCodes.Ldloc_1);
			Parameterless.Add(OpCodes.Ldloc_2);
			Parameterless.Add(OpCodes.Ldloc_3);
			Parameterless.Add(OpCodes.Ldnull);
			Parameterless.Add(OpCodes.Localloc);
			Parameterless.Add(OpCodes.Mul);
			Parameterless.Add(OpCodes.Mul_Ovf);
			Parameterless.Add(OpCodes.Mul_Ovf_Un);
			Parameterless.Add(OpCodes.Neg);
			Parameterless.Add(OpCodes.Nop);
			Parameterless.Add(OpCodes.Not);
			Parameterless.Add(OpCodes.Or);
			Parameterless.Add(OpCodes.Pop);
			Parameterless.Add(OpCodes.Rem);
			Parameterless.Add(OpCodes.Rem_Un);
			Parameterless.Add(OpCodes.Ret);
			Parameterless.Add(OpCodes.Shl);
			Parameterless.Add(OpCodes.Shr);
			Parameterless.Add(OpCodes.Shr_Un);
			Parameterless.Add(OpCodes.Stind_I1);
			Parameterless.Add(OpCodes.Stind_I2);
			Parameterless.Add(OpCodes.Stind_I4);
			Parameterless.Add(OpCodes.Stind_I8);
			Parameterless.Add(OpCodes.Stind_R4);
			Parameterless.Add(OpCodes.Stind_R8);
			Parameterless.Add(OpCodes.Stind_I);
			Parameterless.Add(OpCodes.Stind_Ref);
			Parameterless.Add(OpCodes.Stloc_0);
			Parameterless.Add(OpCodes.Stloc_1);
			Parameterless.Add(OpCodes.Stloc_2);
			Parameterless.Add(OpCodes.Stloc_3);
			Parameterless.Add(OpCodes.Sub);
			Parameterless.Add(OpCodes.Sub_Ovf);
			Parameterless.Add(OpCodes.Sub_Ovf_Un);
			Parameterless.Add(OpCodes.Xor);
			Parameterless.Add(OpCodes.Ldelem_I1);
			Parameterless.Add(OpCodes.Ldelem_I2);
			Parameterless.Add(OpCodes.Ldelem_I4);
			Parameterless.Add(OpCodes.Ldelem_I8);
			Parameterless.Add(OpCodes.Ldelem_U1);
			Parameterless.Add(OpCodes.Ldelem_U2);
			Parameterless.Add(OpCodes.Ldelem_U4);
			Parameterless.Add(OpCodes.Ldelem_R4);
			Parameterless.Add(OpCodes.Ldelem_R8);
			Parameterless.Add(OpCodes.Ldelem_I);
			Parameterless.Add(OpCodes.Ldelem_Ref);
			Parameterless.Add(OpCodes.Ldlen);
			Parameterless.Add(OpCodes.Refanytype);
			Parameterless.Add(OpCodes.Rethrow);
			Parameterless.Add(OpCodes.Stelem_I1);
			Parameterless.Add(OpCodes.Stelem_I2);
			Parameterless.Add(OpCodes.Stelem_I4);
			Parameterless.Add(OpCodes.Stelem_I8);
			Parameterless.Add(OpCodes.Stelem_R4);
			Parameterless.Add(OpCodes.Stelem_R8);
			Parameterless.Add(OpCodes.Stelem_I);
			Parameterless.Add(OpCodes.Stelem_Ref);
			Parameterless.Add(OpCodes.Throw);

			SbyteLocationParameter.Add(OpCodes.Beq_S);
			SbyteLocationParameter.Add(OpCodes.Bge_S);
			SbyteLocationParameter.Add(OpCodes.Bge_Un_S);
			SbyteLocationParameter.Add(OpCodes.Bgt_S);
			SbyteLocationParameter.Add(OpCodes.Bgt_Un_S);
			SbyteLocationParameter.Add(OpCodes.Ble_S);
			SbyteLocationParameter.Add(OpCodes.Ble_Un_S);
			SbyteLocationParameter.Add(OpCodes.Blt_S);
			SbyteLocationParameter.Add(OpCodes.Blt_Un_S);
			SbyteLocationParameter.Add(OpCodes.Bne_Un_S);
			SbyteLocationParameter.Add(OpCodes.Br_S);
			SbyteLocationParameter.Add(OpCodes.Brfalse_S);
			SbyteLocationParameter.Add(OpCodes.Brtrue_S);
			SbyteLocationParameter.Add(OpCodes.Leave_S);

			IntLocationParameter.Add(OpCodes.Beq);
			IntLocationParameter.Add(OpCodes.Bge);
			IntLocationParameter.Add(OpCodes.Bge_Un);
			IntLocationParameter.Add(OpCodes.Bgt);
			IntLocationParameter.Add(OpCodes.Bgt_Un);
			IntLocationParameter.Add(OpCodes.Ble);
			IntLocationParameter.Add(OpCodes.Ble_Un);
			IntLocationParameter.Add(OpCodes.Blt);
			IntLocationParameter.Add(OpCodes.Blt_Un);
			IntLocationParameter.Add(OpCodes.Bne_Un);
			IntLocationParameter.Add(OpCodes.Br);
			IntLocationParameter.Add(OpCodes.Brfalse);
			IntLocationParameter.Add(OpCodes.Brtrue);
			IntLocationParameter.Add(OpCodes.Leave);

			SbyteParameter.Add(OpCodes.Ldc_I4_S);

			IntParameter.Add(OpCodes.Ldc_I4);

			LongParameter.Add(OpCodes.Ldc_I8);

			FloatParameter.Add(OpCodes.Ldc_R4);

			DoubleParameter.Add(OpCodes.Ldc_R8);

			FieldParameter.Add(OpCodes.Ldfld);
			FieldParameter.Add(OpCodes.Ldflda);
			FieldParameter.Add(OpCodes.Ldsfld);
			FieldParameter.Add(OpCodes.Ldsflda);
			FieldParameter.Add(OpCodes.Stfld);
			FieldParameter.Add(OpCodes.Stsfld);

			MethodParameter.Add(OpCodes.Call);
			MethodParameter.Add(OpCodes.Callvirt);
			MethodParameter.Add(OpCodes.Jmp);
			MethodParameter.Add(OpCodes.Ldftn);
			MethodParameter.Add(OpCodes.Ldvirtftn);

			StringParameter.Add(OpCodes.Ldstr);

			TypeParameter.Add(OpCodes.Box);
			TypeParameter.Add(OpCodes.Castclass);
			TypeParameter.Add(OpCodes.Constrained);
			TypeParameter.Add(OpCodes.Cpobj);
			TypeParameter.Add(OpCodes.Initobj);
			TypeParameter.Add(OpCodes.Isinst);
			TypeParameter.Add(OpCodes.Ldelem);
			TypeParameter.Add(OpCodes.Ldelema);
			TypeParameter.Add(OpCodes.Ldobj);
			TypeParameter.Add(OpCodes.Mkrefany);
			TypeParameter.Add(OpCodes.Newarr);
			TypeParameter.Add(OpCodes.Newobj);
			TypeParameter.Add(OpCodes.Refanyval);
			TypeParameter.Add(OpCodes.Sizeof);
			TypeParameter.Add(OpCodes.Stelem);
			TypeParameter.Add(OpCodes.Stobj);
			TypeParameter.Add(OpCodes.Unbox);
			TypeParameter.Add(OpCodes.Unbox_Any);

			ByteArgumentParameter.Add(OpCodes.Ldarg_S);
			ByteArgumentParameter.Add(OpCodes.Ldarga_S);
			ByteArgumentParameter.Add(OpCodes.Starg_S);

			UshortArgumentParameter.Add(OpCodes.Ldarg);
			UshortArgumentParameter.Add(OpCodes.Ldarga);
			UshortArgumentParameter.Add(OpCodes.Starg);

			ByteVariableParameter.Add(OpCodes.Ldloc_S);
			ByteVariableParameter.Add(OpCodes.Ldloca_S);
			ByteVariableParameter.Add(OpCodes.Stloc_S);

			UshortVariableParameter.Add(OpCodes.Ldloc);
			UshortVariableParameter.Add(OpCodes.Ldloca);
			UshortVariableParameter.Add(OpCodes.Stloc);

			foreach (FieldInfo fieldInfo in typeof(OpCodes).GetFields())
			{
				OpCode code = (OpCode)fieldInfo.GetValue(null);
				OpCodesByValue[code.Value] = code;
			}



			NativeTypeNames.Add(typeof(bool), "bool");
			//boxed
			NativeTypeNames.Add(typeof(char), "char");
			//class
			NativeTypeNames.Add(typeof(float), "float32");
			NativeTypeNames.Add(typeof(double), "float64");
			NativeTypeNames.Add(typeof(sbyte), "int8");
			NativeTypeNames.Add(typeof(short), "int16");
			NativeTypeNames.Add(typeof(int), "int32");
			NativeTypeNames.Add(typeof(long), "int64");
			NativeTypeNames.Add(typeof(IntPtr), "native int");
			NativeTypeNames.Add(typeof(UIntPtr), "native unsigned int");
			NativeTypeNames.Add(typeof(object), "object");
			NativeTypeNames.Add(typeof(string), "string");
			NativeTypeNames.Add(typeof(byte), "unsigned int8");
			NativeTypeNames.Add(typeof(ushort), "unsigned int16");
			NativeTypeNames.Add(typeof(uint), "unsigned int32");
			NativeTypeNames.Add(typeof(ulong), "unsigned int64");
			NativeTypeNames.Add(typeof(void), "void");

			foreach (KeyValuePair<Type, string> keyValue in NativeTypeNames)
			{
				NameOfNativeTypes.Add(keyValue.Value, keyValue.Key);
			}

			Keywords.Add("#line");
			Keywords.Add(".addon");
			Keywords.Add(".assembly");
			Keywords.Add(".cctor");
			Keywords.Add(".class");
			Keywords.Add(".corflags");
			Keywords.Add(".ctor");
			Keywords.Add(".custom");
			Keywords.Add(".data");
			Keywords.Add(".emitbyte");
			Keywords.Add(".entrypoint");
			Keywords.Add(".event");
			Keywords.Add(".export");
			Keywords.Add(".field");
			Keywords.Add(".file");
			Keywords.Add(".fire");
			Keywords.Add(".get");
			Keywords.Add(".hash");
			Keywords.Add(".imagebase");
			Keywords.Add(".import");
			Keywords.Add(".language");
			Keywords.Add(".line");
			Keywords.Add(".locale");
			Keywords.Add(".localized");
			Keywords.Add(".locals");
			Keywords.Add(".manifestres");
			Keywords.Add(".maxstack");
			Keywords.Add(".method");
			Keywords.Add(".module");
			Keywords.Add(".mresource");
			Keywords.Add(".namespace");
			Keywords.Add(".other");
			Keywords.Add(".override");
			Keywords.Add(".pack");
			Keywords.Add(".param");
			Keywords.Add(".pdirect");
			Keywords.Add(".permission");
			Keywords.Add(".permissionset");
			Keywords.Add(".property");
			Keywords.Add(".publickey");
			Keywords.Add(".publickeytoken");
			Keywords.Add(".removeon");
			Keywords.Add(".set");
			Keywords.Add(".size");
			Keywords.Add(".subsystem");
			Keywords.Add(".try");
			Keywords.Add(".ver");
			Keywords.Add(".vtable");
			Keywords.Add(".vtentry");
			Keywords.Add(".vtfixup");
			Keywords.Add(".zeroinit");
			Keywords.Add("^THE_END^");
			Keywords.Add("abstract");
			Keywords.Add("add");
			Keywords.Add("add.ovf");
			Keywords.Add("add.ovf.un");
			Keywords.Add("algorithm");
			Keywords.Add("alignment");
			Keywords.Add("and");
			Keywords.Add("ansi");
			Keywords.Add("any");
			Keywords.Add("arglist");
			Keywords.Add("array");
			Keywords.Add("as");
			Keywords.Add("assembly");
			Keywords.Add("assert");
			Keywords.Add("at");
			Keywords.Add("auto");
			Keywords.Add("autochar");
			Keywords.Add("beforefieldinit");
			Keywords.Add("beq");
			Keywords.Add("beq.s");
			Keywords.Add("bge");
			Keywords.Add("bge.s");
			Keywords.Add("bge.un");
			Keywords.Add("bge.un.s");
			Keywords.Add("bgt");
			Keywords.Add("bgt.s");
			Keywords.Add("bgt.un");
			Keywords.Add("bgt.un.s");
			Keywords.Add("ble");
			Keywords.Add("ble.s");
			Keywords.Add("ble.un");
			Keywords.Add("ble.un.s");
			Keywords.Add("blob");
			Keywords.Add("blob_object");
			Keywords.Add("blt");
			Keywords.Add("blt.s");
			Keywords.Add("blt.un");
			Keywords.Add("blt.un.s");
			Keywords.Add("bne.un");
			Keywords.Add("bne.un.s");
			Keywords.Add("bool");
			Keywords.Add("box");
			Keywords.Add("br");
			Keywords.Add("br.s");
			Keywords.Add("break");
			Keywords.Add("brfalse");
			Keywords.Add("brfalse.s");
			Keywords.Add("brinst");
			Keywords.Add("brinst.s");
			Keywords.Add("brnull");
			Keywords.Add("brnull.s");
			Keywords.Add("brtrue");
			Keywords.Add("brtrue.s");
			Keywords.Add("brzero");
			Keywords.Add("brzero.s");
			Keywords.Add("bstr");
			Keywords.Add("bytearray");
			Keywords.Add("byvalstr");
			Keywords.Add("call");
			Keywords.Add("calli");
			Keywords.Add("callmostderived");
			Keywords.Add("callvirt");
			Keywords.Add("carray");
			Keywords.Add("castclass");
			Keywords.Add("catch");
			Keywords.Add("cdecl");
			Keywords.Add("ceq");
			Keywords.Add("cf");
			Keywords.Add("cgt");
			Keywords.Add("cgt.un");
			//Keywords.Add("char");
			Keywords.Add("cil");
			Keywords.Add("ckfinite");
			Keywords.Add("class");
			Keywords.Add("clsid");
			Keywords.Add("clt");
			Keywords.Add("clt.un");
			Keywords.Add("const");
			Keywords.Add("conv.i");
			Keywords.Add("conv.i1");
			Keywords.Add("conv.i2");
			Keywords.Add("conv.i4");
			Keywords.Add("conv.i8");
			Keywords.Add("conv.ovf.i");
			Keywords.Add("conv.ovf.i.un");
			Keywords.Add("conv.ovf.i1");
			Keywords.Add("conv.ovf.i1.un");
			Keywords.Add("conv.ovf.i2");
			Keywords.Add("conv.ovf.i2.un");
			Keywords.Add("conv.ovf.i4");
			Keywords.Add("conv.ovf.i4.un");
			Keywords.Add("conv.ovf.i8");
			Keywords.Add("conv.ovf.i8.un");
			Keywords.Add("conv.ovf.u");
			Keywords.Add("conv.ovf.u.un");
			Keywords.Add("conv.ovf.u1");
			Keywords.Add("conv.ovf.u1.un");
			Keywords.Add("conv.ovf.u2");
			Keywords.Add("conv.ovf.u2.un");
			Keywords.Add("conv.ovf.u4");
			Keywords.Add("conv.ovf.u4.un");
			Keywords.Add("conv.ovf.u8");
			Keywords.Add("conv.ovf.u8.un");
			Keywords.Add("conv.r.un");
			Keywords.Add("conv.r4");
			Keywords.Add("conv.r8");
			Keywords.Add("conv.u");
			Keywords.Add("conv.u1");
			Keywords.Add("conv.u2");
			Keywords.Add("conv.u4");
			Keywords.Add("conv.u8");
			Keywords.Add("cpblk");
			Keywords.Add("cpobj");
			//Keywords.Add("currency");
			Keywords.Add("custom");
			//Keywords.Add("date");
			//Keywords.Add("decimal");
			Keywords.Add("default");
			Keywords.Add("demand");
			Keywords.Add("deny");
			Keywords.Add("div");
			Keywords.Add("div.un");
			Keywords.Add("dup");
			Keywords.Add("endfault");
			Keywords.Add("endfilter");
			Keywords.Add("endfinally");
			Keywords.Add("endmac");
			Keywords.Add("enum");
			Keywords.Add("error");
			Keywords.Add("explicit");
			Keywords.Add("extends");
			Keywords.Add("extern");
			Keywords.Add("false");
			Keywords.Add("famandassem");
			Keywords.Add("family");
			Keywords.Add("famorassem");
			Keywords.Add("fastcall");
			Keywords.Add("fastcall");
			Keywords.Add("fault");
			Keywords.Add("field");
			Keywords.Add("filetime");
			Keywords.Add("filter");
			Keywords.Add("final");
			Keywords.Add("finally");
			Keywords.Add("fixed");
			//Keywords.Add("float");
			//Keywords.Add("float32");
			//Keywords.Add("float64");
			Keywords.Add("forwardref");
			Keywords.Add("fromunmanaged");
			Keywords.Add("handler");
			Keywords.Add("hidebysig");
			Keywords.Add("hresult");
			Keywords.Add("idispatch");
			Keywords.Add("il");
			Keywords.Add("illegal");
			Keywords.Add("implements");
			Keywords.Add("implicitcom");
			Keywords.Add("implicitres");
			Keywords.Add("import");
			Keywords.Add("in");
			Keywords.Add("inheritcheck");
			Keywords.Add("init");
			Keywords.Add("initblk");
			Keywords.Add("initobj");
			Keywords.Add("initonly");
			Keywords.Add("instance");
			//Keywords.Add("int");
			//Keywords.Add("int16");
			//Keywords.Add("int32");
			//Keywords.Add("int64");
			//Keywords.Add("int8");
			Keywords.Add("interface");
			Keywords.Add("internalcall");
			Keywords.Add("isinst");
			Keywords.Add("iunknown");
			Keywords.Add("jmp");
			Keywords.Add("lasterr");
			Keywords.Add("lcid");
			Keywords.Add("ldarg");
			Keywords.Add("ldarg.0");
			Keywords.Add("ldarg.1");
			Keywords.Add("ldarg.2");
			Keywords.Add("ldarg.3");
			Keywords.Add("ldarg.s");
			Keywords.Add("ldarga");
			Keywords.Add("ldarga.s");
			Keywords.Add("ldc.i4");
			Keywords.Add("ldc.i4.0");
			Keywords.Add("ldc.i4.1");
			Keywords.Add("ldc.i4.2");
			Keywords.Add("ldc.i4.3");
			Keywords.Add("ldc.i4.4");
			Keywords.Add("ldc.i4.5");
			Keywords.Add("ldc.i4.6");
			Keywords.Add("ldc.i4.7");
			Keywords.Add("ldc.i4.8");
			Keywords.Add("ldc.i4.M1");
			Keywords.Add("ldc.i4.m1");
			Keywords.Add("ldc.i4.s");
			Keywords.Add("ldc.i8");
			Keywords.Add("ldc.r4");
			Keywords.Add("ldc.r8");
			Keywords.Add("ldelem.i");
			Keywords.Add("ldelem.i1");
			Keywords.Add("ldelem.i2");
			Keywords.Add("ldelem.i4");
			Keywords.Add("ldelem.i8");
			Keywords.Add("ldelem.r4");
			Keywords.Add("ldelem.r8");
			Keywords.Add("ldelem.ref");
			Keywords.Add("ldelem.u1");
			Keywords.Add("ldelem.u2");
			Keywords.Add("ldelem.u4");
			Keywords.Add("ldelem.u8");
			Keywords.Add("ldelema");
			Keywords.Add("ldfld");
			Keywords.Add("ldflda");
			Keywords.Add("ldftn");
			Keywords.Add("ldind.i");
			Keywords.Add("ldind.i1");
			Keywords.Add("ldind.i2");
			Keywords.Add("ldind.i4");
			Keywords.Add("ldind.i8");
			Keywords.Add("ldind.r4");
			Keywords.Add("ldind.r8");
			Keywords.Add("ldind.ref");
			Keywords.Add("ldind.u1");
			Keywords.Add("ldind.u2");
			Keywords.Add("ldind.u4");
			Keywords.Add("ldind.u8");
			Keywords.Add("ldlen");
			Keywords.Add("ldloc");
			Keywords.Add("ldloc.0");
			Keywords.Add("ldloc.1");
			Keywords.Add("ldloc.2");
			Keywords.Add("ldloc.3");
			Keywords.Add("ldloc.s");
			Keywords.Add("ldloca");
			Keywords.Add("ldloca.s");
			Keywords.Add("ldnull");
			Keywords.Add("ldobj");
			Keywords.Add("ldsfld");
			Keywords.Add("ldsflda");
			Keywords.Add("ldstr");
			Keywords.Add("ldtoken");
			Keywords.Add("ldvirtftn");
			Keywords.Add("leave");
			Keywords.Add("leave.s");
			Keywords.Add("linkcheck");
			Keywords.Add("literal");
			Keywords.Add("localloc");
			Keywords.Add("lpstr");
			Keywords.Add("lpstruct");
			Keywords.Add("lptstr");
			Keywords.Add("lpvoid");
			Keywords.Add("lpwstr");
			Keywords.Add("managed");
			Keywords.Add("marshal");
			Keywords.Add("method");
			Keywords.Add("mkrefany");
			Keywords.Add("modopt");
			Keywords.Add("modreq");
			Keywords.Add("mul");
			Keywords.Add("mul.ovf");
			Keywords.Add("mul.ovf.un");
			Keywords.Add("native");
			Keywords.Add("neg");
			Keywords.Add("nested");
			Keywords.Add("newarr");
			Keywords.Add("newobj");
			Keywords.Add("newslot");
			Keywords.Add("noappdomain");
			Keywords.Add("noinlining");
			Keywords.Add("nomachine");
			Keywords.Add("nomangle");
			Keywords.Add("nometadata");
			Keywords.Add("noncasdemand");
			Keywords.Add("noncasinheritance");
			Keywords.Add("noncaslinkdemand");
			Keywords.Add("nop");
			Keywords.Add("noprocess");
			Keywords.Add("not");
			Keywords.Add("not_in_gc_heap");
			Keywords.Add("notremotable");
			Keywords.Add("notserialized");
			Keywords.Add("null");
			Keywords.Add("nullref");
			//Keywords.Add("object");
			Keywords.Add("objectref");
			Keywords.Add("opt");
			Keywords.Add("optil");
			Keywords.Add("or");
			Keywords.Add("out");
			Keywords.Add("permitonly");
			Keywords.Add("pinned");
			Keywords.Add("pinvokeimpl");
			Keywords.Add("pop");
			Keywords.Add("prefix1");
			Keywords.Add("prefix2");
			Keywords.Add("prefix3");
			Keywords.Add("prefix4");
			Keywords.Add("prefix5");
			Keywords.Add("prefix6");
			Keywords.Add("prefix7");
			Keywords.Add("prefixref");
			Keywords.Add("prejitdeny");
			Keywords.Add("prejitgrant");
			Keywords.Add("preservesig");
			Keywords.Add("private");
			Keywords.Add("privatescope");
			Keywords.Add("protected");
			Keywords.Add("public");
			Keywords.Add("readonly");
			Keywords.Add("record");
			Keywords.Add("refany");
			Keywords.Add("refanytype");
			Keywords.Add("refanyval");
			Keywords.Add("rem");
			Keywords.Add("rem.un");
			Keywords.Add("reqmin");
			Keywords.Add("reqopt");
			Keywords.Add("reqrefuse");
			Keywords.Add("reqsecobj");
			Keywords.Add("request");
			Keywords.Add("ret");
			Keywords.Add("rethrow");
			Keywords.Add("retval");
			Keywords.Add("rtspecialname");
			Keywords.Add("runtime");
			Keywords.Add("safearray");
			Keywords.Add("sealed");
			Keywords.Add("sequential");
			Keywords.Add("serializable");
			Keywords.Add("shl");
			Keywords.Add("shr");
			Keywords.Add("shr.un");
			Keywords.Add("sizeof");
			Keywords.Add("special");
			Keywords.Add("specialname");
			Keywords.Add("starg");
			Keywords.Add("starg.s");
			Keywords.Add("static");
			Keywords.Add("stdcall");
			Keywords.Add("stdcall");
			Keywords.Add("stelem.i");
			Keywords.Add("stelem.i1");
			Keywords.Add("stelem.i2");
			Keywords.Add("stelem.i4");
			Keywords.Add("stelem.i8");
			Keywords.Add("stelem.r4");
			Keywords.Add("stelem.r8");
			Keywords.Add("stelem.ref");
			Keywords.Add("stfld");
			Keywords.Add("stind.i");
			Keywords.Add("stind.i1");
			Keywords.Add("stind.i2");
			Keywords.Add("stind.i4");
			Keywords.Add("stind.i8");
			Keywords.Add("stind.r4");
			Keywords.Add("stind.r8");
			Keywords.Add("stind.ref");
			Keywords.Add("stloc");
			Keywords.Add("stloc.0");
			Keywords.Add("stloc.1");
			Keywords.Add("stloc.2");
			Keywords.Add("stloc.3");
			Keywords.Add("stloc.s");
			Keywords.Add("stobj");
			Keywords.Add("storage");
			Keywords.Add("stored_object");
			Keywords.Add("stream");
			Keywords.Add("streamed_object");
			//Keywords.Add("string");
			Keywords.Add("struct");
			Keywords.Add("stsfld");
			Keywords.Add("sub");
			Keywords.Add("sub.ovf");
			Keywords.Add("sub.ovf.un");
			Keywords.Add("switch");
			Keywords.Add("synchronized");
			Keywords.Add("syschar");
			Keywords.Add("sysstring");
			Keywords.Add("tail.");
			Keywords.Add("tbstr");
			Keywords.Add("thiscall");
			Keywords.Add("throw");
			Keywords.Add("tls");
			Keywords.Add("to");
			Keywords.Add("true");
			Keywords.Add("typedref");
			Keywords.Add("unaligned.");
			Keywords.Add("unbox");
			Keywords.Add("unicode");
			Keywords.Add("unmanaged");
			Keywords.Add("unmanagedexp");
			Keywords.Add("unsigned");
			Keywords.Add("unused");
			Keywords.Add("userdefined");
			Keywords.Add("value");
			Keywords.Add("valuetype");
			Keywords.Add("vararg");
			Keywords.Add("variant");
			Keywords.Add("vector");
			Keywords.Add("virtual");
			Keywords.Add("void");
			Keywords.Add("volatile.");
			Keywords.Add("wchar");
			Keywords.Add("winapi");
			Keywords.Add("with");
			Keywords.Add("wrapper");
			Keywords.Add("xor");

			InitializeOpCodeItemsByOpCode();
		}

		public static void Initialize()
		{
			if (!IsInitialized)
			{
				InitializeFields();
				IsInitialized = true;
			}
		}

		public static OpCodeGroup GetGroupOfOpCode(OpCode opCode)
		{
			OpCodeGroup result = OpCodeGroup.Parameterless;

			if (FieldParameter.Contains(opCode))
			{
				result = OpCodeGroup.FieldParameter;
			}
			else if (MethodParameter.Contains(opCode))
			{
				result = OpCodeGroup.MethodParameter;
			}
			else if (StringParameter.Contains(opCode))
			{
				result = OpCodeGroup.StringParameter;
			}
			else if (TypeParameter.Contains(opCode))
			{
				result = OpCodeGroup.TypeParameter;
			}
			else if (SbyteLocationParameter.Contains(opCode))
			{
				result = OpCodeGroup.SbyteLocationParameter;
			}
			else if (IntLocationParameter.Contains(opCode))
			{
				result = OpCodeGroup.IntLocationParameter;
			}
			else if (ByteParameter.Contains(opCode))
			{
				result = OpCodeGroup.ByteParameter;
			}
			else if (UshortParameter.Contains(opCode))
			{
				result = OpCodeGroup.UshortParameter;
			}
			else if (SbyteParameter.Contains(opCode))
			{
				result = OpCodeGroup.SbyteParameter;
			}
			else if (IntParameter.Contains(opCode))
			{
				result = OpCodeGroup.IntParameter;
			}
			else if (LongParameter.Contains(opCode))
			{
				result = OpCodeGroup.LongParameter;
			}
			else if (FloatParameter.Contains(opCode))
			{
				result = OpCodeGroup.FloatParameter;
			}
			else if (DoubleParameter.Contains(opCode))
			{
				result = OpCodeGroup.DoubleParameter;
			}
			else if (ByteArgumentParameter.Contains(opCode))
			{
				result = OpCodeGroup.ByteArgumentParameter;
			}
			else if (UshortArgumentParameter.Contains(opCode))
			{
				result = OpCodeGroup.UshortArgumentParameter;
			}
			else if (ByteVariableParameter.Contains(opCode))
			{
				result = OpCodeGroup.ByteVariableParameter;
			}
			else if (UshortVariableParameter.Contains(opCode))
			{
				result = OpCodeGroup.UshortVariableParameter;
			}
			else if (opCode.Equals(OpCodes.Calli))
			{
				result = OpCodeGroup.Calli;
			}
			else if (opCode.Equals(OpCodes.Switch))
			{
				result = OpCodeGroup.Switch;
			}
			else if (opCode.Equals(OpCodes.Ldtoken))
			{
				result = OpCodeGroup.Ldtoken;
			}

			return result;
		}
	}
}