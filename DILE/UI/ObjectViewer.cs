using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.IO;
using System.Runtime.InteropServices;

namespace Dile.UI
{
	public partial class ObjectViewer : Form
	{
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern int GetMenuItemCount(IntPtr hMenu);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern int EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern int DrawMenuBar(IntPtr hWnd);

		private const int MF_BYPOSITION = 0x400;
		private const int MF_ENABLED = 0x0;
		private const int MF_DISABLED = 0x2;

		private delegate void DisplayValueFormatterDelegate(IValueFormatter valueFormatter, TreeNode parentNode);
		private delegate void EnableControlsForEvaluationDelegate(bool enable);
		private delegate void DisplayEvaluationStateDelegate(string stateDescription, int stepCount);

		private ListViewGroup privateScopeGroup;
		private ListViewGroup PrivateScopeGroup
		{
			get
			{
				return privateScopeGroup;
			}
			set
			{
				privateScopeGroup = value;
			}
		}

		private ListViewGroup privateGroup;
		private ListViewGroup PrivateGroup
		{
			get
			{
				return privateGroup;
			}
			set
			{
				privateGroup = value;
			}
		}

		private ListViewGroup familyAndAssemblyGroup;
		private ListViewGroup FamilyAndAssemblyGroup
		{
			get
			{
				return familyAndAssemblyGroup;
			}
			set
			{
				familyAndAssemblyGroup = value;
			}
		}

		private ListViewGroup assemblyGroup;
		private ListViewGroup AssemblyGroup
		{
			get
			{
				return assemblyGroup;
			}
			set
			{
				assemblyGroup = value;
			}
		}

		private ListViewGroup familyGroup;
		private ListViewGroup FamilyGroup
		{
			get
			{
				return familyGroup;
			}
			set
			{
				familyGroup = value;
			}
		}

		private ListViewGroup familyOrAssemblyGroup;
		private ListViewGroup FamilyOrAssemblyGroup
		{
			get
			{
				return familyOrAssemblyGroup;
			}
			set
			{
				familyOrAssemblyGroup = value;
			}
		}

		private ListViewGroup publicGroup;
		private ListViewGroup PublicGroup
		{
			get
			{
				return publicGroup;
			}
			set
			{
				publicGroup = value;
			}
		}

		private ListViewGroup objectInformationGroup;
		private ListViewGroup ObjectInformationGroup
		{
			get
			{
				return objectInformationGroup;
			}
			set
			{
				objectInformationGroup = value;
			}
		}

		private ListViewGroup evaluationExceptionGroup;
		private ListViewGroup EvaluationExceptionGroup
		{
			get
			{
				return evaluationExceptionGroup;
			}
			set
			{
				evaluationExceptionGroup = value;
			}
		}

		private ListViewGroup missingModulesGroup;
		private ListViewGroup MissingModulesGroup
		{
			get
			{
				return missingModulesGroup;
			}
			set
			{
				missingModulesGroup = value;
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

		private TreeNode rootNode;
		private TreeNode RootNode
		{
			get
			{
				if (rootNode == null)
				{
					rootNode = new TreeNode();
					objectTree.Nodes.Add(rootNode);
				}

				return rootNode;
			}
		}

		private ToolStripMenuItem addModuleMenuItem;
		private ToolStripMenuItem AddModuleMenuItem
		{
			get
			{
				return addModuleMenuItem;
			}
			set
			{
				addModuleMenuItem = value;
			}
		}

		private ToolStripMenuItem AddModuleFromDumpMenuItem
		{
			get;
			set;
		}

		private ToolStripMenuItem displayPropertiesMenuItem;
		private ToolStripMenuItem DisplayPropertiesMenuItem
		{
			get
			{
				return displayPropertiesMenuItem;
			}
			set
			{
				displayPropertiesMenuItem = value;
			}
		}

		private ValueDisplayer valueDisplayer;
		public ValueDisplayer ValueDisplayer
		{
			get
			{
				return valueDisplayer;
			}
			set
			{
				valueDisplayer = value;
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

		private DisplayValueFormatterDelegate displayValueFormatterMethod;
		private DisplayValueFormatterDelegate DisplayValueFormatterMethod
		{
			get
			{
				return displayValueFormatterMethod;
			}
			set
			{
				displayValueFormatterMethod = value;
			}
		}

		private EnableControlsForEvaluationDelegate enableControlsForEvaluationMethod;
		private EnableControlsForEvaluationDelegate EnableControlsForEvaluationMethod
		{
			get
			{
				return enableControlsForEvaluationMethod;
			}
			set
			{
				enableControlsForEvaluationMethod = value;
			}
		}

		private NoArgumentsDelegate stepEvaluationProgressMethod;
		private NoArgumentsDelegate StepEvaluationProgressMethod
		{
			get
			{
				return stepEvaluationProgressMethod;
			}
			set
			{
				stepEvaluationProgressMethod = value;
			}
		}

		private DisplayEvaluationStateDelegate displayEvaluationStateMethod;
		private DisplayEvaluationStateDelegate DisplayEvaluationStateMethod
		{
			get
			{
				return displayEvaluationStateMethod;
			}
			set
			{
				displayEvaluationStateMethod = value;
			}
		}

		private bool updateDisplayedValue = true;
		public bool UpdateDisplayedValue
		{
			get
			{
				return updateDisplayedValue;
			}
			set
			{
				updateDisplayedValue = value;
			}
		}

		private bool cancelValueFormattersDisplaying;
		public bool CancelValueFormattersDisplaying
		{
			get
			{
				return cancelValueFormattersDisplaying;
			}
			set
			{
				cancelValueFormattersDisplaying = value;
			}
		}

		private List<IValueFormatter> missingModules;
		private List<IValueFormatter> MissingModules
		{
			get
			{
				return missingModules;
			}
			set
			{
				missingModules = value;
			}
		}

		private int evaluationLogListBoxHeight;
		private int EvaluationLogListBoxHeight
		{
			get
			{
				return evaluationLogListBoxHeight;
			}
			set
			{
				evaluationLogListBoxHeight = value;
			}
		}

		private bool isTypeOfValueFound;
		private bool IsTypeOfValueFound
		{
			get
			{
				return isTypeOfValueFound;
			}
			set
			{
				isTypeOfValueFound = value;
			}
		}

		public ObjectViewer()
		{
			InitializeComponent();

			fieldList.Initialize();

			AddModuleFromDumpMenuItem = new ToolStripMenuItem("Add module to project from memory dump");
			AddModuleFromDumpMenuItem.Click += new EventHandler(AddModuleFromDumpMenuItem_Click);
			fieldList.ItemContextMenu.Items.Insert(0, AddModuleFromDumpMenuItem);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			fieldList.ItemContextMenu.Items.Insert(0, AddModuleMenuItem);

			DisplayPropertiesMenuItem = new ToolStripMenuItem("Display properties");
			DisplayPropertiesMenuItem.Click += new EventHandler(DisplayPropertiesMenuItem_Click);
			fieldList.ItemContextMenu.Items.Insert(1, DisplayPropertiesMenuItem);

			fieldList.ItemContextMenu.Opening += new CancelEventHandler(ItemContextMenu_Opening);

			MissingModulesGroup = fieldList.Groups[0];
			EvaluationExceptionGroup = fieldList.Groups[1];
			ObjectInformationGroup = fieldList.Groups[2];
			PublicGroup = fieldList.Groups[3];
			FamilyOrAssemblyGroup = fieldList.Groups[4];
			FamilyGroup = fieldList.Groups[5];
			AssemblyGroup = fieldList.Groups[6];
			FamilyAndAssemblyGroup = fieldList.Groups[7];
			PrivateGroup = fieldList.Groups[8];
			PrivateScopeGroup = fieldList.Groups[9];

			displayHexaNumbersButton.Checked = Settings.Instance.DisplayHexaNumbers;
			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			evaluateMethodsButton.Checked = Settings.Instance.EvaluateMethodsInObjectViewer;

			DisplayValueFormatterMethod = new DisplayValueFormatterDelegate(DisplayValueFormatter);
			EnableControlsForEvaluationMethod = new EnableControlsForEvaluationDelegate(EnableControlsForEvaluation);
			StepEvaluationProgressMethod = new NoArgumentsDelegate(StepEvaluationProgress);
			DisplayEvaluationStateMethod = new DisplayEvaluationStateDelegate(DisplayEvaluationState);

			EvaluationLogListBoxHeight = evaluationLogListBox.Height;
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			displayHexaNumbersButton.Checked = Settings.Instance.DisplayHexaNumbers;

			foreach (ListViewItem item in fieldList.Items)
			{
				IValueFormatter valueFormatter = item.Tag as IValueFormatter;

				if (valueFormatter != null)
				{
					item.ToolTipText = valueFormatter.GetFormattedString(Settings.Instance.DisplayHexaNumbers);
					item.SubItems[1].Text = item.ToolTipText;
				}
			}
		}

		private ListViewGroup GetFieldGroup(ValueFieldGroup propertyGroup)
		{
			ListViewGroup result = null;

			switch (propertyGroup)
			{
				case ValueFieldGroup.MissingModule:
					result = MissingModulesGroup;
					break;

				case ValueFieldGroup.EvaluationException:
					result = EvaluationExceptionGroup;
					break;

				case ValueFieldGroup.PrivateScope:
					result = PrivateScopeGroup;
					break;

				case ValueFieldGroup.Private:
					result = PrivateGroup;
					break;

				case ValueFieldGroup.FamilyAndAssembly:
					result = FamilyAndAssemblyGroup;
					break;

				case ValueFieldGroup.Assembly:
					result = AssemblyGroup;
					break;

				case ValueFieldGroup.Family:
					result = FamilyGroup;
					break;

				case ValueFieldGroup.FamilyOrAssembly:
					result = FamilyOrAssemblyGroup;
					break;

				case ValueFieldGroup.Public:
					result = PublicGroup;
					break;

				case ValueFieldGroup.ObjectInformation:
					result = ObjectInformationGroup;
					break;
			}

			return result;
		}

		public void ShowValue(BaseValueRefresher valueRefresher, FrameWrapper frame, string initialExpression)
		{
			evaluateMethodsButton.Checked = Settings.Instance.EvaluateMethodsInObjectViewer;

			if (frame == null)
			{
				MessageBox.Show("There is no active frame and thus expressions cannot be evaluated.\n\nFrames can be changed by using the Call Stack Panel", "No active frame", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				if (!IsHandleCreated)
				{
					CreateHandle();
				}

				objectTree.Sorted = false;

				UpdateDisplayedValue = false;
				objectTree.SelectedNode = RootNode;
				UpdateDisplayedValue = true;

				IValueFormatter rootValueFormatter = new RootValueFormatter("Viewed object");
				rootValueFormatter.ValueRefresher = valueRefresher;
				rootValueFormatter.FieldNode = RootNode;
				RootNode.Tag = rootValueFormatter;
				RootNode.Text = rootValueFormatter.Name;

				FrameRefresher = new FrameRefresher(DebugEventHandler.Instance.EventObjects.Thread, frame.ChainIndex, frame.FrameIndex, frame.IsActiveFrame);

				ProcessWrapper processWrapper = DebugEventHandler.Instance.EventObjects.Thread.GetProcess();
				EvaluationHandler evaluationHandler = new EvaluationHandler(FrameRefresher);
				EvalWrapper evalWrapper = DebugEventHandler.Instance.EventObjects.Thread.CreateEval();
				EvaluationContext = new EvaluationContext(processWrapper,
					evaluationHandler,
					evalWrapper,
					DebugEventHandler.Instance.EventObjects.Thread,
					evaluateMethodsButton.Checked && (DebugEventHandler.Instance.State != DebuggerState.DumpDebugging));
				ValueDisplayer = new ValueDisplayer(evaluationContext);
				ValueDisplayer.ArrayElementEvaluated += new ArrayElementEvaluatedDelegate(ValueDisplayer_ArrayElementEvaluated);
				ValueDisplayer.ErrorOccurred += new ErrorOccurredDelegate(ValueDisplayer_ErrorOccurred);
				ValueDisplayer.EvaluatedNull += new EvaluatedNullDelegate(ValueDisplayer_EvaluatedNull);
				ValueDisplayer.FieldEvaluated += new FieldEvaluatedDelegate(ValueDisplayer_FieldEvaluated);
				ValueDisplayer.PropertyEvaluated += new PropertyEvaluatedDelegate(ValueDisplayer_PropertyEvaluated);
				ValueDisplayer.StateChanging += new StateChangingDelegate(ValueDisplayer_StateChanging);
				ValueDisplayer.StringValueEvaluated += new StringValueEvaluatedDelegate(ValueDisplayer_StringValueEvaluated);
				ValueDisplayer.ToStringEvaluated += new ToStringEvaluatedDelegate(ValueDisplayer_ToStringEvaluated);
				ValueDisplayer.TypeInspected += new TypeInspectedDelegate(ValueDisplayer_TypeInspected);

				if (valueRefresher != null)
				{
					DisplayValue(valueRefresher, RootNode);
				}

				fieldList.Sort();
				objectTree.Sorted = true;
				DisplayExpressionText(initialExpression);
				expressionComboBox.Update();
				ActiveControl = expressionComboBox;
				ShowDialog();
			}
		}

		private void InvokeDisplayValueFormatter(IValueFormatter valueFormatter, TreeNode parentNode)
		{
			if (InvokeRequired)
			{
				Invoke(DisplayValueFormatterMethod, valueFormatter, parentNode);
			}
			else
			{
				DisplayValueFormatter(valueFormatter, parentNode);
			}
		}

		private void InvokeEnableControlsForEvaluation(bool enable)
		{
			if (InvokeRequired)
			{
				Invoke(EnableControlsForEvaluationMethod, enable);
			}
			else
			{
				EnableControlsForEvaluation(enable);
			}
		}

		private void InvokeStepEvaluationProgress()
		{
			if (InvokeRequired)
			{
				Invoke(StepEvaluationProgressMethod);
			}
			else
			{
				StepEvaluationProgress();
			}
		}

		private void InvokeDisplayEvaluationState(string stateDescription, int stepCount)
		{
			if (InvokeRequired)
			{
				Invoke(DisplayEvaluationStateMethod, stateDescription, stepCount);
			}
			else
			{
				DisplayEvaluationState(stateDescription, stepCount);
			}
		}

		private void ValueDisplayer_ArrayElementEvaluated(ValueDisplayer sender, uint elementIndex, IValueFormatter elementValueFormatter)
		{
			InvokeDisplayValueFormatter(elementValueFormatter, sender.ParentNode);
			InvokeStepEvaluationProgress();
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_ErrorOccurred(ValueDisplayer sender, IValueFormatter errorFormatter)
		{
			InvokeDisplayValueFormatter(errorFormatter, sender.ParentNode);
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_EvaluatedNull(ValueDisplayer sender, IValueFormatter nullValueFormatter)
		{
			InvokeDisplayValueFormatter(nullValueFormatter, sender.ParentNode);
			InvokeStepEvaluationProgress();
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_FieldEvaluated(ValueDisplayer sender, FieldDefinition fieldDefinition, IValueFormatter fieldValueFormatter)
		{
			InvokeDisplayValueFormatter(fieldValueFormatter, sender.ParentNode);
			InvokeStepEvaluationProgress();
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_PropertyEvaluated(ValueDisplayer sender, Property property, IValueFormatter propertyValueFormatter)
		{
			InvokeDisplayValueFormatter(propertyValueFormatter, sender.ParentNode);
			InvokeStepEvaluationProgress();
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_StateChanging(ValueDisplayer sender, ValueDisplayerState state, int stepCount)
		{
			switch (state)
			{
				case ValueDisplayerState.Initialize:
					InvokeDisplayEvaluationState("Initializing a new thread to inspect the value...", stepCount);
					break;

				case ValueDisplayerState.StartThread:
					InvokeStepEvaluationProgress();
					InvokeDisplayEvaluationState("Starting new thread to inspect the value...", stepCount);
					break;

				case ValueDisplayerState.CollectTypeInformation:
					InvokeStepEvaluationProgress();
					InvokeDisplayEvaluationState("Collecting information about the base types of the value...", stepCount);
					break;

				case ValueDisplayerState.EvaluateArrayElements:
					InvokeStepEvaluationProgress();
					InvokeDisplayEvaluationState("Evaluating elements of the array value...", stepCount);
					break;

				case ValueDisplayerState.EvaluateStringValue:
					InvokeStepEvaluationProgress();
					InvokeDisplayEvaluationState("Evaluating string value...", stepCount);
					break;

				case ValueDisplayerState.EvaluateFields:
					InvokeStepEvaluationProgress();
					InvokeDisplayEvaluationState("Evaluating fields of the object value...", stepCount);
					break;

				case ValueDisplayerState.EvaluateProperties:
					InvokeDisplayEvaluationState("Evaluating properties of the object value....", stepCount);
					break;

				case ValueDisplayerState.EvaluateToString:
					InvokeDisplayEvaluationState("Evaluating ToString() method of the object value...", stepCount);
					break;

				case ValueDisplayerState.Finish:
					string message = "Evaluation of the value is finished.";

					if (MissingModules.Count > 0)
					{
						message += " Not every information is displayed because some modules are not added to the project!";
					}

					InvokeDisplayEvaluationState(message, stepCount);
					InvokeEnableControlsForEvaluation(true);
					break;

				case ValueDisplayerState.Interrupted:
					InvokeDisplayEvaluationState("Evaluation of the value has been stopped by the user, therefore not every information is displayed!", stepCount);
					InvokeEnableControlsForEvaluation(true);
					break;

				case ValueDisplayerState.MethodCallAbortFailed:
					InvokeDisplayEvaluationState("Not every information is displayed because aborting an evaluation failed. Both the debugger and the debuggee can be in an unstable state!", stepCount);
					InvokeEnableControlsForEvaluation(true);
					break;
			}

			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_StringValueEvaluated(ValueDisplayer sender, IValueFormatter stringValueFormatter)
		{
			InvokeDisplayValueFormatter(stringValueFormatter, sender.ParentNode);
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_ToStringEvaluated(ValueDisplayer sender, MethodDefinition toStringMethodDef, IValueFormatter toStringValueFormatter)
		{
			InvokeDisplayValueFormatter(toStringValueFormatter, sender.ParentNode);
			InvokeStepEvaluationProgress();
			UpdateCancelEvaluation(sender);
		}

		private void ValueDisplayer_TypeInspected(ValueDisplayer sender, TypeDefinition typeDefinition)
		{
			InvokeDisplayEvaluationState(string.Format("Type inspected: {0} (assembly name: {1})", typeDefinition.FullName, typeDefinition.ModuleScope.Assembly.FileName), 1);
			UpdateCancelEvaluation(sender);

			if (!IsTypeOfValueFound)
			{
				IsTypeOfValueFound = true;

				StringValueFormatter objectTypeValueFormatter = new StringValueFormatter(typeDefinition.FullName);
				objectTypeValueFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;
				objectTypeValueFormatter.Name = "Type of value";

				InvokeDisplayValueFormatter(objectTypeValueFormatter, sender.ParentNode);
			}
		}

		private void UpdateCancelEvaluation(ValueDisplayer valueDisplayer)
		{
			if (CancelValueFormattersDisplaying)
			{
				valueDisplayer.InterruptEvaluation();
			}
		}

		private void ChangeCloseButton(bool enable)
		{
			IntPtr menuHandle;
			menuHandle = GetSystemMenu(Handle, 0);

			if (menuHandle != IntPtr.Zero)
			{
				int command = MF_BYPOSITION;

				if (enable)
				{
					command |= MF_ENABLED;
				}
				else
				{
					command |= MF_DISABLED;
				}

				int menuItemCount = GetMenuItemCount(menuHandle);

				EnableMenuItem(menuHandle, menuItemCount - 1, command);
				EnableMenuItem(menuHandle, menuItemCount - 2, command);

				DrawMenuBar(Handle);
			}
		}

		private void EnableControlsForEvaluation(bool enable)
		{
			evaluateButton.Enabled = enable;
			stopEvaluationButton.Enabled = !enable;
			goUpButton.Enabled = enable;
			closeButton.Enabled = enable;
			ChangeCloseButton(enable);

			if (enable)
			{
				DebugEventHandler.Instance.EventObjects.Frame = FrameRefresher.GetRefreshedValue();
			}
		}

		private void StepEvaluationProgress()
		{
			evaluationProgress.Value += 1;
		}

		private void DisplayEvaluationState(string stateDescription, int stepCount)
		{
			evaluationStepLabel.Text = stateDescription;
			evaluationLogListBox.Items.Add(stateDescription);
			evaluationLogListBox.SelectedIndex = evaluationLogListBox.Items.Count - 1;
			evaluationProgress.Value = 0;
			evaluationProgress.Maximum = stepCount;
		}

		private void DisplayValueFormatter(IValueFormatter valueFormatter)
		{
			DisplayValueFormatter(valueFormatter, null);
		}

		private void DisplayValueFormatter(IValueFormatter valueFormatter, TreeNode parentNode)
		{
			bool isMissingModuleFormatter = (valueFormatter is MissingModuleFormatter);

			if (!isMissingModuleFormatter || !MissingModules.Contains(valueFormatter))
			{
				ListViewItem fieldItem = fieldList.Items.Add(valueFormatter.Name);
				fieldItem.ToolTipText = valueFormatter.GetFormattedString(Settings.Instance.DisplayHexaNumbers);
				fieldItem.SubItems.Add(fieldItem.ToolTipText);
				fieldItem.Group = GetFieldGroup(valueFormatter.FieldGroup);
				fieldItem.Tag = valueFormatter;

				if (valueFormatter.IsComplexType)
				{
					if (parentNode == null)
					{
						throw new InvalidOperationException();
					}

					TreeNode valueFormatterNode = parentNode.Nodes.Add(valueFormatter.Name);
					valueFormatterNode.Tag = valueFormatter;
					valueFormatter.FieldNode = valueFormatterNode;
				}

				if (isMissingModuleFormatter)
				{
					MissingModules.Add(valueFormatter);
				}
			}
		}

		private void DisplayValue(BaseValueRefresher valueRefresher, TreeNode parentNode)
		{
			fieldList.Items.Clear();
			evaluationLogListBox.Items.Clear();

			try
			{
				DebugExpressionResult debugValue = new DebugExpressionResult(EvaluationContext, valueRefresher.GetRefreshedValue());

				if (HelperFunctions.HasValueClass(debugValue.ResultValue))
				{
					EnableControlsForEvaluation(false);
					CancelValueFormattersDisplaying = false;
					MissingModules = new List<IValueFormatter>();
					IsTypeOfValueFound = false;
					ValueDisplayer.CreateComplexFormatter(debugValue, valueRefresher, FrameRefresher, parentNode);
				}
				else
				{
					IValueFormatter valueFormatter = ValueDisplayer.CreateSimpleFormatter(debugValue);
					valueFormatter.Name = valueRefresher.Name;
					valueFormatter.ValueRefresher = valueRefresher;
					DisplayValueFormatter(valueFormatter);

					if (valueFormatter is ISimpleTypeValueFormatter)
					{
						StringValueFormatter valueTypeFormatter = new StringValueFormatter(((ISimpleTypeValueFormatter)valueFormatter).GetNumberTypeName());
						valueTypeFormatter.FieldGroup = ValueFieldGroup.ObjectInformation;
						valueTypeFormatter.Name = "Type of value";
						DisplayValueFormatter(valueTypeFormatter);
					}
				}
			}
			catch (EvaluationException evaluationException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation exception", evaluationException.Message));
			}
			catch (EvaluationHandlerException evaluationHandlerException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation running exception", evaluationHandlerException.Message));
			}
			catch (MissingModuleException missingModuleException)
			{
				DisplayValueFormatter(new MissingModuleFormatter(missingModuleException.MissingModule));
			}
			catch (InvalidOperationException invalidOperationException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation exception", invalidOperationException.Message));
			}
			catch (Exception exception)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Unexpected exception", exception.Message));
			}
		}

		private void ClearEvaluationResults()
		{
			fieldList.Items.Clear();

			UpdateDisplayedValue = false;
			RootNode.Nodes.Clear();
			UpdateDisplayedValue = true;
		}

		public void Initialize()
		{
			ClearEvaluationResults();
			ValueDisplayer = null;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			if (ValueDisplayer != null && ValueDisplayer.IsEvaluationRunning)
			{
				CancelValueFormattersDisplaying = true;
				ValueDisplayer.InterruptEvaluation();
			}

			evaluationLogListBox.Items.Clear();
			Close();
		}

		private void ObjectViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (closeButton.Enabled)
			{
				ValueDisplayer = null;
			}
			else
			{
				e.Cancel = true;
			}
		}

		private void fieldList_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!ValueDisplayer.IsEvaluationRunning)
			{
				DisplayPropertiesOfSelectedObject();
			}
		}

		private void DisplayPropertiesOfSelectedObject()
		{
			if (fieldList.SelectedItems != null && fieldList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = fieldList.SelectedItems[0];
				IValueFormatter valueFormatter = selectedItem.Tag as IValueFormatter;

				if (valueFormatter != null && valueFormatter.ValueRefresher != null)
				{
					if (objectTree.SelectedNode != valueFormatter.FieldNode)
					{
						UpdateDisplayedValue = false;
						objectTree.SelectedNode = valueFormatter.FieldNode;
						UpdateDisplayedValue = true;
					}

					DisplayValue(valueFormatter.ValueRefresher, valueFormatter.FieldNode);
				}
			}
		}

		private void ItemContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool isOneItemSelected = (fieldList.SelectedItems.Count == 1);

			AddModuleMenuItem.Enabled = isOneItemSelected;
			AddModuleMenuItem.Visible = false;
			AddModuleFromDumpMenuItem.Enabled = isOneItemSelected;
			AddModuleFromDumpMenuItem.Visible = false;
			DisplayPropertiesMenuItem.Enabled = isOneItemSelected;
			DisplayPropertiesMenuItem.Visible = false;

			if (isOneItemSelected)
			{
				ListViewItem selectedItem = fieldList.SelectedItems[0];
				IValueFormatter valueFormatter = selectedItem.Tag as IValueFormatter;

				if (valueFormatter != null)
				{
					if (valueFormatter is MissingModuleFormatter)
					{
						AddModuleMenuItem.Visible = true;
						AddModuleFromDumpMenuItem.Visible = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging);
					}

					if (valueFormatter.IsComplexType)
					{
						DisplayPropertiesMenuItem.Visible = true;
					}
				}
			}
		}

		private void AddMissingModule(bool treatAsInMemory)
		{
			if (!ValueDisplayer.IsEvaluationRunning && fieldList.SelectedItems != null && fieldList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = fieldList.SelectedItems[0];
				MissingModuleFormatter valueFormatter = selectedItem.Tag as MissingModuleFormatter;

				if (valueFormatter != null)
				{
					valueFormatter.MissingModule.AddModuleToProject(treatAsInMemory);
					fieldList.Items.Remove(selectedItem);
				}
			}
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			AddMissingModule(false);
		}

		private void AddModuleFromDumpMenuItem_Click(object sender, EventArgs e)
		{
			AddMissingModule(true);
		}

		private void DisplayPropertiesMenuItem_Click(object sender, EventArgs e)
		{
			if (!ValueDisplayer.IsEvaluationRunning)
			{
				DisplayPropertiesOfSelectedObject();
			}
		}

		private void goUpButton_Click(object sender, EventArgs e)
		{
			if (objectTree.SelectedNode == null)
			{
				objectTree.SelectedNode = RootNode;
			}
			else if (objectTree.SelectedNode.Parent != null)
			{
				objectTree.SelectedNode = objectTree.SelectedNode.Parent;
			}
		}

		private void objectTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (UpdateDisplayedValue && ValueDisplayer != null)
			{
				if (ValueDisplayer.IsEvaluationRunning)
				{
					e.Cancel = true;
				}
				else
				{
					IValueFormatter nodeValueFormatter = e.Node.Tag as IValueFormatter;

					if (nodeValueFormatter != null && nodeValueFormatter.ValueRefresher != null)
					{
						UpdateDisplayedValue = false;
						e.Node.Nodes.Clear();
						UpdateDisplayedValue = true;

						DisplayValue(nodeValueFormatter.ValueRefresher, e.Node);
					}
				}
			}
		}

		private void displayHexaNumbersButton_Click(object sender, EventArgs e)
		{
			Settings.Instance.DisplayHexaNumbers = !Settings.Instance.DisplayHexaNumbers;
			Settings.SaveConfiguration();
		}

		private void evaluateButton_Click(object sender, EventArgs e)
		{
			DisplayExpressionText(expressionComboBox.Text);
			ClearEvaluationResults();

			try
			{
				Parser parser = new Parser();
				List<BaseExpression> expressions = parser.Parse(expressionComboBox.Text);

				if (expressions.Count > 0 && expressions[0] is MemberExpression)
				{
					throw new ParserException("The expression cannot start with a member expression (most likely an assembly is not in the project and therefore type information is not available).");
				}

				ExpressionValueRefresher expressionRefresher = new ExpressionValueRefresher(expressions, FrameRefresher, EvaluationContext.EvaluationHandler, expressionComboBox.Text);

				objectTree.Sorted = false;

				UpdateDisplayedValue = false;
				objectTree.SelectedNode = RootNode;
				UpdateDisplayedValue = true;

				IValueFormatter rootValueFormatter = new RootValueFormatter("Evaluated expression: " + expressionComboBox.Text);
				rootValueFormatter.ValueRefresher = expressionRefresher;
				rootValueFormatter.FieldNode = RootNode;
				RootNode.Tag = rootValueFormatter;
				RootNode.Text = rootValueFormatter.Name;

				DisplayValue(expressionRefresher, RootNode);

				fieldList.Sort();
				objectTree.Sorted = true;
			}
			catch (ParserException parserException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Parser exception", parserException.Message));
			}
			catch (EvaluationException evaluationException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation exception", evaluationException.Message));
			}
			catch (EvaluationHandlerException evaluationHandlerException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation running exception", evaluationHandlerException.Message));
			}
			catch (MissingModuleException missingModuleException)
			{
				DisplayValueFormatter(new MissingModuleFormatter(missingModuleException.MissingModule));
			}
			catch (InvalidOperationException invalidOperationException)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Evaluation exception", invalidOperationException.Message));
			}
			catch (Exception exception)
			{
				DisplayValueFormatter(new ErrorValueFormatter("Unexpected exception", exception.Message));
			}

			expressionComboBox.Focus();
		}

		private void DisplayExpressionText(string expressionText)
		{
			if (expressionText != null)
			{
				if (expressionComboBox.Items.Contains(expressionText))
				{
					string expression = expressionText;

					expressionComboBox.Items.Remove(expression);
					expressionComboBox.Items.Insert(0, expression);
					expressionComboBox.Text = expression;
				}
				else
				{
					expressionComboBox.Items.Insert(0, expressionText);
					expressionComboBox.Text = expressionText;
				}
			}
		}

		private void ObjectViewer_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Alt && e.KeyCode == Keys.E)
			{
				expressionComboBox.Focus();
			}
			else if (e.KeyCode == Keys.Escape)
			{
				if (stopEvaluationButton.Enabled)
				{
					StopEvaluation();
				}
				else
				{
					DialogResult = DialogResult.Cancel;
				}
			}
		}

		private void stopEvaluationButton_Click(object sender, EventArgs e)
		{
			StopEvaluation();
		}

		private void StopEvaluation()
		{
			if (ValueDisplayer.IsEvaluationRunning)
			{
				ValueDisplayer.InterruptEvaluation();
			}

			CancelValueFormattersDisplaying = true;
		}

		private void showLogCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (showLogCheckBox.Checked)
			{
				evaluationLogPanel.Height += EvaluationLogListBoxHeight;
			}
			else
			{
				evaluationLogPanel.Height -= EvaluationLogListBoxHeight;
			}

			evaluationLogListBox.Height = EvaluationLogListBoxHeight;
			evaluationLogListBox.Visible = showLogCheckBox.Checked;
		}

		private void evaluateMethodsButton_Click(object sender, EventArgs e)
		{
			EvaluationContext.MethodCallsEnabled = evaluateMethodsButton.Checked;
			Settings.Instance.EvaluateMethodsInObjectViewer = evaluateMethodsButton.Checked;
			Settings.SaveConfiguration();
		}
	}
}