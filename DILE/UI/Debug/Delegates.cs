using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using System.Threading;
using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public delegate void MethodCallResultDelegate(ValueWrapper result, int hResult);
	public delegate void MethodCallResultWithTagDelegate(ValueWrapper result, int hResult, object tag);
	public delegate void StateChangingDelegate(ValueDisplayer sender, ValueDisplayerState state, int stepCount);
	public delegate void TypeInspectedDelegate(ValueDisplayer sender, TypeDefinition typeDefinition);
	public delegate void ArrayElementEvaluatedDelegate(ValueDisplayer sender, uint elementIndex, IValueFormatter elementValueFormatter);
	public delegate void FieldEvaluatedDelegate(ValueDisplayer sender, FieldDefinition fieldDefinition, IValueFormatter fieldValueFormatter);
	public delegate void PropertyEvaluatedDelegate(ValueDisplayer sender, Property property, IValueFormatter propertyValueFormatter);
	public delegate void ToStringEvaluatedDelegate(ValueDisplayer sender, MethodDefinition toStringMethodDef, IValueFormatter toStringValueFormatter);
	public delegate void StringValueEvaluatedDelegate(ValueDisplayer sender, IValueFormatter stringValueFormatter);
	public delegate void EvaluatedNullDelegate(ValueDisplayer sender, IValueFormatter nullValueFormatter);
	public delegate void ErrorOccurredDelegate(ValueDisplayer sender, IValueFormatter errorFormatter);
}