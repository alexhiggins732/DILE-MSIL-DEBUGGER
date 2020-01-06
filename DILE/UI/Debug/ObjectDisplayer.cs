using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public class ObjectDisplayer
	{
		private delegate ListViewItem AddFieldValueDelegate<T>(ValueFieldGroup propertyGroup, string fieldName, T value, BaseValueRefresher valueRefresher, TreeNode treeNode, bool reformattableNumber);
		private delegate TreeNode AddObjectToTreeDelegate(TreeNodeCollection parentNodeCollection, string objectName, BaseValueRefresher valueRefresher);

		private ObjectViewer objectViewer;
		private ObjectViewer ObjectViewer
		{
			get
			{
				return objectViewer;
			}
			set
			{
				objectViewer = value;
			}
		}

		//private DebugEventInformation debugEventInformation;
		//public DebugEventInformation DebugEventInformation
		//{
		//  get
		//  {
		//    return debugEventInformation;
		//  }
		//  private set
		//  {
		//    debugEventInformation = value;
		//  }
		//}

		private TreeNodeCollection parentNodeCollection;
		private TreeNodeCollection ParentNodeCollection
		{
			get
			{
				return parentNodeCollection;
			}
			set
			{
				parentNodeCollection = value;
			}
		}

		private AddObjectToTreeDelegate addObjectToTreeMethod;
		private AddObjectToTreeDelegate AddObjectToTreeMethod
		{
			get
			{
				return addObjectToTreeMethod;
			}
			set
			{
				addObjectToTreeMethod = value;
			}
		}

		private List<EvaluationResult> evaluationResults;
		private List<EvaluationResult> EvaluationResults
		{
			get
			{
				return evaluationResults;
			}
			set
			{
				evaluationResults = value;
			}
		}

		private List<uint> overridenMethods = new List<uint>();
		private List<uint> OverridenMethods
		{
			get
			{
				return overridenMethods;
			}
			set
			{
				overridenMethods = value;
			}
		}

		private EvaluationHandler methodCaller;
		private EvaluationHandler MethodCaller
		{
			get
			{
				return methodCaller;
			}
			set
			{
				methodCaller = value;
			}
		}

		private FrameRefresher frameRefresher;
		private FrameRefresher FrameRefresher
		{
			get
			{
				return frameRefresher;
			}
			set
			{
				frameRefresher = value;

				if (frameRefresher == null)
				{
					Frame = null;
				}
				else
				{
					Frame = frameRefresher.GetRefreshedValue();
				}
			}
		}

		private FrameWrapper frame;
		private FrameWrapper Frame
		{
			get
			{
				return frame;
			}
			set
			{
				frame = value;
			}
		}

		//public ObjectDisplayer(FrameRefresher frameRefresher, ObjectViewer objectViewer, DebugEventInformation debugEventInformation, TreeNodeCollection parentNodeCollection)
		//{
		//  FrameRefresher = frameRefresher;
		//  ObjectViewer = objectViewer;
		//  DebugEventInformation = debugEventInformation;
		//  ParentNodeCollection = parentNodeCollection;

		//  //AddObjectToTreeMethod = new AddObjectToTreeDelegate(ObjectViewer.AddObjectToTree);
		//}

		public void DisplayValue(ValueWrapper objectWrapper, BaseValueRefresher valueRefresher)
		{
			DisplayField(valueRefresher, objectWrapper, null, ValueFieldGroup.ObjectInformation, string.Empty, false);
			DisplayEvaluatedResults();
		}

		private void DisplayEvaluatedResults()
		{
			if (EvaluationResults != null)
			{
				foreach (EvaluationResult evaluationResult in EvaluationResults)
				{
					if (evaluationResult.Result == null)
					{
						if (evaluationResult.IsSuccessful)
						{
							AddFieldValue<string>(evaluationResult.Group, evaluationResult.Expression, "<property evaluation timed out>");
						}
						else
						{
							string errorMessage = string.Format("<{0}>", evaluationResult.Exception.Message);
							AddFieldValue<string>(evaluationResult.Group, evaluationResult.Expression, errorMessage);
						}
					}
					else
					{
						DisplayField(evaluationResult.ResultRefresher, evaluationResult.Result, null, evaluationResult.Group, evaluationResult.Expression, true);
					}
				}

				EvaluationResults = null;
			}
		}

		private void AddFieldValue<T>(ValueFieldGroup propertyGroup, string fieldName, T value)
		{
			AddFieldValue<T>(propertyGroup, fieldName, value, null, null, false);
		}

		private void AddFieldValue<T>(ValueFieldGroup propertyGroup, string fieldName, T value, bool reformattableNumber)
		{
			AddFieldValue<T>(propertyGroup, fieldName, value, null, null, reformattableNumber);
		}

		private void AddFieldValue<T>(ValueFieldGroup propertyGroup, string fieldName, T value, BaseValueRefresher valueRefresher, TreeNode treeNode)
		{
			AddFieldValue<T>(propertyGroup, fieldName, value, valueRefresher, treeNode, false);
		}

		private void AddFieldValue<T>(ValueFieldGroup propertyGroup, string fieldName, T value, BaseValueRefresher valueRefresher, TreeNode treeNode, bool reformattableNumber)
		{
			if (fieldName == null || fieldName.Length == 0)
			{
				fieldName = string.Format("<{0} value>", typeof(T).FullName);
			}

			//if (ObjectViewer.IsInvokeRequired)
			//{
			//  AddFieldValueDelegate<T> addFieldValueDelegate = new AddFieldValueDelegate<T>(ObjectViewer.AddFieldValue<T>);

			//  ObjectViewer.BeginInvoke(addFieldValueDelegate, propertyGroup, fieldName, value, valueRefresher, treeNode, reformattableNumber);
			//}
			//else
			//{
			//  ObjectViewer.AddFieldValue<T>(propertyGroup, fieldName, value, valueRefresher, treeNode, reformattableNumber);
			//}
		}

		private TreeNode AddObjectToTree(string objectName, BaseValueRefresher valueRefresher)
		{
			TreeNode result = null;

			//if (ObjectViewer.IsInvokeRequired)
			//{
			//  result = ObjectViewer.Invoke(AddObjectToTreeMethod, ParentNodeCollection, objectName, valueRefresher) as TreeNode;
			//}
			//else
			//{
			//  result = ObjectViewer.AddObjectToTree(ParentNodeCollection, objectName, valueRefresher);
			//}

			return result;
		}

		private TreeNode AddObjectToTree(TreeNodeCollection parentNodeCollection, string objectName, BaseValueRefresher valueRefresher)
		{
			TreeNode result = null;

			//if (ObjectViewer.IsInvokeRequired)
			//{
			//  result = ObjectViewer.Invoke(AddObjectToTreeMethod, parentNodeCollection, objectName, valueRefresher) as TreeNode;
			//}
			//else
			//{
			//  result = ObjectViewer.AddObjectToTree(parentNodeCollection, objectName, valueRefresher);
			//}

			return result;
		}

		private MethodDefinition FindMethodByName(string methodName, TypeDefinition typeDefinition)
		{
			MethodDefinition result = null;

			if (typeDefinition.MethodDefinitions != null && typeDefinition.MethodDefinitions.Count > 0)
			{
				Dictionary<uint, MethodDefinition>.ValueCollection.Enumerator enumerator = typeDefinition.MethodDefinitions.Values.GetEnumerator();

				while (result == null && enumerator.MoveNext())
				{
					if (enumerator.Current.Name == methodName && enumerator.Current.Parameters == null)
					{
						result = enumerator.Current;
					}
				}
			}

			if (result == null)
			{
				//TokenBase tokenObject = HelperFunctions.FindObjectByToken(typeDefinition.BaseTypeToken, typeDefinition.ModuleScope.Assembly.FullPath);

				//if (tokenObject != null)
				//{
				//  Type tokenObjectType = tokenObject.GetType();

				//  if (tokenObjectType == typeof(TypeDefinition))
				//  {
				//    result = FindMethodByName(methodName, (TypeDefinition)tokenObject);
				//  }
				//  else if (tokenObjectType == typeof(TypeReference))
				//  {
				//    TypeReference typeReference = (TypeReference)tokenObject;
				//    TypeDefinition baseTypeDefinition = FindTypeByName(typeReference.FullName, typeReference.ReferencedAssembly);

				//    if (baseTypeDefinition != null)
				//    {
				//      result = FindMethodByName(methodName, baseTypeDefinition);
				//    }
				//  }
				//}
			}

			return result;
		}

		private TypeDefinition FindTypeByName(string typeFullName, string moduleName)
		{
			TypeDefinition result = null;
			try
			{
				moduleName = Path.GetFileNameWithoutExtension(moduleName);
			}
			catch
			{
			}

			int index = 0;
			while (result == null && index < Project.Instance.Assemblies.Count)
			{
				Assembly assembly = Project.Instance.Assemblies[index++];

				if (moduleName == Path.GetFileNameWithoutExtension(assembly.FullPath))
				{
					Dictionary<uint, TypeDefinition>.ValueCollection.Enumerator enumerator = assembly.ModuleScope.TypeDefinitions.Values.GetEnumerator();

					while (result == null && enumerator.MoveNext())
					{
						if (enumerator.Current.FullName == typeFullName)
						{
							result = enumerator.Current;
						}
					}
				}
			}

			return result;
		}

		private void DisplayField(BaseValueRefresher valueRefresher, ValueWrapper objectValue, FieldDefinition field, string fieldName, bool recursiveCall)
		{
			ValueFieldGroup propertyGroup = (ValueFieldGroup)CorFieldAttr.fdPublic;

			if (field != null)
			{
				propertyGroup = (ValueFieldGroup)(field.Flags & CorFieldAttr.fdFieldAccessMask);
			}

			DisplayField(valueRefresher, objectValue, field, propertyGroup, fieldName, recursiveCall);
		}

		private void DisplayField(BaseValueRefresher valueRefresher, ValueWrapper objectValue, FieldDefinition field, ValueFieldGroup propertyGroup, string fieldName, bool recursiveCall)
		{
			if (objectValue == null)
			{
				objectValue = valueRefresher.GetRefreshedValue();
			}

			if (objectValue == null)
			{
				AddFieldValue<string>(propertyGroup, fieldName, "<undefined value>");
			}
			else
			{
				CorElementType elementType = (CorElementType)objectValue.ElementType;

				switch (elementType)
				{
					case CorElementType.ELEMENT_TYPE_BOOLEAN:
						AddFieldValue<bool>(propertyGroup, fieldName, objectValue.GetGenericValue<bool>());
						break;

					case CorElementType.ELEMENT_TYPE_CHAR:
						AddFieldValue<char>(propertyGroup, fieldName, objectValue.GetGenericValue<char>());
						break;

					case CorElementType.ELEMENT_TYPE_I1:
						AddFieldValue<sbyte>(propertyGroup, fieldName, objectValue.GetGenericValue<sbyte>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_I2:
						AddFieldValue<short>(propertyGroup, fieldName, objectValue.GetGenericValue<short>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_I4:
						AddFieldValue<int>(propertyGroup, fieldName, objectValue.GetGenericValue<int>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_I8:
						AddFieldValue<long>(propertyGroup, fieldName, objectValue.GetGenericValue<long>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_U1:
						AddFieldValue<byte>(propertyGroup, fieldName, objectValue.GetGenericValue<byte>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_U2:
						AddFieldValue<ushort>(propertyGroup, fieldName, objectValue.GetGenericValue<ushort>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_U4:
						AddFieldValue<uint>(propertyGroup, fieldName, objectValue.GetGenericValue<uint>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_U8:
						AddFieldValue<ulong>(propertyGroup, fieldName, objectValue.GetGenericValue<ulong>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_I:
						AddFieldValue<int>(propertyGroup, fieldName, objectValue.GetGenericValue<int>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_U:
						AddFieldValue<uint>(propertyGroup, fieldName, objectValue.GetGenericValue<uint>(), true);
						break;

					case CorElementType.ELEMENT_TYPE_R4:
						AddFieldValue<float>(propertyGroup, fieldName, objectValue.GetGenericValue<float>());
						break;

					case CorElementType.ELEMENT_TYPE_R8:
						AddFieldValue<double>(propertyGroup, fieldName, objectValue.GetGenericValue<double>());
						break;

					case CorElementType.ELEMENT_TYPE_STRING:
						ValueWrapper dereferencedValue = objectValue.DereferenceValue();

						if (dereferencedValue == null)
						{
							AddFieldValue<string>(propertyGroup, fieldName, "<undefined value>");
						}
						else
						{
							AddFieldValue<string>(propertyGroup, fieldName, HelperFunctions.ShowEscapeCharacters(dereferencedValue.GetStringValue(), true));
						}
						break;

					case CorElementType.ELEMENT_TYPE_CLASS:
					case CorElementType.ELEMENT_TYPE_OBJECT:
						DisplayObject(valueRefresher, objectValue, recursiveCall, fieldName, propertyGroup);
						break;

					case CorElementType.ELEMENT_TYPE_ARRAY:
					case CorElementType.ELEMENT_TYPE_SZARRAY:
						if (recursiveCall)
						{
							if (objectValue.IsNull())
							{
								AddFieldValue<string>(propertyGroup, fieldName, "<undefined value>");
							}
							else
							{
								TreeNode arrayNode = AddObjectToTree(fieldName, valueRefresher);
								ArrayValueWrapper arrayValue = objectValue.ConvertToArrayValue();
								AddFieldValue<string>(propertyGroup, fieldName, string.Format("<array count={0}>", arrayValue.GetCount()), valueRefresher, arrayNode);
							}
						}
						else
						{
							ArrayValueWrapper arrayValue = objectValue.ConvertToArrayValue();
							bool hasBaseIndicies = arrayValue.HasBaseIndicies();
							List<uint> baseIndicies = null;

							if (hasBaseIndicies)
							{
								baseIndicies = arrayValue.GetBaseIndicies();
							}

							List<uint> dimensions = arrayValue.GetDimensions();
							uint rank = arrayValue.GetRank();
							uint count = arrayValue.GetCount();

							for (uint position = 0; position < count; position++)
							{
								ValueWrapper element = arrayValue.GetElementAtPosition(position);
								string elementName = GetArrayElementIndex(baseIndicies, dimensions, position);
								ArrayElementRefresher elementRefresher = new ArrayElementRefresher(elementName, valueRefresher, position);

								TreeNode elementNode = AddObjectToTree(ParentNodeCollection, elementRefresher.Name, elementRefresher);
								CorElementType arrayElementType = (CorElementType)element.ElementType;
								bool isValueTypeElement = (arrayElementType == CorElementType.ELEMENT_TYPE_VALUETYPE);

								if (arrayElementType == CorElementType.ELEMENT_TYPE_CLASS || arrayElementType == CorElementType.ELEMENT_TYPE_OBJECT || isValueTypeElement)
								{
									ValueWrapper dereferencedElement = (isValueTypeElement ? element : element.DereferenceValue());

									if (dereferencedElement == null)
									{
										AddFieldValue<string>(propertyGroup, elementName, "<undefined value>");
									}
									else
									{
										TypeDefinition typeDefinition = HelperFunctions.GetTypeDefinition(dereferencedElement);

										if (typeDefinition != null)
										{
											AddFieldValue<string>(propertyGroup, elementName, string.Format("<{0} object>", typeDefinition.FullName), elementRefresher, elementNode);
										}
									}
								}
								else
								{
									DisplayField(elementRefresher, element, null, elementName, false);
								}
							}
						}
						break;

					case CorElementType.ELEMENT_TYPE_VALUETYPE:
						DisplayValueType(valueRefresher, objectValue, recursiveCall, fieldName, propertyGroup);
						break;

					case CorElementType.ELEMENT_TYPE_PTR:
						valueRefresher.AddPointerToName();
						dereferencedValue = objectValue.DereferenceValue();

						if (dereferencedValue == null)
						{
							if (objectValue.IsNull())
							{
								AddFieldValue<string>(propertyGroup, fieldName, "<null*>");
							}
							else
							{
								uint hResult = objectValue.GetDereferenceError();

								if (hResult == 0x00131317)
								{
									AddFieldValue<string>(propertyGroup, fieldName, "<void*>");
								}
								else
								{
									Exception exception = Marshal.GetExceptionForHR((int)hResult);

									if (exception == null)
									{
										AddFieldValue<string>(propertyGroup, fieldName, string.Format("Unknown error occurred (HRESULT: {0})", hResult));
									}
									else
									{
										AddFieldValue<string>(propertyGroup, fieldName, exception.Message);
									}
								}
							}
						}
						else
						{
							DisplayField(valueRefresher, dereferencedValue, field, fieldName, recursiveCall);
						}
						break;

					case CorElementType.ELEMENT_TYPE_BYREF:
						dereferencedValue = objectValue.DereferenceValue();

						DisplayField(valueRefresher, dereferencedValue, field, fieldName, recursiveCall);
						break;

#if DEBUG
					default:
						throw new NotImplementedException();
#endif
				}
			}
		}

		private string GetArrayElementIndex(List<uint> baseIndices, List<uint> dimensions, uint position)
		{
			StringBuilder result = new StringBuilder();

			for (int index = dimensions.Count - 1; index >= 0; index--)
			{
				uint lowerIndex = (baseIndices == null ? 0 : baseIndices[index]);
				uint dimension = dimensions[index];
				int padding = Math.Max(Convert.ToString(lowerIndex).Length, Convert.ToString(dimension).Length);

				if (position > dimension)
				{
					result.Insert(0, Convert.ToString((position % dimension) + lowerIndex).PadLeft(padding, '0'));

					position /= dimension;
				}
				else
				{
					result.Insert(0, Convert.ToString(position).PadLeft(padding, '0'));

					position = 0;
				}

				if (index > 0)
				{
					result.Insert(0, ", ");
				}
			}

			result.Insert(0, "[");
			result.Append("]");

			return result.ToString();
		}

		private void DisplayValueType(BaseValueRefresher valueRefresher, ValueWrapper objectValue, bool recursiveCall, string fieldName, ValueFieldGroup propertyGroup)
		{
			DisplayObject(valueRefresher, objectValue, recursiveCall, fieldName, propertyGroup, true);
		}

		private void DisplayObject(BaseValueRefresher valueRefresher, ValueWrapper objectValue, bool recursiveCall, string fieldName, ValueFieldGroup propertyGroup)
		{
			DisplayObject(valueRefresher, objectValue, recursiveCall, fieldName, propertyGroup, false);
		}

		private void DisplayObject(BaseValueRefresher objectValueRefresher, ValueWrapper objectValue, bool recursiveCall, string fieldName, ValueFieldGroup propertyGroup, bool isValueType)
		{
			ValueWrapper derefencedObject = (isValueType ? objectValue : objectValue.DereferenceValue());

			if (derefencedObject == null)
			{
				AddFieldValue<string>(propertyGroup, fieldName, "<undefined value>");
			}
			else
			{
				if (derefencedObject.IsBoxedValue())
				{
					derefencedObject = derefencedObject.UnboxValue();
				}

				ClassWrapper classInformation = derefencedObject.GetClassInformation();
				ModuleWrapper module = classInformation.GetModule();
				uint classToken = classInformation.GetToken();

				TokenBase classDefinition = HelperFunctions.FindObjectByToken(classToken, module);

				if (classDefinition is TypeDefinition)
				{
					TypeDefinition typeDefinition = (TypeDefinition)classDefinition;

					if (recursiveCall)
					{
						TreeNode objectTreeNode = AddObjectToTree(fieldName, objectValueRefresher);

						AddFieldValue<string>(propertyGroup, fieldName, string.Format("<{0} object>", typeDefinition.FullName), objectValueRefresher, objectTreeNode);
					}
					else
					{
						DisplayObjectFields(objectValueRefresher, derefencedObject, module, module.GetName(), typeDefinition);
						DisplayObjectProperties(objectValueRefresher, objectValue, module, module.GetName(), typeDefinition);

						MethodDefinition toStringMethod = FindMethodByName("ToString", typeDefinition);

						if (toStringMethod != null)
						{
							List<ValueWrapper> arguments = new List<ValueWrapper>();
							arguments.Add(objectValue);

							CallMethodOnObject(module, toStringMethod, arguments, "ToString()", ValueFieldGroup.ObjectInformation, objectValueRefresher);
						}
					}
				}
				else
				{
					objectValueRefresher.MissingModule = new MissingModule(module);

					AddFieldValue<string>(ValueFieldGroup.ObjectInformation, "Evaluating error", string.Format("The {0} assembly which contains the definition of the requested type (token: 0x{1}) has not been loaded by DILE.", module.GetName(), HelperFunctions.FormatAsHexNumber(classToken, 8)), objectValueRefresher, null);
				}
			}
		}

		private void DisplayObjectFields(BaseValueRefresher valueRefresher, ValueWrapper derefencedObject, ModuleWrapper module, string moduleName, TypeDefinition typeDefinition)
		{
			while (typeDefinition != null)
			{
				if (typeDefinition.FieldDefinitions != null && typeDefinition.FieldDefinitions.Values != null)
				{
					ClassWrapper classInformation = module.GetClass(typeDefinition.Token);

					foreach (FieldDefinition fieldDefinition in typeDefinition.FieldDefinitions.Values)
					{
						if ((fieldDefinition.Flags & CorFieldAttr.fdLiteral) == CorFieldAttr.fdLiteral)
						{
							if (fieldDefinition.IsReformattableDefaultValue())
							{
								AddFieldValue<object>((ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask), fieldDefinition.Name, fieldDefinition.DefaultValueNumber, true);
							}
							else
							{
								AddFieldValue<string>((ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask), fieldDefinition.Name, fieldDefinition.GetFormattedDefaultValue(), false);
							}
						}
						else
						{
							ValueWrapper fieldValue = null;
							BaseValueRefresher fieldValueRefresher = null;

							if ((fieldDefinition.Flags & CorFieldAttr.fdStatic) == CorFieldAttr.fdStatic)
							{
								try
								{
									fieldValue = classInformation.GetStaticFieldValue(fieldDefinition.Token, Frame);
									fieldValueRefresher = new StaticFieldValueRefresher(fieldDefinition.Name, classInformation, FrameRefresher, fieldDefinition.Token);
								}
								catch (Exception exception)
								{
									AddFieldValue<string>((ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask), fieldDefinition.Name, string.Format("<{0}>", exception.Message));
									UIHandler.Instance.ShowException(exception);
								}
							}
							else
							{
								try
								{
									fieldValue = derefencedObject.GetFieldValue(classInformation, fieldDefinition.Token);
									fieldValueRefresher = new FieldValueRefresher(fieldDefinition.Name, valueRefresher, classInformation, fieldDefinition.Token);
								}
								catch (Exception exception)
								{
									AddFieldValue<string>((ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask), fieldDefinition.Name, string.Format("<{0}>", exception.Message));
									UIHandler.Instance.ShowException(exception);
								}
							}

							if (fieldValue != null)
							{
								CorElementType fieldElementType = (CorElementType)fieldValue.ElementType;

								if ((fieldElementType == CorElementType.ELEMENT_TYPE_CLASS) || (fieldElementType == CorElementType.ELEMENT_TYPE_OBJECT))
								{
									string objectValueName = "<undefined value>";
									TreeNode fieldNode = null;

									if (fieldValue.DereferenceValue() != null)
									{
										objectValueName = string.Format("<{0} object>", fieldDefinition.FieldTypeName);
										fieldNode = AddObjectToTree(fieldDefinition.Name, fieldValueRefresher);
									}

									AddFieldValue<string>((ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask), fieldDefinition.Name, objectValueName, fieldValueRefresher, fieldNode);
								}
								else
								{
									DisplayField(fieldValueRefresher, fieldValue, fieldDefinition, fieldDefinition.Name, true);
								}
							}
						}
					}
				}

				TokenBase baseType = HelperFunctions.FindObjectByToken(typeDefinition.BaseTypeToken, module);
				typeDefinition = (baseType == null ? null : baseType as TypeDefinition);
			}
		}

		private void DisplayObjectProperties(BaseValueRefresher objectValueRefresher, ValueWrapper objectValue, ModuleWrapper module, string moduleName, TypeDefinition typeDefinition)
		{
			while (typeDefinition != null)
			{
				if (typeDefinition.Properties != null && typeDefinition.Properties.Values != null)
				{
					ClassWrapper classInformation = module.GetClass(typeDefinition.Token);

					foreach (Property property in typeDefinition.Properties.Values)
					{
						if (typeDefinition.ModuleScope.Assembly.AllTokens.ContainsKey(property.GetterMethodToken))
						{
							MethodDefinition getterMethod = typeDefinition.ModuleScope.Assembly.AllTokens[property.GetterMethodToken] as MethodDefinition;

							if (getterMethod != null && (getterMethod.Flags & CorMethodAttr.mdAbstract) != CorMethodAttr.mdAbstract && (getterMethod.CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS && getterMethod.Parameters == null)
							{
								List<ValueWrapper> arguments = new List<ValueWrapper>();
								arguments.Add(objectValue);
								ValueFieldGroup propertyGroup = (ValueFieldGroup)(getterMethod.Flags & CorMethodAttr.mdMemberAccessMask);

								CallMethodOnObject(module, getterMethod, arguments, property.Name, propertyGroup, objectValueRefresher);
							}
						}
					}
				}

				TokenBase baseType = HelperFunctions.FindObjectByToken(typeDefinition.BaseTypeToken, module);
				typeDefinition = (baseType == null ? null : baseType as TypeDefinition);
			}
		}

		private void CallMethodOnObject(ModuleWrapper module, MethodDefinition methodToCall, List<ValueWrapper> arguments, string methodName, ValueFieldGroup propertyGroup, BaseValueRefresher objectValueRefresher)
		{
			if (!OverridenMethods.Contains(methodToCall.Token))
			{
				FunctionWrapper methodToCallWrapper = module.GetFunction(methodToCall.Token);

				if (methodToCallWrapper != null)
				{
					if (methodToCall.Overrides != 0)
					{
						OverridenMethods.Add(methodToCall.Overrides);
					}

					EvaluationResult methodCallResult = new EvaluationResult(methodName, propertyGroup);
					methodCallResult.ResultRefresher = objectValueRefresher;
					EvaluationHandler evaluationHandler = new EvaluationHandler(FrameRefresher);

					methodCallResult.LoadBaseEvaluationResult(evaluationHandler.CallFunction(methodToCallWrapper, arguments));
					methodCallResult.ResultRefresher = new PropertyValueRefresher(methodCallResult.Expression, MethodCaller, methodToCallWrapper, arguments, methodCallResult.ResultRefresher);

					if (EvaluationResults == null)
					{
						EvaluationResults = new List<EvaluationResult>();
					}

					EvaluationResults.Add(methodCallResult);
				}
			}
		}
	}
}