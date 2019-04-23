using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Windows.Forms;

namespace Dile.UI
{
	public static class TreeViewSearcher
	{
		private static TreeNode FindNodeByName(TreeNodeCollection nodes, string text)
		{
			TreeNode result = null;
			int index = 0;

			while (result == null && index < nodes.Count)
			{
				TreeNode node = nodes[index++];

				if (node.Text == text)
				{
					result = node;
				}
			}

			return result;
		}

		private static TreeNode SearchTokenNode(TreeNode parentNode, TokenBase parentTokenObject, string subNodeText, string nodeText)
		{
			TreeNode result = SearchNodes(parentNode, parentTokenObject);

			if (subNodeText != null && result != null)
			{
				result.Expand();
				result = FindNodeByName(result.Nodes, subNodeText);
			}

			if (result != null)
			{
				result.Expand();
				result = FindNodeByName(result.Nodes, nodeText);
			}

			return result;
		}

		private static TreeNode SearchNodes(TreeNode parentNode, TokenBase tokenObject)
		{
			TreeNode result = null;

			if (parentNode != null)
			{
				switch (tokenObject.ItemType)
				{
					case SearchOptions.Assembly:
						Assembly assembly = (Assembly)tokenObject;
						parentNode.Expand();
						result = FindNodeByName(parentNode.Nodes, assembly.FileName);
						break;

					case SearchOptions.AssemblyReference:
						AssemblyReference assemblyReference = (AssemblyReference)tokenObject;

						result = SearchTokenNode(parentNode, assemblyReference.Assembly, " References", assemblyReference.Name);
						break;

					case SearchOptions.FieldDefintion:
						FieldDefinition fieldDefinition = (FieldDefinition)tokenObject;

						result = SearchTokenNode(parentNode, fieldDefinition.BaseTypeDefinition, "Fields", fieldDefinition.Name);
						break;

					case SearchOptions.File:
						File file = (File)tokenObject;

						result = SearchTokenNode(parentNode, file.Assembly, " Files", file.Name);
						break;

					case SearchOptions.ManifestResource:
						ManifestResource manifestResource = (ManifestResource)tokenObject;

						result = SearchTokenNode(parentNode, manifestResource.Assembly, " Manifest Resources", manifestResource.Name);
						break;

					case SearchOptions.MethodDefinition:
						MethodDefinition methodDefinition = (MethodDefinition)tokenObject;

						if (methodDefinition.OwnerProperty != null)
						{
							result = SearchNodes(parentNode, methodDefinition.OwnerProperty);
							result.Expand();
							result = FindNodeByName(result.Nodes, methodDefinition.DisplayName);
						}
						else if (methodDefinition.OwnerEventDefinition != null)
						{
							result = SearchNodes(parentNode, methodDefinition.OwnerEventDefinition);
							result.Expand();
							result = FindNodeByName(result.Nodes, methodDefinition.DisplayName);
						}
						else
						{
							result = SearchTokenNode(parentNode, methodDefinition.BaseTypeDefinition, "Methods", methodDefinition.DisplayName);
						}
						break;

					case SearchOptions.ModuleReference:
						ModuleReference moduleReference = (ModuleReference)tokenObject;

						result = SearchTokenNode(parentNode, moduleReference.Assembly, " Module References", moduleReference.Name);
						break;

					case SearchOptions.ExportedType:
						ExportedType exportedType = (ExportedType)tokenObject;

						result = SearchTokenNode(parentNode, exportedType.Assembly, " Exported Types", exportedType.Name);
						break;

					case SearchOptions.ModuleScope:
						ModuleScope moduleScope = (ModuleScope)tokenObject;

						result = SearchTokenNode(parentNode, moduleScope.Assembly, null, moduleScope.Name);
						break;

					case SearchOptions.Property:
						Property property = (Property)tokenObject;

						result = SearchTokenNode(parentNode, property.BaseTypeDefinition, "Properties", property.Name);
						break;

					case SearchOptions.EventDefinition:
						EventDefinition eventDefinition = (EventDefinition)tokenObject;

						result = SearchTokenNode(parentNode, eventDefinition.BaseTypeDefinition, "Events", eventDefinition.Name);
						break;

					case SearchOptions.TypeDefinition:
						TypeDefinition typeDefinition = (TypeDefinition)tokenObject;
						string typeNamespace = typeDefinition.Namespace;

						if (typeNamespace.Length == 0)
						{
							typeNamespace = Constants.DefaultNamespaceName;
						}

						result = SearchTokenNode(parentNode, typeDefinition.ModuleScope, typeNamespace, typeDefinition.FullName);
						break;
				}
			}

			return result;
		}

		public static TreeNode LocateNode(TreeNode startingNode, TokenBase tokenObject)
		{
			return SearchNodes(startingNode, tokenObject);
		}
	}
}