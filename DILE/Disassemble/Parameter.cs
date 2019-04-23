using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class Parameter : TokenBase, IComparable, ILazyInitialized
	{
		private const string ParamArrayAttribute = "System.ParamArrayAttribute::.ctor()";

		private MethodDefinition method;
		public MethodDefinition Method
		{
			get
			{
				return method;
			}
			private set
			{
				method = value;
			}
		}

		private uint ordinalIndex;
		public uint OrdinalIndex
		{
			get
			{
				return ordinalIndex;
			}
			private set
			{
				ordinalIndex = value;
			}
		}

		private CorParamAttr attributeFlags;
		public CorParamAttr AttributeFlags
		{
			get
			{
				return attributeFlags;
			}
			private set
			{
				attributeFlags = value;
			}
		}

		private CorElementType elementType;
		public CorElementType ElementType
		{
			get
			{
				return elementType;
			}
			private set
			{
				elementType = value;
			}
		}

		private IntPtr defaultValue;
		public IntPtr DefaultValue
		{
			get
			{
				return defaultValue;
			}
			private set
			{
				defaultValue = value;
			}
		}

		private uint defaultValueLength;
		public uint DefaultValueLength
		{
			get
			{
				return defaultValueLength;
			}
			private set
			{
				defaultValueLength = value;
			}
		}

		private string defaultValueAsString = string.Empty;
		public string DefaultValueAsString
		{
			get
			{
				return defaultValueAsString;
			}
			private set
			{
				defaultValueAsString = value;
			}
		}

		private string marshalAsTypeString;
		public string MarshalAsTypeString
		{
			get
			{
				return marshalAsTypeString;
			}
			private set
			{
				marshalAsTypeString = value;
			}
		}

		private List<CustomAttribute> customAttributes;
		public List<CustomAttribute> CustomAttributes
		{
			get
			{
				return customAttributes;
			}
			set
			{
				customAttributes = value;
			}
		}

		[ThreadStatic()]
		private static StringBuilder attributeTextBuilder;
		private static StringBuilder AttributeTextBuilder
		{
			get
			{
				if (attributeTextBuilder == null)
				{
					attributeTextBuilder = new StringBuilder();
				}

				return attributeTextBuilder;
			}
		}

		private bool isLazyInitialized = false;
		private bool IsLazyInitialized
		{
			get
			{
				return isLazyInitialized;
			}
			set
			{
				isLazyInitialized = value;
			}
		}

		public Parameter(IMetaDataImport2 import, Dictionary<uint, TokenBase> allTokens, uint token, MethodDefinition method, uint ordinalIndex, string name, uint attributeFlags, uint elementType, IntPtr defaultValue, uint defaultValueLength)
		{
			Token = token;
			Method = method;
			OrdinalIndex = ordinalIndex;
			Name = name;
			AttributeFlags = (CorParamAttr)attributeFlags;
			ElementType = (CorElementType)elementType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;
			ReadDefaultValue();
		}

		private void ReadDefaultValue()
		{
			if ((AttributeFlags & CorParamAttr.pdHasDefault) == CorParamAttr.pdHasDefault)
			{
				StringBuilder defaultValue = new StringBuilder();
				defaultValue.Append(".param [");
				defaultValue.Append(OrdinalIndex);
				defaultValue.Append("] = ");

				object defaultValueNumber;
				defaultValue.Append(HelperFunctions.ReadDefaultValue(ElementType, DefaultValue, DefaultValueLength, out defaultValueNumber));

				DefaultValueAsString = defaultValue.ToString();
			}
		}

		public void ReadMarshalInformation(IMetaDataImport2 import, Dictionary<uint, TokenBase> allTokens, int parameterCount)
		{
			if ((AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
			{
				MarshalAsTypeString = string.Format("marshal({0})", HelperFunctions.ReadMarshalDescriptor(import, allTokens, Token, parameterCount));
			}
		}

		public string GetAttributeText()
		{
			AttributeTextBuilder.Length = 0;
			CorParamAttr refParam = (CorParamAttr.pdIn | CorParamAttr.pdOut);

			if ((AttributeFlags & refParam) == refParam)
			{
				AttributeTextBuilder.Append("ref ");
			}
			else if ((AttributeFlags & CorParamAttr.pdOut) == CorParamAttr.pdOut)
			{
				AttributeTextBuilder.Append("[out] ");
			}

			if ((AttributeFlags & CorParamAttr.pdIn) == CorParamAttr.pdIn)
			{
				AttributeTextBuilder.Append("[in] ");
			}

			if ((AttributeFlags & CorParamAttr.pdOptional) == CorParamAttr.pdOptional)
			{
				AttributeTextBuilder.Append("[opt] ");
			}

			return AttributeTextBuilder.ToString();
		}

		public int CompareTo(object obj)
		{
			if (obj == null || obj.GetType() != typeof(Parameter))
			{
				throw new NotSupportedException();
			}

			Parameter otherParameter = (Parameter)obj;

			return OrdinalIndex.CompareTo(otherParameter.OrdinalIndex);
		}

		#region ILazyInitialized Members

		public void LazyInitialize(Dictionary<uint, TokenBase> allTokens)
		{
			if (!IsLazyInitialized)
			{
				InnerLazyInitialize(false);
				IsLazyInitialized = true;
			}
		}

		#endregion

		public void OpenMetadataAndInitialize()
		{
			if (!IsLazyInitialized)
			{
				InnerLazyInitialize(true);
				IsLazyInitialized = true;
			}
		}

		private void InnerLazyInitialize(bool openMetadata)
		{
			Assembly assembly = Method.BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				if (openMetadata)
				{
					assembly.OpenMetadataInterfaces();
				}

				CustomAttributes = HelperFunctions.EnumCustomAttributes(assembly.Import, assembly, this);
			}
			finally
			{
				if (openMetadata)
				{
					assembly.CloseMetadataInterfaces();
				}
			}

			if (CustomAttributes != null && CustomAttributes.Count > 0)
			{
				foreach (CustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
				}
			}
		}

		public bool ParamArrayAttributeExists()
		{
			bool result = false;

			OpenMetadataAndInitialize();

			if (CustomAttributes != null && CustomAttributes.Count > 0)
			{
				int index = 0;

				while (!result && index < CustomAttributes.Count)
				{
					CustomAttribute customAttribute = CustomAttributes[index++];

					if (customAttribute.Name.Contains(ParamArrayAttribute))
					{
						result = true;
					}
				}
			}

			return result;
		}
	}
}