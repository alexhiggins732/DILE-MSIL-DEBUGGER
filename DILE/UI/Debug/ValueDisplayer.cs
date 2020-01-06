using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public class ValueDisplayer
	{
		public event StateChangingDelegate StateChanging;
		public event TypeInspectedDelegate TypeInspected;
		public event ArrayElementEvaluatedDelegate ArrayElementEvaluated;
		public event StringValueEvaluatedDelegate StringValueEvaluated;
		public event FieldEvaluatedDelegate FieldEvaluated;
		public event PropertyEvaluatedDelegate PropertyEvaluated;
		public event ToStringEvaluatedDelegate ToStringEvaluated;
		public event EvaluatedNullDelegate EvaluatedNull;
		public event ErrorOccurredDelegate ErrorOccurred;

		private ReaderWriterLock lockObject = new ReaderWriterLock();
		private ReaderWriterLock LockObject
		{
			get
			{
				return lockObject;
			}
			set
			{
				lockObject = value;
			}
		}

		private EvaluationContext evaluationContext;
		private EvaluationContext EvaluationContext
		{
			get
			{
				return evaluationContext;
			}
			set
			{
				evaluationContext = value;
			}
		}

		private DebugExpressionResult debugValue;
		private DebugExpressionResult DebugValue
		{
			get
			{
				return debugValue;
			}
			set
			{
				debugValue = value;
			}
		}

		private BaseValueRefresher debugValueRefresher;
		private BaseValueRefresher DebugValueRefresher
		{
			get
			{
				return debugValueRefresher;
			}
			set
			{
				debugValueRefresher = value;
			}
		}

		private ValueWrapper derefencedValueWrapper;
		public ValueWrapper DerefencedValueWrapper
		{
			get
			{
				if (derefencedValueWrapper == null)
				{
					derefencedValueWrapper = DebugValue.ResultValue.DereferenceValue();
				}

				return derefencedValueWrapper;
			}
		}

		private ValueDisplayerCancelReason cancelReason;
		private ValueDisplayerCancelReason CancelReason
		{
			get
			{
				LockObject.AcquireReaderLock(-1);
				ValueDisplayerCancelReason value;

				try
				{
					value = cancelReason;
				}
				finally
				{
					LockObject.ReleaseReaderLock();
				}

				return value;
			}
			set
			{
				LockObject.AcquireWriterLock(-1);

				try
				{
					cancelReason = value;
				}
				finally
				{
					LockObject.ReleaseWriterLock();
				}
			}
		}

		private bool CancelEvaluation
		{
			get
			{
				return (CancelReason != ValueDisplayerCancelReason.None);
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
			}
		}

		private FrameWrapper frame;
		private FrameWrapper Frame
		{
			get
			{
				if (frame == null)
				{
					frame = FrameRefresher.GetRefreshedValue();
				}

				return frame;
			}
			set
			{
				frame = null;
			}
		}

		private MethodDefinition debugValueToStringMethod;
		private MethodDefinition DebugValueToStringMethod
		{
			get
			{
				return debugValueToStringMethod;
			}
			set
			{
				debugValueToStringMethod = value;
			}
		}

		private List<FieldDefinition> debugValueFields;
		private List<FieldDefinition> DebugValueFields
		{
			get
			{
				return debugValueFields;
			}
			set
			{
				debugValueFields = value;
			}
		}

		private Dictionary<string, Property> debugValueProperties;
		private Dictionary<string, Property> DebugValueProperties
		{
			get
			{
				return debugValueProperties;
			}
			set
			{
				debugValueProperties = value;
			}
		}

		private TreeNode parentNode;
		public TreeNode ParentNode
		{
			get
			{
				return parentNode;
			}
			set
			{
				parentNode = value;
			}
		}

		private bool isEvaluationRunning;
		public bool IsEvaluationRunning
		{
			get
			{
				LockObject.AcquireReaderLock(-1);
				bool value;

				try
				{
					value = isEvaluationRunning;
				}
				finally
				{
					LockObject.ReleaseReaderLock();
				}

				return value;
			}
			private set
			{
				LockObject.AcquireWriterLock(-1);

				try
				{
					isEvaluationRunning = value;
				}
				finally
				{
					LockObject.ReleaseWriterLock();
				}
			}
		}

		public ValueDisplayer(EvaluationContext evaluationContext)
		{
			EvaluationContext = evaluationContext;
		}

		public IValueFormatter CreateSimpleFormatter(ValueWrapper valueWrapper)
		{
			return CreateSimpleFormatter(new DebugExpressionResult(EvaluationContext, valueWrapper));
		}

		public IValueFormatter CreateSimpleFormatter(DebugExpressionResult debugValue)
		{
			IValueFormatter result = null;

			if (debugValue.ResultValue == null)
			{
				result = new StringValueFormatter("<undefined value>");
			}
			else if ((CorElementType)debugValue.ResultValue.ElementType != CorElementType.ELEMENT_TYPE_VALUETYPE && HelperFunctions.HasValueClass(debugValue.ResultValue) && debugValue.ResultValue.IsNull())
			{
				try
				{
					if (debugValue.ResultClass == null)
					{
						result = new StringValueFormatter("<null value>");
					}
					else
					{
						TypeDefinition debugValueTypeDef = HelperFunctions.FindTypeOfValue(EvaluationContext, debugValue);
						result = new StringValueFormatter(string.Format("<{0} object (null value)>", debugValueTypeDef.FullName));
					}
				}
				catch (MissingModuleException missingModuleException)
				{
					result = new MissingModuleFormatter(missingModuleException.MissingModule);
				}
			}
			else
			{
				CorElementType debugValueElementType = (CorElementType)debugValue.ResultValue.ElementType;

				switch (debugValueElementType)
				{
					case CorElementType.ELEMENT_TYPE_BOOLEAN:
						result = new BoolValueFormatter(debugValue.ResultValue.GetGenericValue<bool>());
						break;

					case CorElementType.ELEMENT_TYPE_CHAR:
						result = new CharValueFormatter(debugValue.ResultValue.GetGenericValue<char>());
						break;

					case CorElementType.ELEMENT_TYPE_I:
						result = new NumberValueFormatter<int>(debugValue.ResultValue.GetGenericValue<int>());
						break;

					case CorElementType.ELEMENT_TYPE_I1:
						result = new NumberValueFormatter<sbyte>(debugValue.ResultValue.GetGenericValue<sbyte>());
						break;

					case CorElementType.ELEMENT_TYPE_I2:
						result = new NumberValueFormatter<short>(debugValue.ResultValue.GetGenericValue<short>());
						break;

					case CorElementType.ELEMENT_TYPE_I4:
						result = new NumberValueFormatter<int>(debugValue.ResultValue.GetGenericValue<int>());
						break;

					case CorElementType.ELEMENT_TYPE_I8:
						result = new NumberValueFormatter<long>(debugValue.ResultValue.GetGenericValue<long>());
						break;

					case CorElementType.ELEMENT_TYPE_U:
						result = new NumberValueFormatter<uint>(debugValue.ResultValue.GetGenericValue<uint>());
						break;

					case CorElementType.ELEMENT_TYPE_U1:
						result = new NumberValueFormatter<byte>(debugValue.ResultValue.GetGenericValue<byte>());
						break;

					case CorElementType.ELEMENT_TYPE_U2:
						result = new NumberValueFormatter<ushort>(debugValue.ResultValue.GetGenericValue<ushort>());
						break;

					case CorElementType.ELEMENT_TYPE_U4:
						result = new NumberValueFormatter<uint>(debugValue.ResultValue.GetGenericValue<uint>());
						break;

					case CorElementType.ELEMENT_TYPE_U8:
						result = new NumberValueFormatter<ulong>(debugValue.ResultValue.GetGenericValue<ulong>());
						break;

					case CorElementType.ELEMENT_TYPE_R4:
						result = new NumberValueFormatter<float>(debugValue.ResultValue.GetGenericValue<float>());
						break;

					case CorElementType.ELEMENT_TYPE_R8:
						result = new NumberValueFormatter<double>(debugValue.ResultValue.GetGenericValue<double>());
						break;

					case CorElementType.ELEMENT_TYPE_STRING:
						ValueWrapper dereferencedString = debugValue.ResultValue.DereferenceValue();
						result = new StringValueFormatter(HelperFunctions.ShowEscapeCharacters(dereferencedString.GetStringValue(), true));
						result.IsComplexType = true;
						break;

					case CorElementType.ELEMENT_TYPE_CLASS:
					case CorElementType.ELEMENT_TYPE_OBJECT:
					case CorElementType.ELEMENT_TYPE_VALUETYPE:
						try
						{
							string debugValueTypeName = string.Empty;
							TypeTreeNode debugValueTypeTree = HelperFunctions.GetValueTypeTree(EvaluationContext, debugValue.ResultValue);

							if (debugValueTypeTree == null)
							{
								TypeDefinition debugValueTypeDef = HelperFunctions.FindTypeOfValue(EvaluationContext, debugValue);
								debugValueTypeName = debugValueTypeDef.FullName;
							}
							else
							{
								debugValueTypeName = debugValueTypeTree.GetTreeAsString();
							}

							result = new StringValueFormatter(string.Format("<{0} object>", debugValueTypeName));
							result.IsComplexType = true;
						}
						catch (MissingModuleException missingModuleException)
						{
							result = new MissingModuleFormatter(missingModuleException.MissingModule);
						}
						break;

					case CorElementType.ELEMENT_TYPE_ARRAY:
					case CorElementType.ELEMENT_TYPE_SZARRAY:
						ArrayValueWrapper arrayWrapper = debugValue.ResultValue.ConvertToArrayValue();
						result = new ArrayValueFormatter(arrayWrapper.GetCount());
						result.IsComplexType = true;
						break;

					case CorElementType.ELEMENT_TYPE_PTR:
						ValueWrapper dereferencedValue = debugValue.ResultValue.DereferenceValue();

						if (dereferencedValue == null)
						{
							if (dereferencedValue.IsNull())
							{
								result = new StringValueFormatter("<null*>");
							}
							else
							{
								uint hResult = debugValue.ResultValue.GetDereferenceError();

								if (hResult == 0x00131317)
								{
									result = new StringValueFormatter("<void*>");
								}
								else
								{
									Exception exception = Marshal.GetExceptionForHR((int)hResult);

									if (exception == null)
									{
										result = new StringValueFormatter(string.Format("Unknown error occurred (HRESULT: {0})", hResult));
									}
									else
									{
										result = new StringValueFormatter(exception.Message);
									}
								}
							}
						}
						else
						{
							result = CreateSimpleFormatter(dereferencedValue);
							result.AddPointerPrefix();
						}
						break;

					case CorElementType.ELEMENT_TYPE_BYREF:
						result = CreateSimpleFormatter(debugValue.ResultValue.DereferenceValue());
						break;

					default:
						result = new ErrorValueFormatter(string.Format("Unable to display type: {0}", Enum.GetName(typeof(CorElementType), debugValueElementType)));
						break;
				}
			}

			return result;
		}

		public void CreateComplexFormatter(DebugExpressionResult debugValue, BaseValueRefresher debugValueRefresher, FrameRefresher frameRefresher, TreeNode parentNode)
		{
			if (!HelperFunctions.HasValueClass(debugValue.ResultValue))
			{
				throw new ArgumentException("The value has no class.", "debugValue");
			}

			if (IsEvaluationRunning)
			{
				throw new InvalidOperationException("An evaluation is already running. Create a new ValueDisplayer object to start a new one.");
			}

			if (debugValue.ResultValue == null || (debugValue.ResultValue.ElementType != (int)CorElementType.ELEMENT_TYPE_VALUETYPE && debugValue.ResultValue.IsNull()))
			{
				if (EvaluatedNull != null)
				{
					StringValueFormatter nullValueFormatter = new StringValueFormatter("The object is null.");
					nullValueFormatter.Name = debugValueRefresher.Name;
					nullValueFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;

					EvaluatedNull(this, nullValueFormatter);
				}

				if (StateChanging != null)
				{
					StateChanging(this, ValueDisplayerState.Finish, 1);
				}
			}
			else
			{
				if (StateChanging != null)
				{
					StateChanging(this, ValueDisplayerState.Initialize, 1);
				}

				IsEvaluationRunning = true;
				DebugValue = debugValue;
				DebugValueRefresher = debugValueRefresher;
				FrameRefresher = frameRefresher;
				ParentNode = parentNode;

				Thread evaluationThread = new Thread(new ThreadStart(Start));
				evaluationThread.Name = "ValueDisplayer evaluation thread";
				evaluationThread.Priority = ThreadPriority.BelowNormal;

				if (StateChanging != null)
				{
					StateChanging(this, ValueDisplayerState.StartThread, 1);
				}

				evaluationThread.Start();
			}
		}

		public void InterruptEvaluation()
		{
			CancelReason = ValueDisplayerCancelReason.Interrupted;
		}

		private void Start()
		{
			DebugValueFields = new List<FieldDefinition>();
			DebugValueProperties = new Dictionary<string, Property>();

			try
			{
				if (DebugValue.ResultValue.ElementType == (int)CorElementType.ELEMENT_TYPE_ARRAY || DebugValue.ResultValue.ElementType == (int)CorElementType.ELEMENT_TYPE_SZARRAY)
				{
					StartEvaluateArray();
				}
				else
				{
					StartEvaluateObject();
				}
			}
			catch (MissingModuleException missingModuleException)
			{
				if (ErrorOccurred != null && !CancelEvaluation)
				{
					ErrorOccurred(this, new MissingModuleFormatter(missingModuleException.MissingModule));
				}
			}
			catch (Exception exception)
			{
				if (ErrorOccurred != null && !CancelEvaluation)
				{
					ErrorOccurred(this, new ErrorValueFormatter(exception.Message));
				}
			}

			if (StateChanging != null)
			{
				if (CancelEvaluation)
				{
					switch (CancelReason)
					{
						case ValueDisplayerCancelReason.Interrupted:
							StateChanging(this, ValueDisplayerState.Interrupted, 1);
							break;

						case ValueDisplayerCancelReason.MethodCallAbortFailed:
							StateChanging(this, ValueDisplayerState.MethodCallAbortFailed, 0);
							break;
					}
				}
				else
				{
					StateChanging(this, ValueDisplayerState.Finish, 1);
				}
			}

			DebugValueFields = null;
			DebugValueProperties = null;
			DebugValueToStringMethod = null;
			DebugValue = null;
			FrameRefresher = null;
			Frame = null;
			ParentNode = null;
			IsEvaluationRunning = false;
			CancelReason = ValueDisplayerCancelReason.None;
		}

		private void StartEvaluateArray()
		{
			ArrayValueWrapper arrayValue = DebugValue.ResultValue.ConvertToArrayValue();
			bool hasBaseIndicies = arrayValue.HasBaseIndicies();
			List<uint> baseIndicies = null;

			if (hasBaseIndicies)
			{
				baseIndicies = arrayValue.GetBaseIndicies();
			}

			List<uint> dimensions = arrayValue.GetDimensions();
			uint rank = arrayValue.GetRank();
			uint count = arrayValue.GetCount();
			uint elementIndex = 0;

			if (StateChanging != null && !CancelEvaluation)
			{
				StateChanging(this, ValueDisplayerState.EvaluateArrayElements, Convert.ToInt32(count));
			}

			while (!CancelEvaluation && elementIndex < count)
			{
				ValueWrapper element = arrayValue.GetElementAtPosition(elementIndex);
				IValueFormatter elementFormatter = CreateSimpleFormatter(element);
				elementFormatter.Name = GetArrayElementIndex(baseIndicies, dimensions, elementIndex);

				if (elementFormatter.IsComplexType)
				{
					elementFormatter.ValueRefresher = new ArrayElementRefresher(elementFormatter.Name, DebugValueRefresher, elementIndex);
				}

				elementFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;

				if (ArrayElementEvaluated != null)
				{
					ArrayElementEvaluated(this, elementIndex, elementFormatter);
				}

				elementIndex++;
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

		private void StartEvaluateObject()
		{
			if (StateChanging != null && !CancelEvaluation)
			{
				StateChanging(this, ValueDisplayerState.CollectTypeInformation, 1);
			}

			TypeDefinition debugValueTypeDef = HelperFunctions.FindTypeOfValue(EvaluationContext, DebugValue);
			TypeDefinition stringTypeDef = HelperFunctions.FindTypeByName(Constants.StringTypeName, Constants.MscorlibName);

			if (stringTypeDef == null && ErrorOccurred != null)
			{
				ModuleWrapper mscorlibModule = HelperFunctions.FindModuleByNameWithoutExtension(EvaluationContext, Constants.MscorlibName);

				if (mscorlibModule == null)
				{
					ErrorOccurred(this, new ErrorValueFormatter(string.Format("The '{0}' module is not loaded by the current thread.", Constants.MscorlibName)));
				}
				else
				{
					ErrorOccurred(this, new MissingModuleFormatter(new MissingModule(mscorlibModule)));
				}
			}

			CollectTypeInformation(debugValueTypeDef, stringTypeDef);

			if (DebugValue.ResultValue.ElementType == (int)CorElementType.ELEMENT_TYPE_STRING)
			{
				if (StateChanging != null && !CancelEvaluation)
				{
					StateChanging(this, ValueDisplayerState.EvaluateStringValue, 1);
				}

				EvaluateStringValue();
			}

			if (StateChanging != null && !CancelEvaluation)
			{
				StateChanging(this, ValueDisplayerState.EvaluateFields, DebugValueFields.Count);
			}

			EvaluateFields();

			if (EvaluationContext.MethodCallsEnabled)
			{
				if (StateChanging != null && !CancelEvaluation)
				{
					StateChanging(this, ValueDisplayerState.EvaluateProperties, DebugValueProperties.Count);
				}

				EvaluateProperties();

				if (StateChanging != null && DebugValueToStringMethod != null && !CancelEvaluation)
				{
					StateChanging(this, ValueDisplayerState.EvaluateToString, 1);
				}

				EvaluateToString();
			}
		}

		private TypeDefinition FindBaseTypeDefinition(TokenBase baseTokenObject)
		{
			TypeDefinition result = null;
			TypeDefinition baseTypeDefinition = baseTokenObject as TypeDefinition;

			if (baseTypeDefinition == null)
			{
				TypeReference baseTypeReference = baseTokenObject as TypeReference;

				if (baseTypeReference == null)
				{
					TypeSpecification baseTypeSpecification = (TypeSpecification)baseTokenObject;
					TypeSignatureItem typeSignatureItem = baseTypeSpecification.Type as TypeSignatureItem;

					if (typeSignatureItem != null)
					{
						result = FindBaseTypeDefinition(typeSignatureItem.TokenObject);
					}
					else
					{
						throw new NotImplementedException("Inspecting the following type is not implemented yet: " + baseTypeSpecification.Type.GetType().FullName);
					}
				}
				else
				{
					result = HelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly);

					if (result == null && ErrorOccurred != null)
					{
						ModuleWrapper referencedModule = HelperFunctions.FindModuleByNameWithoutExtension(EvaluationContext, baseTypeReference.ReferencedAssembly);

						if (referencedModule == null)
						{
							ErrorOccurred(this, new ErrorValueFormatter(string.Format("The '{0}' module is not loaded by the current thread.", baseTypeReference.ReferencedAssembly)));
						}
						else
						{
							ErrorOccurred(this, new MissingModuleFormatter(new MissingModule(referencedModule)));
						}
					}
				}
			}
			else
			{
				result = baseTypeDefinition;
			}

			return result;
		}

		private TypeDefinition FindBaseTypeDefinition(TypeDefinition typeDefinition)
		{
			TypeDefinition result = null;

			if (typeDefinition.ModuleScope.Assembly.AllTokens.ContainsKey(typeDefinition.BaseTypeToken))
			{
				TokenBase baseTokenObject = typeDefinition.ModuleScope.Assembly.AllTokens[typeDefinition.BaseTypeToken];

				result = FindBaseTypeDefinition(baseTokenObject);
			}

			return result;
		}

		private void CollectTypeInformation(TypeDefinition typeDefinition, TypeDefinition stringTypeDef)
		{
			if (!CancelEvaluation)
			{
				if (typeDefinition.FieldDefinitions != null)
				{
					foreach (KeyValuePair<uint, FieldDefinition> fieldDefinitionPair in typeDefinition.FieldDefinitions)
					{
						DebugValueFields.Add(fieldDefinitionPair.Value);
					}
				}

				if (!CancelEvaluation && typeDefinition.Properties != null)
				{
					foreach (KeyValuePair<uint, Property> propertyPair in typeDefinition.Properties)
					{
						if (!DebugValueProperties.ContainsKey(propertyPair.Value.Name) && typeDefinition.ModuleScope.Assembly.AllTokens.ContainsKey(propertyPair.Value.GetterMethodToken))
						{
							MethodDefinition getterMethod = (MethodDefinition)typeDefinition.ModuleScope.Assembly.AllTokens[propertyPair.Value.GetterMethodToken];

							if ((getterMethod.Flags & CorMethodAttr.mdAbstract) != CorMethodAttr.mdAbstract && !getterMethod.IsStatic)
							{
								if (getterMethod.Parameters == null || getterMethod.Parameters.Count == 0)
								{
									DebugValueProperties.Add(propertyPair.Value.Name, propertyPair.Value);
								}
							}
						}
					}
				}

				if (!CancelEvaluation && DebugValueToStringMethod == null && stringTypeDef != null)
				{
					DebugValueToStringMethod = typeDefinition.SearchToStringMethod(stringTypeDef);
				}

				if (!CancelEvaluation)
				{
					if (TypeInspected != null)
					{
						TypeInspected(this, typeDefinition);
					}

					TypeDefinition baseTypeDefinition = FindBaseTypeDefinition(typeDefinition);

					if (baseTypeDefinition != null)
					{
						CollectTypeInformation(baseTypeDefinition, stringTypeDef);
					}
				}
			}
		}

		private IValueFormatter EvaluateField(FieldDefinition fieldDefinition)
		{
			IValueFormatter result = null;

			if ((fieldDefinition.Flags & CorFieldAttr.fdLiteral) == CorFieldAttr.fdLiteral)
			{
				if (fieldDefinition.IsReformattableDefaultValue())
				{
					result = new NumberValueFormatter<object>(fieldDefinition.DefaultValueNumber);
				}
				else
				{
					result = new StringValueFormatter(fieldDefinition.DefaultValueAsString);
				}
			}
			else
			{
				ClassWrapper fieldClass = null;

				try
				{
					fieldClass = HelperFunctions.FindClassOfTypeDefintion(EvaluationContext, fieldDefinition.BaseTypeDefinition);
				}
				catch (MissingModuleException missingModuleException)
				{
					result = new MissingModuleFormatter(missingModuleException.MissingModule);
				}
				catch (Exception exception)
				{
					result = new ErrorValueFormatter(exception.Message);
				}

				if (fieldClass != null)
				{
					if ((fieldDefinition.Flags & CorFieldAttr.fdStatic) == CorFieldAttr.fdStatic)
					{
						try
						{
							ValueWrapper fieldValue = fieldClass.GetStaticFieldValue(fieldDefinition.Token, Frame);
							result = CreateSimpleFormatter(fieldValue);

							if (result.IsComplexType)
							{
								result.ValueRefresher = new StaticFieldValueRefresher(fieldDefinition.Name, fieldClass, FrameRefresher, fieldDefinition.Token);
							}
						}
						catch (Exception exception)
						{
							result = new ErrorValueFormatter(exception.Message);
						}
					}
					else
					{
						try
						{
							ValueWrapper fieldValue = null;

							if ((CorElementType)DebugValue.ResultValue.ElementType == CorElementType.ELEMENT_TYPE_VALUETYPE)
							{
								fieldValue = DebugValue.ResultValue.GetFieldValue(fieldClass, fieldDefinition.Token);
							}
							else
							{
								ValueWrapper dereferencedValue = DebugValue.ResultValue.DereferenceValue();

								if (dereferencedValue.IsBoxedValue())
								{
									dereferencedValue = dereferencedValue.UnboxValue();
								}

								fieldValue = dereferencedValue.GetFieldValue(fieldClass, fieldDefinition.Token);
							}

							result = CreateSimpleFormatter(fieldValue);

							if (result.IsComplexType)
							{
								result.ValueRefresher = new FieldValueRefresher(fieldDefinition.Name, DebugValueRefresher, fieldClass, fieldDefinition.Token);
							}
						}
						catch (Exception exception)
						{
							result = new ErrorValueFormatter(exception.Message);
						}
					}
				}
			}

			if (result.FieldGroup != ValueFieldGroup.MissingModule)
			{
				result.FieldGroup = (ValueFieldGroup)(fieldDefinition.Flags & CorFieldAttr.fdFieldAccessMask);
				result.Name = fieldDefinition.Name;
			}

			return result;
		}

		private void EvaluateFields()
		{
			int index = 0;

			while (!CancelEvaluation && index < DebugValueFields.Count)
			{
				FieldDefinition fieldDefinition = DebugValueFields[index++];
				IValueFormatter fieldValueFormatter = EvaluateField(fieldDefinition);

				if (FieldEvaluated != null)
				{
					FieldEvaluated(this, fieldDefinition, fieldValueFormatter);
				}
			}
		}

		private IValueFormatter EvaluateMethod(MethodDefinition methodDefinition, string fieldName)
		{
			IValueFormatter result = null;
			List<ValueWrapper> arguments = new List<ValueWrapper>(1);
			arguments.Add(DebugValue.ResultValue);

			BaseEvaluationResult evaluationResult;
			List<TypeWrapper> typeArguments = null;

			if (DebugValue.ResultValue.IsVersion2)
			{
				TypeWrapper exactType = DebugValue.ResultValue.Version2.GetExactType();
				typeArguments = exactType.EnumerateAllTypeParameters();
			}

			evaluationResult = EvaluationContext.EvaluationHandler.CallMethod(EvaluationContext, methodDefinition, arguments, typeArguments);

			if (evaluationResult.IsSuccessful)
			{
				result = CreateSimpleFormatter(evaluationResult.Result);
			}
			else
			{
				result = new ErrorValueFormatter(string.Format("An error occurred while trying to evaluate the {0} method. (Message: {1})", methodDefinition.HeaderText, evaluationResult.Exception.Message));

				if (evaluationResult.Reason == EvaluationFinishedReason.AbortFailed || evaluationResult.Reason == EvaluationFinishedReason.AbortTimeout)
				{
					CancelReason = ValueDisplayerCancelReason.MethodCallAbortFailed;
				}
			}

			if (result.FieldGroup != ValueFieldGroup.MissingModule)
			{
				result.FieldGroup = (ValueFieldGroup)(methodDefinition.Flags & CorMethodAttr.mdMemberAccessMask);
				result.Name = fieldName;
			}

			return result;
		}

		private IValueFormatter EvaluateProperty(Property property)
		{
			IValueFormatter result = null;
			MethodDefinition getterMethod = (MethodDefinition)property.BaseTypeDefinition.ModuleScope.Assembly.AllTokens[property.GetterMethodToken];
			result = EvaluateMethod(getterMethod, property.Name);

			if (result.IsComplexType)
			{
				result.ValueRefresher = new ThisMethodValueRefresher(property.Name, EvaluationContext, DebugValueRefresher, getterMethod);
			}

			return result;
		}

		private void EvaluateProperties()
		{
			Dictionary<string, Property>.Enumerator propertiesEnumerator = DebugValueProperties.GetEnumerator();

			while (!CancelEvaluation && propertiesEnumerator.MoveNext())
			{
				IValueFormatter propertyValueFormatter = EvaluateProperty(propertiesEnumerator.Current.Value);

				if (PropertyEvaluated != null)
				{
					PropertyEvaluated(this, propertiesEnumerator.Current.Value, propertyValueFormatter);
				}
			}
		}

		private void EvaluateToString()
		{
			if (!CancelEvaluation && DebugValueToStringMethod != null)
			{
				IValueFormatter valueFormatter = EvaluateMethod(DebugValueToStringMethod, string.Format("{0}()", DebugValueToStringMethod.HeaderText));

				if (valueFormatter.IsComplexType)
				{
					valueFormatter.ValueRefresher = new ThisMethodValueRefresher(Constants.ToStringMethodName, EvaluationContext, DebugValueRefresher, DebugValueToStringMethod);
				}

				valueFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;

				if (ToStringEvaluated != null)
				{
					ToStringEvaluated(this, DebugValueToStringMethod, valueFormatter);
				}
			}
		}

		private void EvaluateStringValue()
		{
			ValueWrapper dereferencedValue = DebugValue.ResultValue.DereferenceValue();
			string stringValue = dereferencedValue.GetStringValue();
			stringValue = HelperFunctions.ShowEscapeCharacters(stringValue, true);
			StringValueFormatter stringValueFormatter = new StringValueFormatter(stringValue);
			stringValueFormatter.Name = "String value";
			stringValueFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;

			if (StringValueEvaluated != null)
			{
				StringValueEvaluated(this, stringValueFormatter);
			}
		}
	}
}