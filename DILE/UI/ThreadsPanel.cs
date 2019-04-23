using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Metadata;
using Dile.UI.Debug;
using Dile.Disassemble;
using System.Threading;
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public partial class ThreadsPanel : BasePanel
	{
		private MethodDefinition getThreadNameMethod;
		private MethodDefinition GetThreadNameMethod
		{
			get
			{
				return getThreadNameMethod;
			}
			set
			{
				getThreadNameMethod = value;
			}
		}

		private bool hasSearchedForNameMethod = false;
		private bool HasSearchedForNameMethod
		{
			get
			{
				return hasSearchedForNameMethod;
			}
			set
			{
				hasSearchedForNameMethod = value;
			}
		}

		private string evaluatedThreadName;
		private string EvaluatedThreadName
		{
			get
			{
				return evaluatedThreadName;
			}
			set
			{
				evaluatedThreadName = value;
			}
		}

		private ToolStripMenuItem changeThreadMenuItem;
		private ToolStripMenuItem ChangeThreadMenuItem
		{
			get
			{
				return changeThreadMenuItem;
			}
			set
			{
				changeThreadMenuItem = value;
			}
		}

		public ThreadsPanel()
		{
			InitializeComponent();
			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			threadsGrid.Initialize();
			ChangeThreadMenuItem = new ToolStripMenuItem("Change current thread");
			ChangeThreadMenuItem.Click += new EventHandler(ChangeThreadMenuItem_Click);
			threadsGrid.RowContextMenu.Items.Insert(0, ChangeThreadMenuItem);

			threadsGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		private void ChangeThreadMenuItem_Click(object sender, EventArgs e)
		{
			if (threadsGrid.SelectedRows.Count == 1)
			{
				ThreadWrapper thread = (ThreadWrapper)threadsGrid.SelectedRows[0].Tag;

				ChangeCurrentThread(thread);
			}
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			ChangeThreadMenuItem.Enabled = (threadsGrid.SelectedRows.Count == 1);
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			if (threadsGrid.Rows != null)
			{
				foreach (DataGridViewRow row in threadsGrid.Rows)
				{
					DataGridViewCell idCell = row.Cells[0];
					idCell.Value = HelperFunctions.FormatNumber(idCell.Tag);
				}
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			threadsGrid.Rows.Clear();
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			ShowThreads();
		}

		public void Reset()
		{
			GetThreadNameMethod = null;
			HasSearchedForNameMethod = false;
		}

		private void FindGetThreadNameMethod(uint threadTypeToken, ModuleWrapper module)
		{
			TypeDefinition threadType = HelperFunctions.FindObjectByToken(threadTypeToken, module) as TypeDefinition;

			if (threadType != null)
			{
				Property nameProperty = threadType.FindPropertyByName("Name");

				if (nameProperty != null)
				{
					GetThreadNameMethod = HelperFunctions.FindObjectByToken(nameProperty.GetterMethodToken, module) as MethodDefinition;
				}
			}

			HasSearchedForNameMethod = true;
		}

		private void GetThreadName(ThreadWrapper threadWrapper, ValueWrapper threadObject, FrameRefresher threadActiveFrameRefresher)
		{
			if (DebugEventHandler.Instance.State != DebuggerState.DumpDebugging)
			{
				List<ModuleWrapper> modules = threadWrapper.FindModulesByName(GetThreadNameMethod.BaseTypeDefinition.ModuleScope.Assembly.FileName);

				if (modules.Count == 1)
				{
					ModuleWrapper module = modules[0];
					FunctionWrapper getThreadNameFunction = module.GetFunction(GetThreadNameMethod.Token);
					List<ValueWrapper> arguments = new List<ValueWrapper>(1);
					arguments.Add(threadObject);

					EvaluationHandler methodCaller = new EvaluationHandler(threadActiveFrameRefresher);
					BaseEvaluationResult evaluationResult = methodCaller.CallFunction(getThreadNameFunction, arguments);

					if (evaluationResult.IsSuccessful)
					{
						if (evaluationResult.Result != null && (CorElementType)evaluationResult.Result.ElementType == CorElementType.ELEMENT_TYPE_STRING)
						{
							ValueWrapper dereferencedResult = evaluationResult.Result.DereferenceValue();

							if (dereferencedResult != null)
							{
								EvaluatedThreadName = HelperFunctions.ShowEscapeCharacters(dereferencedResult.GetStringValue(), true);
							}
						}
					}
				}
			}
		}

		private void ShowThreads()
		{
			threadsGrid.BeginGridUpdate();
			threadsGrid.Rows.Clear();
			List<ThreadWrapper> threads = DebugEventHandler.Instance.EventObjects.Controller.EnumerateThreads();

			foreach (ThreadWrapper thread in threads)
			{
				EvaluatedThreadName = "<no name>";
				ValueWrapper threadObject = null;
				ValueWrapper dereferencedObject = null;

				if (!HasSearchedForNameMethod)
				{
					threadObject = thread.GetObject();

					if (threadObject != null && !threadObject.IsNull())
					{
						dereferencedObject = threadObject.DereferenceValue();

						if (dereferencedObject != null)
						{
							ClassWrapper threadClass = dereferencedObject.GetClassInformation();
							uint threadTypeToken = threadClass.GetToken();
							ModuleWrapper module = threadClass.GetModule();

							FindGetThreadNameMethod(threadTypeToken, module);
						}
					}
				}

				if (HasSearchedForNameMethod)
				{
					if (GetThreadNameMethod == null)
					{
						EvaluatedThreadName = "<definition of the Thread class is not loaded>";
					}
					else
					{
						if (threadObject == null)
						{
							threadObject = thread.GetObject();

							if (threadObject != null && !threadObject.IsNull())
							{
								dereferencedObject = threadObject.DereferenceValue();
							}
						}

						if (dereferencedObject != null)
						{
							FrameWrapper threadActiveFrame = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging ? thread.Version3.GetActiveFrame() : thread.GetActiveFrame());

							if (threadActiveFrame != null)
							{
								FrameRefresher threadActiveFrameRefresher = new FrameRefresher(thread, threadActiveFrame.ChainIndex, threadActiveFrame.FrameIndex, threadActiveFrame.IsActiveFrame);

								GetThreadName(thread, threadObject, threadActiveFrameRefresher);
							}
						}
					}
				}

				DataGridViewRow row = threadsGrid.Rows[threadsGrid.Rows.Add()];

				uint threadID = thread.GetID();
				DataGridViewCell idCell = row.Cells[0];
				idCell.Tag = threadID;
				idCell.Value = HelperFunctions.FormatNumber(threadID);
				row.Cells[1].Value = EvaluatedThreadName;

				AppDomainWrapper appDomain = thread.GetAppDomain();

				if (appDomain != null)
				{
					row.Cells[2].Value = appDomain.GetName();
				}

				row.Tag = thread;
			}

			threadsGrid.EndGridUpdate();
		}

		private void threadsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (threadsGrid.CurrentRow != null)
			{
				ThreadWrapper thread = (ThreadWrapper)threadsGrid.CurrentRow.Tag;

				ChangeCurrentThread(thread);
			}
		}

		private void ChangeCurrentThread(ThreadWrapper thread)
		{
			UIHandler.Instance.ClearDebugPanels(true);
			UIHandler.Instance.ClearCodeDisplayers(true);
			UIHandler.Instance.ThreadChangedUpdate(thread);
		}
	}
}