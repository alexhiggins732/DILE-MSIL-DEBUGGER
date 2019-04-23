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
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public partial class ObjectsPanel : BasePanel
	{
		private ObjectsPanelMode mode;
		public ObjectsPanelMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;

				ChangeMode();
			}
		}

		public string ColumnHeader
		{
			get
			{
				return objectsGrid.Columns[0].HeaderText;
			}
			set
			{
				objectsGrid.Columns[0].HeaderText = value;
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

		private ValueDisplayer valueDisplayer;
		private ValueDisplayer ValueDisplayer
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

		private ToolStripMenuItem displayInViewerMenuItem;
		private ToolStripMenuItem DisplayInViewerMenuItem
		{
			get
			{
				return displayInViewerMenuItem;
			}
			set
			{
				displayInViewerMenuItem = value;
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

		public ObjectsPanel()
		{
			InitializeComponent();
			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			objectsGrid.Initialize();

			foreach (DataGridViewColumn column in objectsGrid.Columns)
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			AddModuleFromDumpMenuItem = new ToolStripMenuItem("Add module to project from memory dump");
			AddModuleFromDumpMenuItem.Click += new EventHandler(AddModuleFromDumpMenuItem_Click);
			objectsGrid.RowContextMenu.Items.Insert(0, AddModuleFromDumpMenuItem);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			objectsGrid.RowContextMenu.Items.Insert(0, AddModuleMenuItem);

			DisplayInViewerMenuItem = new ToolStripMenuItem("Display in Object Viewer...");
			DisplayInViewerMenuItem.Click += new EventHandler(DisplayInViewerMenuItem_Click);
			objectsGrid.RowContextMenu.Items.Insert(1, DisplayInViewerMenuItem);

			objectsGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);
		}

		protected override bool UpdateWhenActiveFrameChanges()
		{
			return true;
		}

		private void ChangeMode()
		{
			switch (Mode)
			{
				case ObjectsPanelMode.Arguments:
					valueNameColumn.HeaderText = "Argument";
					valueNameColumn.ReadOnly = true;
					break;

				case ObjectsPanelMode.AutoObjects:
					valueNameColumn.HeaderText = "Expression";
					valueNameColumn.ReadOnly = true;
					break;

				case ObjectsPanelMode.LocalVariables:
					valueNameColumn.HeaderText = "Local variable";
					valueNameColumn.ReadOnly = true;
					break;

				case ObjectsPanelMode.Watch:
					valueNameColumn.HeaderText = "Expression";
					valueNameColumn.ReadOnly = false;
					break;
			}

			bool watchPanel = (Mode == ObjectsPanelMode.Watch);

			objectsGrid.ReadOnly = !watchPanel;
			objectsGrid.AllowUserToAddRows = watchPanel;
			objectsGrid.AllowUserToDeleteRows = watchPanel;
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = objectsGrid.IsCurrentCellInEditMode;

			if (!objectsGrid.IsCurrentCellInEditMode)
			{
				AddModuleMenuItem.Visible = false;
				AddModuleFromDumpMenuItem.Visible = false;
				DisplayInViewerMenuItem.Visible = false;

				if (objectsGrid.SelectedRows.Count == 1)
				{
					DataGridViewRow selectedRow = objectsGrid.SelectedRows[0];
					IValueFormatter valueFormatter = selectedRow.Tag as IValueFormatter;

					if (valueFormatter != null)
					{
						if (valueFormatter is MissingModuleFormatter)
						{
							AddModuleMenuItem.Visible = true;
							AddModuleFromDumpMenuItem.Visible = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging);
						}

						DisplayInViewerMenuItem.Visible = true;
					}
				}
			}
		}

		private void AddMissingModule(bool treatAsInMemory)
		{
			if (objectsGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow selectedRow = objectsGrid.SelectedRows[0];
				MissingModuleFormatter missingModuleFormatter = selectedRow.Tag as MissingModuleFormatter;

				if (missingModuleFormatter != null)
				{
					missingModuleFormatter.MissingModule.AddModuleToProject(treatAsInMemory);
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

		private void DisplayInViewerMenuItem_Click(object sender, EventArgs e)
		{
			if (objectsGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow selectedRow = objectsGrid.SelectedRows[0];
				IValueFormatter valueFormatter = selectedRow.Tag as IValueFormatter;

				if (valueFormatter != null)
				{
					UIHandler.Instance.ShowObjectInObjectViewer(ActiveFrame, valueFormatter.ValueRefresher, (string)selectedRow.Cells[0].Value);
				}
			}
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			foreach (DataGridViewRow row in objectsGrid.Rows)
			{
				IValueFormatter valueFormatter = row.Tag as IValueFormatter;

				if (valueFormatter != null)
				{
					row.Cells[valueColumn.Index].Value = valueFormatter.GetFormattedString(Settings.Instance.DisplayHexaNumbers);
				}
			}
		}

		private void AddValueFormatter(IValueFormatter valueFormatter)
		{
			int addedRowIndex = objectsGrid.Rows.Add(valueFormatter.Name, valueFormatter.GetFormattedString(Settings.Instance.DisplayHexaNumbers));

			DataGridViewRow addedRow = objectsGrid.Rows[addedRowIndex];
			addedRow.Tag = valueFormatter;
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			if (Mode == ObjectsPanelMode.Watch)
			{
				foreach (DataGridViewRow watchRow in objectsGrid.Rows)
				{
					watchRow.Cells[valueColumn.Index].Value = string.Empty;
				}
			}
			else
			{
				objectsGrid.Rows.Clear();
			}
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			if (DebugEventHandler.Instance.EventObjects.Thread != null)
			{
				if (ActiveFrame == null)
				{
					UIHandler.Instance.DisplayUserWarning("No stack frame information is available at the location.");
				}
				else if (!ActiveFrame.IsILFrame() && Mode != ObjectsPanelMode.Watch)
				{
					switch (Mode)
					{
						case ObjectsPanelMode.Arguments:
							UIHandler.Instance.DisplayUserWarning("The current frame is native therefore arguments are not available.");
							break;

						case ObjectsPanelMode.AutoObjects:
							UIHandler.Instance.DisplayUserWarning("The current frame is native therefore auto objects are not available.");
							break;

						case ObjectsPanelMode.LocalVariables:
							UIHandler.Instance.DisplayUserWarning("The current frame is native therefore local variables are not available.");
							break;
					}
				}
				else
				{
					EvalWrapper evalWrapper = DebugEventHandler.Instance.EventObjects.Thread.CreateEval();
					EvaluationContext = new EvaluationContext(DebugEventHandler.Instance.EventObjects.Process,
						new EvaluationHandler(ActiveFrameRefresher),
						evalWrapper,
						DebugEventHandler.Instance.EventObjects.Thread,
						(DebugEventHandler.Instance.State != DebuggerState.DumpDebugging));
					ValueDisplayer = new ValueDisplayer(EvaluationContext);

					switch (Mode)
					{
						case ObjectsPanelMode.Arguments:
							DisplayArguments();
							break;

						case ObjectsPanelMode.AutoObjects:
							DisplayCurrentException();
							break;

						case ObjectsPanelMode.LocalVariables:
							DisplayLocalVariables();
							break;

						case ObjectsPanelMode.Watch:
							DisplayWatchExpressions();
							break;
					}
				}
			}
		}

		private void DisplayArguments()
		{
			try
			{
				uint argumentCount = ActiveFrame.GetArgumentCount();
				List<BaseValueRefresher> arguments = new List<BaseValueRefresher>(Convert.ToInt32(argumentCount));

				for (uint index = 0; index < argumentCount; index++)
				{
					ArgumentValueRefresher refresher = new ArgumentValueRefresher(string.Concat("A_", index), ActiveFrameRefresher, index);

					arguments.Add(refresher);
				}

				ShowObjects(arguments);
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}
		}

		public void DisplayLocalVariables()
		{
			try
			{
				uint localVariableCount = ActiveFrame.GetLocalVariableCount();
				List<BaseValueRefresher> localVariables = new List<BaseValueRefresher>(Convert.ToInt32(localVariableCount));

				for (uint index = 0; index < localVariableCount; index++)
				{
					LocalVariableRefresher refresher = new LocalVariableRefresher(string.Concat("V_", index), ActiveFrameRefresher, index);

					localVariables.Add(refresher);
				}

				ShowObjects(localVariables);
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}
		}

		private void DisplayCurrentException()
		{
			try
			{
				ValueWrapper exceptionObject = DebugEventHandler.Instance.EventObjects.Thread.GetCurrentException();

				if (exceptionObject != null)
				{
					ExceptionValueRefresher valueRefresher = new ExceptionValueRefresher(Constants.CurrentExceptionName, DebugEventHandler.Instance.EventObjects.Thread);

					ShowObject(valueRefresher);
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}
		}

		private void DisplayWatchExpression(DataGridViewRow watchRow)
		{
			IValueFormatter watchValueFormatter = null;
			ExpressionValueRefresher expressionRefresher = null;

			try
			{
				Parser parser = new Parser();
				string watchExpression = (string)watchRow.Cells[valueNameColumn.Index].Value;
				List<BaseExpression> expressions = parser.Parse(watchExpression);
				expressionRefresher = new ExpressionValueRefresher(expressions, ActiveFrameRefresher, EvaluationContext.EvaluationHandler, watchExpression);

				watchValueFormatter = ValueDisplayer.CreateSimpleFormatter(expressionRefresher.GetRefreshedValue());
			}
			catch (ParserException parserException)
			{
				watchValueFormatter = new ErrorValueFormatter("Parser exception", parserException.Message);
			}
			catch (EvaluationException evaluationException)
			{
				watchValueFormatter = new ErrorValueFormatter("Evaluation exception", evaluationException.Message);
			}
			catch (EvaluationHandlerException evaluationHandlerException)
			{
				watchValueFormatter = new ErrorValueFormatter("Evaluation running exception", evaluationHandlerException.Message);
			}
			catch (MissingModuleException missingModuleException)
			{
				watchValueFormatter = new MissingModuleFormatter(missingModuleException.MissingModule);
			}
			catch (InvalidOperationException invalidOperationException)
			{
				watchValueFormatter = new ErrorValueFormatter("Evaluation exception", invalidOperationException.Message);
			}
			catch (Exception exception)
			{
				watchValueFormatter = new ErrorValueFormatter("Unexpected exception", exception.Message);
			}

			if (watchValueFormatter != null)
			{
				watchValueFormatter.ValueRefresher = expressionRefresher;
				watchRow.Cells[valueColumn.Index].Value = watchValueFormatter.GetFormattedString(Settings.Instance.DisplayHexaNumbers);
				watchRow.Tag = watchValueFormatter;
			}
		}

		private void DisplayWatchExpressions()
		{
			for (int index = 0; index < objectsGrid.Rows.Count; index++)
			{
				DataGridViewRow watchRow = objectsGrid.Rows[index];

				if (!watchRow.IsNewRow)
				{
					DisplayWatchExpression(watchRow);
				}
			}
		}

		private void ShowObject(BaseValueRefresher refresher)
		{
			List<BaseValueRefresher> objects = new List<BaseValueRefresher>(1);
			objects.Add(refresher);

			ShowObjects(objects);
		}

		private void ShowObjects(List<BaseValueRefresher> objects)
		{
			for (int index = 0; index < objects.Count; index++)
			{
				BaseValueRefresher valueRefresher = objects[index];
				IValueFormatter valueFormatter = null;

				try
				{
					DebugExpressionResult debugValue = new DebugExpressionResult(evaluationContext, valueRefresher.GetRefreshedValue());
					valueFormatter = ValueDisplayer.CreateSimpleFormatter(debugValue);
				}
				catch (Exception exception)
				{
					valueFormatter = new StringValueFormatter(exception.ToString());
				}

				valueFormatter.Name = valueRefresher.Name;
				valueFormatter.ValueRefresher = valueRefresher;

				AddValueFormatter(valueFormatter);
			}
		}

		private void objectsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (objectsGrid.CurrentRow != null)
			{
				IValueFormatter valueFormatter = objectsGrid.CurrentRow.Tag as IValueFormatter;

				if (valueFormatter != null)
				{
					UIHandler.Instance.ShowObjectInObjectViewer(ActiveFrame, valueFormatter.ValueRefresher, (string)objectsGrid.CurrentRow.Cells[0].Value);
				}
			}
		}

		private void objectsGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex == valueColumn.Index)
			{
				e.Cancel = true;
			}
		}

		private void objectsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (DebugEventHandler.Instance.EventObjects.Thread != null && ActiveFrame != null && e.RowIndex < objectsGrid.Rows.Count)
			{
				try
				{
					DisplayWatchExpression(objectsGrid.Rows[e.RowIndex]);
				}
				catch (Exception exception)
				{
					UIHandler.Instance.ShowException(exception);
					UIHandler.Instance.DisplayUserWarning(exception.Message);
				}
				finally
				{
					try
					{
						ActiveFrame = ActiveFrameRefresher.GetRefreshedValue();
						DebugEventHandler.Instance.EventObjects.Frame = ActiveFrame;
					}
					catch (Exception exception)
					{
						UIHandler.Instance.ShowException(exception);
						UIHandler.Instance.DisplayUserWarning(exception.Message);
					}
				}
			}
		}
	}
}