using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Dile.UI.Debug
{
	public class FunctionBreakpointInformation : BreakpointInformation, IXmlSerializable
	{
		private string module;
		private string Module
		{
			get
			{
				return module;
			}
			set
			{
				module = value;
			}
		}

		private uint methodToken;
		private uint MethodToken
		{
			get
			{
				return methodToken;
			}
			set
			{
				methodToken = value;
			}
		}

		private MethodDefinition methodDefinition;
		public MethodDefinition MethodDefinition
		{
			get
			{
				return methodDefinition;
			}
			private set
			{
				methodDefinition = value;

				if (methodDefinition != null)
				{
					MethodToken = methodDefinition.Token;
				}
			}
		}

		private uint offset;
		public uint Offset
		{
			get
			{
				return offset;
			}
			private set
			{
				offset = value;
			}
		}

		private List<FunctionBreakpointWrapper> breakpoints = new List<FunctionBreakpointWrapper>();
		public List<FunctionBreakpointWrapper> Breakpoints
		{
			get
			{
				return breakpoints;
			}
			set
			{
				breakpoints = value;
			}
		}

		public override string DisplayName
		{
			get
			{
				string result = string.Empty;

				if (MethodDefinition != null)
				{
					result = string.Format("{0}::{1}", MethodDefinition.BaseTypeDefinition.FullName, MethodDefinition.DisplayName);
				}

				return result;
			}
		}

		public override string OffsetValue
		{
			get
			{
				return string.Format("IL_{0}", HelperFunctions.FormatAsHexNumber(Offset, 4));
			}
		}

		public FunctionBreakpointInformation()
		{
		}

		public FunctionBreakpointInformation(MethodDefinition methodDefinition, uint offset)
		{
			MethodDefinition = methodDefinition;
			Offset = offset;

			Module = MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.FullPath;
		}

		protected override void Activate(bool active)
		{
			foreach (FunctionBreakpointWrapper breakpoint in Breakpoints)
			{
				if (breakpoint.IsActive() != active)
				{
					breakpoint.Activate(active);
				}
			}
		}

		public override void NavigateTo()
		{
			CodeObjectDisplayOptions displayOptions = new CodeObjectDisplayOptions();
			displayOptions.NavigateToOffset = Offset;

			UIHandler.Instance.ShowCodeObject(MethodDefinition, displayOptions);
		}

		public override void Reset()
		{
			Breakpoints.Clear();
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			XmlSchema result = new XmlSchema();

			XmlSchemaAttribute moduleAttribute = new XmlSchemaAttribute();
			moduleAttribute.SchemaType = XmlSchemaSimpleType.GetBuiltInSimpleType(XmlTypeCode.String);
			moduleAttribute.Name = "Module";
			result.Items.Add(moduleAttribute);

			XmlSchemaAttribute methodTokenAttribute = new XmlSchemaAttribute();
			methodTokenAttribute.SchemaType = XmlSchemaSimpleType.GetBuiltInSimpleType(XmlTypeCode.UnsignedInt);
			methodTokenAttribute.Name = "MethodToken";
			result.Items.Add(methodTokenAttribute);

			XmlSchemaAttribute offsetAttribute = new XmlSchemaAttribute();
			offsetAttribute.SchemaType = XmlSchemaSimpleType.GetBuiltInSimpleType(XmlTypeCode.UnsignedInt);
			offsetAttribute.Name = "Offset";
			result.Items.Add(offsetAttribute);

			return result;
		}

		public void ReadXml(XmlReader reader)
		{
			Module = reader.GetAttribute("Module");
			MethodToken = Convert.ToUInt32(reader.GetAttribute("MethodToken"));
			Offset = Convert.ToUInt32(reader.GetAttribute("Offset"));
			reader.Read();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Module", Module);
			writer.WriteAttributeString("MethodToken", Convert.ToString(MethodToken));
			writer.WriteAttributeString("Offset", Convert.ToString(Offset));
		}

		#endregion

		public bool AssociateWithMethod()
		{
			if (string.IsNullOrEmpty(Module))
			{
				MethodDefinition = HelperFunctions.FindObjectByToken(MethodToken) as MethodDefinition;
				if (MethodDefinition != null)
				{
					Module = MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.FullPath;
				}
			}
			else
			{
				MethodDefinition = HelperFunctions.FindObjectByToken(MethodToken, Module, false) as MethodDefinition;
			}

			return (MethodDefinition != null);
		}
	}
}