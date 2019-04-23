using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public class ProjectExplorer : BasePanel
	{
		private TreeView projectElements;
		private ContextMenuStrip projectMenu;
		private ToolStripMenuItem addAssemblyMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem projectPropertiesMenuItem;
		private ContextMenuStrip assemblyMenu;
		private ToolStripMenuItem removeAssemblyMenuItem;
		private ContextMenuStrip assemblyReferenceMenu;
		private ToolStripMenuItem openReferenceInProjectMenuItem;
		private ToolStripMenuItem setAsStartupAssemblyMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem reloadAssemblyMenuItem;
		private ToolStripMenuItem reloadAllAssembliesMenuItem;
		private ToolStripMenuItem assemblyPathMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem assemblyReferencePathMenuItem;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem copyPathMenuItem;
		private ToolStripMenuItem displayPathMenuItem;
		private ToolStripMenuItem copyAssemblyReferencePathToClipboardMenuItem;
		private ToolStripMenuItem displayAssemblyReferencePathMenuItem;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem locateEntryMethodMenuItem;
		private ToolStripMenuItem displayEntryMethodMenuItem;
		private System.ComponentModel.IContainer components = null;

		public ProjectExplorer()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.projectElements = new System.Windows.Forms.TreeView();
			this.assemblyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.assemblyPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.setAsStartupAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.locateEntryMethodMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayEntryMethodMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.reloadAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.reloadAllAssembliesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectPropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.assemblyReferenceMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.assemblyReferencePathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.copyAssemblyReferencePathToClipboardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayAssemblyReferencePathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.openReferenceInProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.assemblyMenu.SuspendLayout();
			this.projectMenu.SuspendLayout();
			this.assemblyReferenceMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// projectElements
			// 
			this.projectElements.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectElements.HideSelection = false;
			this.projectElements.Location = new System.Drawing.Point(0, 0);
			this.projectElements.Name = "projectElements";
			this.projectElements.ShowNodeToolTips = true;
			this.projectElements.Size = new System.Drawing.Size(292, 273);
			this.projectElements.Sorted = true;
			this.projectElements.TabIndex = 0;
			this.projectElements.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.projectElements_BeforeExpand);
			this.projectElements.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.projectElements_NodeMouseDoubleClick);
			this.projectElements.MouseDown += new System.Windows.Forms.MouseEventHandler(this.projectElements_MouseDown);
			// 
			// assemblyMenu
			// 
			this.assemblyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblyPathMenuItem,
            this.copyPathMenuItem,
            this.displayPathMenuItem,
            this.toolStripMenuItem1,
            this.setAsStartupAssemblyMenuItem,
            this.locateEntryMethodMenuItem,
            this.displayEntryMethodMenuItem,
            this.toolStripSeparator2,
            this.reloadAssemblyMenuItem,
            this.removeAssemblyMenuItem});
			this.assemblyMenu.Name = "assemblyMenu";
			this.assemblyMenu.Size = new System.Drawing.Size(283, 214);
			this.assemblyMenu.Opening += new System.ComponentModel.CancelEventHandler(this.assemblyMenu_Opening);
			// 
			// assemblyPathMenuItem
			// 
			this.assemblyPathMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
			this.assemblyPathMenuItem.Name = "assemblyPathMenuItem";
			this.assemblyPathMenuItem.Size = new System.Drawing.Size(282, 22);
			// 
			// copyPathMenuItem
			// 
			this.copyPathMenuItem.Name = "copyPathMenuItem";
			this.copyPathMenuItem.Size = new System.Drawing.Size(282, 22);
			this.copyPathMenuItem.Text = "Copy path to clipboard";
			this.copyPathMenuItem.Click += new System.EventHandler(this.copyPathMenuItem_Click);
			// 
			// displayPathMenuItem
			// 
			this.displayPathMenuItem.Name = "displayPathMenuItem";
			this.displayPathMenuItem.Size = new System.Drawing.Size(282, 22);
			this.displayPathMenuItem.Text = "Display path...";
			this.displayPathMenuItem.Click += new System.EventHandler(this.displayPathMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(279, 6);
			// 
			// setAsStartupAssemblyMenuItem
			// 
			this.setAsStartupAssemblyMenuItem.Name = "setAsStartupAssemblyMenuItem";
			this.setAsStartupAssemblyMenuItem.Size = new System.Drawing.Size(282, 22);
			this.setAsStartupAssemblyMenuItem.Text = "Set as startup assembly";
			this.setAsStartupAssemblyMenuItem.Click += new System.EventHandler(this.setAsStartupAssemblyMenuItem_Click);
			// 
			// locateEntryMethodMenuItem
			// 
			this.locateEntryMethodMenuItem.Name = "locateEntryMethodMenuItem";
			this.locateEntryMethodMenuItem.Size = new System.Drawing.Size(282, 22);
			this.locateEntryMethodMenuItem.Text = "Locate entry method in Project Explorer";
			this.locateEntryMethodMenuItem.Click += new System.EventHandler(this.locateEntryMethodMenuItem_Click);
			// 
			// displayEntryMethodMenuItem
			// 
			this.displayEntryMethodMenuItem.Name = "displayEntryMethodMenuItem";
			this.displayEntryMethodMenuItem.Size = new System.Drawing.Size(282, 22);
			this.displayEntryMethodMenuItem.Text = "Display entry method";
			this.displayEntryMethodMenuItem.Click += new System.EventHandler(this.displayEntryMethodMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(279, 6);
			// 
			// reloadAssemblyMenuItem
			// 
			this.reloadAssemblyMenuItem.Name = "reloadAssemblyMenuItem";
			this.reloadAssemblyMenuItem.Size = new System.Drawing.Size(282, 22);
			this.reloadAssemblyMenuItem.Text = "Reload assembly";
			this.reloadAssemblyMenuItem.Click += new System.EventHandler(this.reloadAssemblyMenuItem_Click);
			// 
			// removeAssemblyMenuItem
			// 
			this.removeAssemblyMenuItem.Name = "removeAssemblyMenuItem";
			this.removeAssemblyMenuItem.Size = new System.Drawing.Size(282, 22);
			this.removeAssemblyMenuItem.Text = "Remove assembly";
			this.removeAssemblyMenuItem.Click += new System.EventHandler(this.removeAssemblyMenuItem_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAssemblyMenuItem,
            this.toolStripSeparator1,
            this.reloadAllAssembliesMenuItem,
            this.projectPropertiesMenuItem});
			this.projectMenu.Name = "projectMenu";
			this.projectMenu.Size = new System.Drawing.Size(186, 76);
			// 
			// addAssemblyMenuItem
			// 
			this.addAssemblyMenuItem.Name = "addAssemblyMenuItem";
			this.addAssemblyMenuItem.Size = new System.Drawing.Size(185, 22);
			this.addAssemblyMenuItem.Text = "Add assembly...";
			this.addAssemblyMenuItem.Click += new System.EventHandler(this.addAssemblyMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
			// 
			// reloadAllAssembliesMenuItem
			// 
			this.reloadAllAssembliesMenuItem.Name = "reloadAllAssembliesMenuItem";
			this.reloadAllAssembliesMenuItem.Size = new System.Drawing.Size(185, 22);
			this.reloadAllAssembliesMenuItem.Text = "Reload all assemblies";
			this.reloadAllAssembliesMenuItem.Click += new System.EventHandler(this.reloadAllAssembliesMenuItem_Click);
			// 
			// projectPropertiesMenuItem
			// 
			this.projectPropertiesMenuItem.Name = "projectPropertiesMenuItem";
			this.projectPropertiesMenuItem.Size = new System.Drawing.Size(185, 22);
			this.projectPropertiesMenuItem.Text = "Properties...";
			this.projectPropertiesMenuItem.Click += new System.EventHandler(this.projectPropertiesMenuItem_Click);
			// 
			// assemblyReferenceMenu
			// 
			this.assemblyReferenceMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblyReferencePathMenuItem,
            this.toolStripMenuItem2,
            this.copyAssemblyReferencePathToClipboardMenuItem,
            this.displayAssemblyReferencePathMenuItem,
            this.toolStripMenuItem3,
            this.openReferenceInProjectMenuItem});
			this.assemblyReferenceMenu.Name = "assemblyReferenceMenu";
			this.assemblyReferenceMenu.Size = new System.Drawing.Size(209, 126);
			this.assemblyReferenceMenu.Opening += new System.ComponentModel.CancelEventHandler(this.assemblyReferenceMenu_Opening);
			// 
			// assemblyReferencePathMenuItem
			// 
			this.assemblyReferencePathMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
			this.assemblyReferencePathMenuItem.Name = "assemblyReferencePathMenuItem";
			this.assemblyReferencePathMenuItem.Size = new System.Drawing.Size(208, 22);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
			// 
			// copyAssemblyReferencePathToClipboardMenuItem
			// 
			this.copyAssemblyReferencePathToClipboardMenuItem.Name = "copyAssemblyReferencePathToClipboardMenuItem";
			this.copyAssemblyReferencePathToClipboardMenuItem.Size = new System.Drawing.Size(208, 22);
			this.copyAssemblyReferencePathToClipboardMenuItem.Text = "Copy path to clipboard";
			this.copyAssemblyReferencePathToClipboardMenuItem.Click += new System.EventHandler(this.copyAssemblyReferencePathToClipboardMenuItem_Click);
			// 
			// displayAssemblyReferencePathMenuItem
			// 
			this.displayAssemblyReferencePathMenuItem.Name = "displayAssemblyReferencePathMenuItem";
			this.displayAssemblyReferencePathMenuItem.Size = new System.Drawing.Size(208, 22);
			this.displayAssemblyReferencePathMenuItem.Text = "Display path...";
			this.displayAssemblyReferencePathMenuItem.Click += new System.EventHandler(this.displayAssemblyReferencePathMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
			// 
			// openReferenceInProjectMenuItem
			// 
			this.openReferenceInProjectMenuItem.Name = "openReferenceInProjectMenuItem";
			this.openReferenceInProjectMenuItem.Size = new System.Drawing.Size(208, 22);
			this.openReferenceInProjectMenuItem.Text = "Open reference in project";
			this.openReferenceInProjectMenuItem.Click += new System.EventHandler(this.openReferenceInProjectMenuItem_Click);
			// 
			// ProjectExplorer
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.projectElements);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ProjectExplorer";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
			this.TabText = "Project explorer";
			this.assemblyMenu.ResumeLayout(false);
			this.projectMenu.ResumeLayout(false);
			this.assemblyReferenceMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private bool wordWrap = true;
		public bool WordWrap
		{
			get
			{
				return wordWrap;
			}
			set
			{
				wordWrap = value;
			}
		}

		public TreeView ProjectElements
		{
			get
			{
				return projectElements;
			}
		}

		private NoArgumentsDelegate addAssemblyDelegate;
		public NoArgumentsDelegate AddAssemblyDelegate
		{
			get
			{
				return addAssemblyDelegate;
			}
			set
			{
				addAssemblyDelegate = value;
			}
		}

		private List<CodeDisplayer> codeDisplayers = new List<CodeDisplayer>();
		public List<CodeDisplayer> CodeDisplayers
		{
			get
			{
				return codeDisplayers;
			}
			set
			{
				codeDisplayers = value;
			}
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		public void RemoveUnnecessaryAssemblies()
		{
			if (ProjectElements.Nodes.Count == 1)
			{
				ProjectElements.BeginUpdate();
				int index = 0;
				TreeNodeCollection assemblyNodes = ProjectElements.Nodes[0].Nodes;

				while (index < assemblyNodes.Count)
				{
					TreeNode assemblyNode = assemblyNodes[index];
					Assembly assembly = assemblyNode.Tag as Assembly;

					if (assembly != null && !Project.Instance.Assemblies.Contains(assembly))
					{
						assemblyNode.Remove();
					}
					else
					{
						index++;
					}
				}

				ProjectElements.EndUpdate();
			}
		}

		public void ShowProject()
		{
			Text = string.Format("Project Explorer - {0}", Project.Instance.Name);
			projectElements.BeginUpdate();
			projectElements.Nodes.Clear();

			TreeNode projectNode = new TreeNode(HelperFunctions.TruncateText(Project.Instance.Name));
			projectNode.ContextMenuStrip = projectMenu;
			projectNode.Tag = Project.Instance;

			foreach (Assembly assembly in Project.Instance.Assemblies)
			{
				ShowAssembly(projectNode, assembly);
			}

			projectElements.Nodes.Add(projectNode);
			projectElements.EndUpdate();
		}

		public void AddAssemblyToProject(Assembly assembly)
		{
			projectElements.BeginUpdate();

			ShowAssembly(projectElements.Nodes[0], assembly);
			Project.Instance.Assemblies.Add(assembly);
			if (!assembly.LoadedFromMemory)
			{
				Project.Instance.IsSaved = false;
			}

			projectElements.EndUpdate();
		}

		private void ShowAssembly(TreeNode projectNode, Assembly assembly)
		{
			if (assembly != null)
			{
				TreeNode assemblyNode = new TreeNode(HelperFunctions.TruncateText(assembly.FileName));
				assemblyNode.ContextMenuStrip = assemblyMenu;

				if (!assembly.LoadedFromMemory && string.Compare(Project.Instance.StartupAssemblyPath, assembly.FullPath, true) == 0)
				{
					assemblyNode.ForeColor = Color.Red;
					assemblyNode.NodeFont = new Font(ProjectElements.Font, FontStyle.Italic);
				}

				assemblyNode.Tag = assembly;
				projectNode.Nodes.Add(assemblyNode);

				if (assembly.DisplayInTree)
				{
					TreeNode assemblyDefinitionNode = new TreeNode(" definition");
					assemblyDefinitionNode.Tag = assembly;
					assemblyNode.Nodes.Add(assemblyDefinitionNode);
				}

				if (assembly.AssemblyReferences != null)
				{
					TreeNode referencesNode = new TreeNode(" References");
					referencesNode.Tag = assembly;
					assemblyNode.Nodes.Add(referencesNode);

					foreach (AssemblyReference reference in assembly.AssemblyReferences.Values)
					{
						TreeNode referenceNode = new TreeNode(HelperFunctions.TruncateText(reference.Name));
						referenceNode.ContextMenuStrip = assemblyReferenceMenu;
						referenceNode.Tag = reference;
						referencesNode.Nodes.Add(referenceNode);
					}
				}

				if (assembly.ManifestResources != null)
				{
					TreeNode manifestResourcesNode = new TreeNode(" Manifest Resources");
					assemblyNode.Nodes.Add(manifestResourcesNode);

					foreach (ManifestResource manifestResource in assembly.ManifestResources)
					{
						TreeNode manifestResourceNode = new TreeNode(HelperFunctions.TruncateText(manifestResource.Name));
						manifestResourceNode.Tag = manifestResource;
						manifestResourcesNode.Nodes.Add(manifestResourceNode);
					}
				}

				if (assembly.Files != null)
				{
					TreeNode filesNode = new TreeNode(" Files");
					assemblyNode.Nodes.Add(filesNode);

					foreach (File file in assembly.Files)
					{
						if (file.DisplayInTree)
						{
							TreeNode fileNode = new TreeNode(HelperFunctions.TruncateText(file.Name));
							fileNode.Tag = file;
							filesNode.Nodes.Add(fileNode);
						}
					}
				}

				if (assembly.ModuleReferences != null)
				{
					TreeNode moduleReferencesNode = new TreeNode(" Module References");
					assemblyNode.Nodes.Add(moduleReferencesNode);

					foreach (ModuleReference moduleReference in assembly.ModuleReferences)
					{
						TreeNode moduleReferenceNode = new TreeNode(HelperFunctions.TruncateText(moduleReference.Name));
						moduleReferenceNode.Tag = moduleReference;
						moduleReferencesNode.Nodes.Add(moduleReferenceNode);
					}
				}

				if (assembly.ExportedTypes != null)
				{
					TreeNode exportedTypesNode = new TreeNode(" Exported Types");
					assemblyNode.Nodes.Add(exportedTypesNode);

					foreach (ExportedType exportedType in assembly.ExportedTypes)
					{
						TreeNode exportedTypeNode = new TreeNode(HelperFunctions.TruncateText(exportedType.Name));
						exportedTypeNode.Tag = exportedType;
						exportedTypesNode.Nodes.Add(exportedTypeNode);
					}
				}

				TreeNode moduleScopeNode = new TreeNode(HelperFunctions.TruncateText(assembly.ModuleScope.Name));
				assemblyNode.Nodes.Add(moduleScopeNode);

				TreeNode moduleScopeDefinitionNode = new TreeNode(" definition");
				moduleScopeDefinitionNode.Tag = assembly.ModuleScope;
				moduleScopeNode.Nodes.Add(moduleScopeDefinitionNode);
				Dictionary<string, TreeNode> namespaceNodes = new Dictionary<string, TreeNode>();

				foreach (TypeDefinition typeDefinition in assembly.ModuleScope.TypeDefinitions.Values)
				{
					CreateTypeDefinitionNode(namespaceNodes, moduleScopeNode, typeDefinition, true);
				}

				if (assembly.GlobalType.FieldDefinitions != null || assembly.GlobalType.MethodDefinitions != null || assembly.GlobalType.Properties != null)
				{
					CreateTypeDefinitionNode(namespaceNodes, moduleScopeNode, assembly.GlobalType, false).Text = " {global type}";
				}
			}
		}

		private TreeNode CreateTypeDefinitionNode(Dictionary<string, TreeNode> namespaceNodes, TreeNode moduleScopeNode, TypeDefinition typeDefinition, bool createDefinitionNode)
		{
			TreeNode result = new TreeNode(HelperFunctions.TruncateText(typeDefinition.FullName));
			string typeNamespace = typeDefinition.Namespace;

			if (typeNamespace.Length == 0)
			{
				typeNamespace = Constants.DefaultNamespaceName;
			}

			if (namespaceNodes.ContainsKey(typeNamespace))
			{
				namespaceNodes[typeNamespace].Nodes.Add(result);
			}
			else
			{
				TreeNode namespaceNode = new TreeNode(typeNamespace);
				moduleScopeNode.Nodes.Add(namespaceNode);
				namespaceNodes[typeNamespace] = namespaceNode;

				namespaceNode.Nodes.Add(result);
			}

			if (createDefinitionNode)
			{
				TreeNode classNode = new TreeNode("definition");
				classNode.Tag = typeDefinition;
				result.Nodes.Add(classNode);
			}
			else
			{
				CreateTypeDefinitionSubnodes(typeDefinition, result);
				result.Tag = true;
			}

			return result;
		}

		private static void CreateTypeDefinitionSubnodes(TypeDefinition typeDefinition, TreeNode typeDefinitionNode)
		{
			Assembly assembly = typeDefinition.ModuleScope.Assembly;
			typeDefinition.LazyInitialize(assembly.AllTokens);
			Dictionary<uint, MethodDefinition> methodDefinitions = null;

			if (typeDefinition.MethodDefinitions != null)
			{
				methodDefinitions = new Dictionary<uint, MethodDefinition>(typeDefinition.MethodDefinitions);
			}

			if (typeDefinition.FieldDefinitions != null)
			{
				TreeNode fieldsNode = new TreeNode("Fields");
				typeDefinitionNode.Nodes.Add(fieldsNode);

				foreach (FieldDefinition field in typeDefinition.FieldDefinitions.Values)
				{
					field.LazyInitialize(assembly.AllTokens);
					TreeNode fieldNode = new TreeNode(HelperFunctions.TruncateText(field.Name));
					fieldNode.Tag = field;
					fieldsNode.Nodes.Add(fieldNode);
				}
			}

			if (typeDefinition.Properties != null)
			{
				TreeNode propertiesNode = new TreeNode("Properties");
				typeDefinitionNode.Nodes.Add(propertiesNode);

				foreach (Property property in typeDefinition.Properties.Values)
				{
					property.LazyInitialize(assembly.AllTokens);
					TreeNode propertyNode = new TreeNode(HelperFunctions.TruncateText(property.Name));
					propertiesNode.Nodes.Add(propertyNode);

					TreeNode definitionNode = new TreeNode(" definition");
					definitionNode.Tag = property;
					propertyNode.Nodes.Add(definitionNode);

					if (methodDefinitions != null)
					{
						DisplayAssociatedMethod(assembly, methodDefinitions, property.GetterMethodToken, propertyNode);
						DisplayAssociatedMethod(assembly, methodDefinitions, property.SetterMethodToken, propertyNode);

						for (int index = 0; index < property.OtherMethodsCount; index++)
						{
							uint token = property.OtherMethods[index];

							DisplayAssociatedMethod(assembly, methodDefinitions, token, propertyNode);
						}
					}
				}
			}

			if (typeDefinition.EventDefinitions != null)
			{
				TreeNode eventsNode = new TreeNode("Events");
				typeDefinitionNode.Nodes.Add(eventsNode);

				foreach (EventDefinition eventDefinition in typeDefinition.EventDefinitions.Values)
				{
					eventDefinition.LazyInitialize(assembly.AllTokens);
					TreeNode eventNode = new TreeNode(HelperFunctions.TruncateText(eventDefinition.Name));
					eventsNode.Nodes.Add(eventNode);

					TreeNode definitionNode = new TreeNode(" definition");
					definitionNode.Tag = eventDefinition;
					eventNode.Nodes.Add(definitionNode);

					if (methodDefinitions != null)
					{
						DisplayAssociatedMethod(assembly, methodDefinitions, eventDefinition.AddOnMethodToken, eventNode);
						DisplayAssociatedMethod(assembly, methodDefinitions, eventDefinition.RemoveOnMethodToken, eventNode);
						DisplayAssociatedMethod(assembly, methodDefinitions, eventDefinition.FireMethodToken, eventNode);

						for (int index = 0; index < eventDefinition.OtherMethodsCount; index++)
						{
							uint token = eventDefinition.OtherMethods[index];

							DisplayAssociatedMethod(assembly, methodDefinitions, token, eventNode);
						}
					}
				}
			}

			if (methodDefinitions != null && methodDefinitions.Count > 0)
			{
				TreeNode methodsNode = new TreeNode("Methods");
				typeDefinitionNode.Nodes.Add(methodsNode);

				foreach (MethodDefinition method in methodDefinitions.Values)
				{
					method.LazyInitialize(assembly.AllTokens);
					TreeNode methodNode = new TreeNode(HelperFunctions.TruncateText(method.DisplayName));
					methodNode.Tag = method;
					methodsNode.Nodes.Add(methodNode);
				}
			}
		}

		private static void DisplayAssociatedMethod(Assembly assembly, Dictionary<uint, MethodDefinition> methodDefinitions, uint methodToken, TreeNode parentNode)
		{
			if (methodDefinitions.ContainsKey(methodToken))
			{
				MethodDefinition method = methodDefinitions[methodToken];
				method.LazyInitialize(assembly.AllTokens);

				TreeNode methodNode = new TreeNode(HelperFunctions.TruncateText(method.DisplayName));
				methodNode.Tag = method;
				parentNode.Nodes.Add(methodNode);

				methodDefinitions.Remove(methodToken);
			}
		}

		private void projectElements_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			IMultiLine codeObject = projectElements.SelectedNode.Tag as IMultiLine;

			if (codeObject != null && projectElements.SelectedNode.Nodes.Count == 0)
			{
				ShowCodeObject(codeObject, new CodeObjectDisplayOptions());
			}
		}

		public void UpdateBreakpoint(IMultiLine codeObject, BreakpointInformation breakpointInformation)
		{
			CodeDisplayer codeDisplayer = FindCodeDisplayer(codeObject);

			if (codeDisplayer != null)
			{
				codeDisplayer.UpdateBreakpoint(breakpointInformation);
			}
		}

		public void ShowCodeObject(IMultiLine codeObject, CodeObjectDisplayOptions options)
		{
			CodeDisplayer codeDisplayer = FindCodeDisplayer(codeObject);

			if (codeDisplayer == null)
			{
				CodeEditorForm codeEditorForm = new CodeEditorForm();
				codeEditorForm.UpdateFont(Settings.Instance.CodeEditorFont.Font);
				codeEditorForm.SetWordWrap(WordWrap);

				CodeDisplayer displayer = new CodeDisplayer(DockPanel, codeObject, codeEditorForm);
				CodeDisplayers.Add(displayer);
				displayer.ShowCodeObject(options);
			}
			else
			{
				codeDisplayer.ShowCodeObject(options);
			}
		}

		private CodeDisplayer FindCodeDisplayer(IMultiLine codeObject)
		{
			CodeDisplayer result = null;
			int index = 0;

			while (result == null && index < CodeDisplayers.Count)
			{
				CodeDisplayer displayer = CodeDisplayers[index++];

				if (displayer.CodeObject == codeObject)
				{
					result = displayer;
				}
			}

			return result;
		}

		private void removeAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is Assembly)
			{
				Assembly assembly = (Assembly)projectElements.SelectedNode.Nodes[0].Tag;

				Project.Instance.RemoveAssemblyRelatedBreakpoints(assembly);
				Project.Instance.Assemblies.Remove(assembly);
				Project.Instance.IsSaved = false;
				projectElements.Nodes.Remove(projectElements.SelectedNode);
			}
		}

		private void openReferenceInProjectMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Tag is AssemblyReference)
			{
				AssemblyReference assemblyReference = (AssemblyReference)projectElements.SelectedNode.Tag;

				UIHandler.Instance.AddAssembly(assemblyReference.FullPath);
			}
		}

		private void addAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			AddAssemblyDelegate();
		}

		private void projectPropertiesMenuItem_Click(object sender, EventArgs e)
		{
			ProjectProperties properties = new ProjectProperties();

			if (properties.DisplaySettings() == DialogResult.OK)
			{
				projectElements.Nodes[0].Text = HelperFunctions.TruncateText(Project.Instance.Name);
			}
		}

		private void setAsStartupAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is Assembly)
			{
				Assembly assembly = (Assembly)ProjectElements.SelectedNode.Nodes[0].Tag;

				foreach (TreeNode node in ProjectElements.Nodes[0].Nodes)
				{
					node.ForeColor = SystemColors.WindowText;
					node.NodeFont = ProjectElements.Font;
				}

				Project.Instance.StartupAssembly = assembly;
				Project.Instance.IsSaved = false;
				ProjectElements.SelectedNode.ForeColor = Color.Red;
				ProjectElements.SelectedNode.NodeFont = new Font(ProjectElements.Font, FontStyle.Italic);
				UIHandler.Instance.ShowDebuggerState(DebugEventHandler.Instance.State);
			}
		}

		public void LocateTokenNode(TokenBase tokenObject)
		{
			ProjectElements.SelectedNode = TreeViewSearcher.LocateNode(ProjectElements.Nodes[0], tokenObject);
			Activate();
		}

		private void reloadAssemblyMenuItem_Click(object sender, EventArgs e)
		{
			if (ProjectElements.SelectedNode != null && ProjectElements.SelectedNode.Nodes != null && ProjectElements.SelectedNode.Nodes.Count > 0)
			{
				TreeNode assemblyNode = ProjectElements.SelectedNode.Nodes[0];
				Assembly assembly = assemblyNode.Tag as Assembly;

				if (assembly != null)
				{
					ProjectElements.SelectedNode.Remove();
					Project.Instance.Assemblies.Remove(assembly);
					UIHandler.Instance.AddAssembly(assembly.FullPath);
				}
			}
		}

		private void reloadAllAssembliesMenuItem_Click(object sender, EventArgs e)
		{
			if (Project.Instance.Assemblies != null && Project.Instance.Assemblies.Count > 0)
			{
				AssemblyLoadRequest[] requestedAssemblies = new AssemblyLoadRequest[Project.Instance.Assemblies.Count];

				for (int index = 0; index < Project.Instance.Assemblies.Count; index++)
				{
					requestedAssemblies[index] = new AssemblyLoadRequest(Project.Instance.Assemblies[index].FullPath);
				}

				ProjectElements.Nodes[0].Nodes.Clear();
				Project.Instance.Assemblies.Clear();
				UIHandler.Instance.AddAssembly(requestedAssemblies);
			}
		}

		private void assemblyMenu_Opening(object sender, CancelEventArgs e)
		{
			string assemblyPath = string.Empty;

			if (ProjectElements.SelectedNode != null && ProjectElements.SelectedNode.Nodes != null && ProjectElements.SelectedNode.Nodes.Count > 0)
			{
				Assembly assembly = ProjectElements.SelectedNode.Nodes[0].Tag as Assembly;

				if (assembly != null)
				{
					if (assembly.IsInMemory)
					{
						assemblyPath = string.Empty;
					}
					else
					{
						assemblyPath = assembly.FullPath;
					}

					bool entryPointMethodFound = false;
					TokenBase entryPoint;
					if (assembly.AllTokens.TryGetValue(assembly.EntryPointToken, out entryPoint))
					{
						MethodDefinition entryPointMethod = entryPoint as MethodDefinition;
						if (entryPointMethod != null && entryPointMethod.EntryPoint)
						{
							entryPointMethodFound = true;
						}
					}

					foreach (ToolStripItem menuItem in assemblyMenu.Items)
					{
						if (menuItem != removeAssemblyMenuItem)
						{
							menuItem.Visible = !assembly.LoadedFromMemory;

							if ((menuItem == locateEntryMethodMenuItem || menuItem == displayEntryMethodMenuItem)
								&& !assembly.LoadedFromMemory)
							{
								menuItem.Visible = entryPointMethodFound;
							}
						}
					}
				}
			}

			assemblyPathMenuItem.Text = assemblyPath;
		}

		private void assemblyReferenceMenu_Opening(object sender, CancelEventArgs e)
		{
			string assemblyReferencePath = string.Empty;

			if (ProjectElements.SelectedNode != null)
			{
				AssemblyReference assemblyReference = ProjectElements.SelectedNode.Tag as AssemblyReference;

				if (assemblyReference != null)
				{
					assemblyReferencePath = assemblyReference.FullPath;
				}
			}

			assemblyReferencePathMenuItem.Text = assemblyReferencePath;
		}

		private void projectElements_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node != null && e.Node.Tag == null && e.Node.Nodes != null && e.Node.Nodes.Count >= 1)
			{
				TypeDefinition typeDefinition = e.Node.Nodes[0].Tag as TypeDefinition;

				if (typeDefinition != null)
				{
					CreateTypeDefinitionSubnodes(typeDefinition, e.Node);
					e.Node.Tag = true;
				}
			}
		}

		private void projectElements_MouseDown(object sender, MouseEventArgs e)
		{
			ProjectElements.SelectedNode = ProjectElements.GetNodeAt(e.X, e.Y);
		}

		private void copyPathMenuItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(assemblyPathMenuItem.Text);
		}

		private void displayPathMenuItem_Click(object sender, EventArgs e)
		{
			TextDisplayer.Instance.ShowText(assemblyPathMenuItem.Text);
		}

		private void copyAssemblyReferencePathToClipboardMenuItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(assemblyReferencePathMenuItem.Text);
		}

		private void displayAssemblyReferencePathMenuItem_Click(object sender, EventArgs e)
		{
			TextDisplayer.Instance.ShowText(assemblyReferencePathMenuItem.Text);
		}

		private void locateEntryMethodMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is Assembly)
			{
				Assembly assembly = (Assembly)ProjectElements.SelectedNode.Nodes[0].Tag;
				MethodDefinition entryPoint = assembly.AllTokens[assembly.EntryPointToken] as MethodDefinition;

				if (entryPoint != null && entryPoint.EntryPoint)
				{
					LocateTokenNode(entryPoint);
				}
			}
		}

		private void displayEntryMethodMenuItem_Click(object sender, EventArgs e)
		{
			if (projectElements.SelectedNode != null && projectElements.SelectedNode.Nodes.Count > 0 && projectElements.SelectedNode.Nodes[0].Tag is Assembly)
			{
				Assembly assembly = (Assembly)ProjectElements.SelectedNode.Nodes[0].Tag;
				MethodDefinition entryPoint = assembly.AllTokens[assembly.EntryPointToken] as MethodDefinition;

				if (entryPoint != null && entryPoint.EntryPoint)
				{
					ShowCodeObject(entryPoint, new CodeObjectDisplayOptions());
				}
			}
		}
	}
}