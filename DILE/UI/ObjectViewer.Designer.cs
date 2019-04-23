namespace Dile.UI
{
	partial class ObjectViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Missing modules", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Evaluation exceptions", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Object Information", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Public fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Family or assembly fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Family fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Assembly fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Family and assembly fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Private fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Private scope fields/properties", System.Windows.Forms.HorizontalAlignment.Center);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectViewer));
			this.buttonsPanel = new System.Windows.Forms.Panel();
			this.closeButton = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.objectTree = new System.Windows.Forms.TreeView();
			this.fieldList = new Dile.Controls.CustomListView();
			this.fieldNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fieldValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.goUpButton = new System.Windows.Forms.ToolStripButton();
			this.displayHexaNumbersButton = new System.Windows.Forms.ToolStripButton();
			this.evaluateMethodsButton = new System.Windows.Forms.ToolStripButton();
			this.evaluatePanel = new System.Windows.Forms.Panel();
			this.stopEvaluationButton = new System.Windows.Forms.Button();
			this.expressionComboBox = new System.Windows.Forms.ComboBox();
			this.evaluateButton = new System.Windows.Forms.Button();
			this.evaluationLogPanel = new System.Windows.Forms.Panel();
			this.evaluationLogListBox = new System.Windows.Forms.ListBox();
			this.showLogCheckBox = new System.Windows.Forms.CheckBox();
			this.evaluationProgress = new System.Windows.Forms.ProgressBar();
			this.evaluationStepLabel = new System.Windows.Forms.Label();
			this.buttonsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.evaluatePanel.SuspendLayout();
			this.evaluationLogPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonsPanel
			// 
			this.buttonsPanel.Controls.Add(this.closeButton);
			this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonsPanel.Location = new System.Drawing.Point(0, 445);
			this.buttonsPanel.Name = "buttonsPanel";
			this.buttonsPanel.Size = new System.Drawing.Size(845, 28);
			this.buttonsPanel.TabIndex = 2;
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Location = new System.Drawing.Point(758, 3);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "&Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 56);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.objectTree);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.fieldList);
			this.splitContainer1.Size = new System.Drawing.Size(845, 289);
			this.splitContainer1.SplitterDistance = 281;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.Text = "splitContainer1";
			// 
			// objectTree
			// 
			this.objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectTree.HideSelection = false;
			this.objectTree.Location = new System.Drawing.Point(0, 0);
			this.objectTree.Name = "objectTree";
			this.objectTree.Size = new System.Drawing.Size(281, 289);
			this.objectTree.TabIndex = 0;
			this.objectTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.objectTree_BeforeSelect);
			// 
			// fieldList
			// 
			this.fieldList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fieldNameColumn,
            this.fieldValue});
			this.fieldList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fieldList.FullRowSelect = true;
			listViewGroup1.Header = "Missing modules";
			listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup1.Name = "Missing modules";
			listViewGroup2.Header = "Evaluation exceptions";
			listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup2.Name = "Evaluation exceptions";
			listViewGroup3.Header = "Object Information";
			listViewGroup3.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup3.Name = "Object Information";
			listViewGroup4.Header = "Public fields/properties";
			listViewGroup4.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup4.Name = "Public fields/properties";
			listViewGroup5.Header = "Family or assembly fields/properties";
			listViewGroup5.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup5.Name = "Family or assembly fields/properties";
			listViewGroup6.Header = "Family fields/properties";
			listViewGroup6.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup6.Name = "Family fields/properties";
			listViewGroup7.Header = "Assembly fields/properties";
			listViewGroup7.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup7.Name = "Assembly fields/properties";
			listViewGroup8.Header = "Family and assembly fields/properties";
			listViewGroup8.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup8.Name = "Family and assembly fields/properties";
			listViewGroup9.Header = "Private fields/properties";
			listViewGroup9.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup9.Name = "Private fields/properties";
			listViewGroup10.Header = "Private scope fields/properties";
			listViewGroup10.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup10.Name = "Private scope fields/properties";
			this.fieldList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10});
			this.fieldList.Location = new System.Drawing.Point(0, 0);
			this.fieldList.Name = "fieldList";
			this.fieldList.ShowItemToolTips = true;
			this.fieldList.Size = new System.Drawing.Size(560, 289);
			this.fieldList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.fieldList.TabIndex = 0;
			this.fieldList.UseCompatibleStateImageBehavior = false;
			this.fieldList.View = System.Windows.Forms.View.Details;
			this.fieldList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fieldList_MouseDoubleClick);
			// 
			// fieldNameColumn
			// 
			this.fieldNameColumn.Text = "Field name";
			this.fieldNameColumn.Width = 152;
			// 
			// fieldValue
			// 
			this.fieldValue.Text = "Field value";
			this.fieldValue.Width = 387;
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goUpButton,
            this.displayHexaNumbersButton,
            this.evaluateMethodsButton});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(845, 25);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip";
			// 
			// goUpButton
			// 
			this.goUpButton.Image = ((System.Drawing.Image)(resources.GetObject("goUpButton.Image")));
			this.goUpButton.ImageTransparentColor = System.Drawing.Color.White;
			this.goUpButton.Name = "goUpButton";
			this.goUpButton.Size = new System.Drawing.Size(59, 22);
			this.goUpButton.Text = "Go up";
			this.goUpButton.ToolTipText = "Go up 1 level in the hierarchy.";
			this.goUpButton.Click += new System.EventHandler(this.goUpButton_Click);
			// 
			// displayHexaNumbersButton
			// 
			this.displayHexaNumbersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.displayHexaNumbersButton.Image = ((System.Drawing.Image)(resources.GetObject("displayHexaNumbersButton.Image")));
			this.displayHexaNumbersButton.ImageTransparentColor = System.Drawing.Color.White;
			this.displayHexaNumbersButton.Name = "displayHexaNumbersButton";
			this.displayHexaNumbersButton.Size = new System.Drawing.Size(23, 22);
			this.displayHexaNumbersButton.Text = "Display numbers in hexadecimal format";
			this.displayHexaNumbersButton.Click += new System.EventHandler(this.displayHexaNumbersButton_Click);
			// 
			// evaluateMethodsButton
			// 
			this.evaluateMethodsButton.CheckOnClick = true;
			this.evaluateMethodsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.evaluateMethodsButton.Image = ((System.Drawing.Image)(resources.GetObject("evaluateMethodsButton.Image")));
			this.evaluateMethodsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.evaluateMethodsButton.Name = "evaluateMethodsButton";
			this.evaluateMethodsButton.Size = new System.Drawing.Size(23, 22);
			this.evaluateMethodsButton.Text = "Evaluate properties and ToString()";
			this.evaluateMethodsButton.Click += new System.EventHandler(this.evaluateMethodsButton_Click);
			// 
			// evaluatePanel
			// 
			this.evaluatePanel.Controls.Add(this.stopEvaluationButton);
			this.evaluatePanel.Controls.Add(this.expressionComboBox);
			this.evaluatePanel.Controls.Add(this.evaluateButton);
			this.evaluatePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.evaluatePanel.Location = new System.Drawing.Point(0, 25);
			this.evaluatePanel.Name = "evaluatePanel";
			this.evaluatePanel.Size = new System.Drawing.Size(845, 31);
			this.evaluatePanel.TabIndex = 1;
			// 
			// stopEvaluationButton
			// 
			this.stopEvaluationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stopEvaluationButton.Enabled = false;
			this.stopEvaluationButton.Location = new System.Drawing.Point(761, 4);
			this.stopEvaluationButton.Name = "stopEvaluationButton";
			this.stopEvaluationButton.Size = new System.Drawing.Size(75, 23);
			this.stopEvaluationButton.TabIndex = 2;
			this.stopEvaluationButton.Text = "&Stop";
			this.stopEvaluationButton.UseVisualStyleBackColor = true;
			this.stopEvaluationButton.Click += new System.EventHandler(this.stopEvaluationButton_Click);
			// 
			// expressionComboBox
			// 
			this.expressionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.expressionComboBox.FormattingEnabled = true;
			this.expressionComboBox.Location = new System.Drawing.Point(3, 3);
			this.expressionComboBox.Name = "expressionComboBox";
			this.expressionComboBox.Size = new System.Drawing.Size(671, 21);
			this.expressionComboBox.TabIndex = 0;
			// 
			// evaluateButton
			// 
			this.evaluateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.evaluateButton.Location = new System.Drawing.Point(680, 4);
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.Size = new System.Drawing.Size(75, 23);
			this.evaluateButton.TabIndex = 1;
			this.evaluateButton.Text = "E&valuate";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// evaluationLogPanel
			// 
			this.evaluationLogPanel.AutoScroll = true;
			this.evaluationLogPanel.Controls.Add(this.evaluationLogListBox);
			this.evaluationLogPanel.Controls.Add(this.showLogCheckBox);
			this.evaluationLogPanel.Controls.Add(this.evaluationProgress);
			this.evaluationLogPanel.Controls.Add(this.evaluationStepLabel);
			this.evaluationLogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.evaluationLogPanel.Location = new System.Drawing.Point(0, 345);
			this.evaluationLogPanel.Name = "evaluationLogPanel";
			this.evaluationLogPanel.Size = new System.Drawing.Size(845, 100);
			this.evaluationLogPanel.TabIndex = 4;
			// 
			// evaluationLogListBox
			// 
			this.evaluationLogListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.evaluationLogListBox.FormattingEnabled = true;
			this.evaluationLogListBox.Location = new System.Drawing.Point(0, 57);
			this.evaluationLogListBox.Name = "evaluationLogListBox";
			this.evaluationLogListBox.Size = new System.Drawing.Size(845, 43);
			this.evaluationLogListBox.TabIndex = 3;
			// 
			// showLogCheckBox
			// 
			this.showLogCheckBox.Checked = true;
			this.showLogCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showLogCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.showLogCheckBox.Location = new System.Drawing.Point(0, 33);
			this.showLogCheckBox.Name = "showLogCheckBox";
			this.showLogCheckBox.Size = new System.Drawing.Size(845, 24);
			this.showLogCheckBox.TabIndex = 2;
			this.showLogCheckBox.Text = "S&how evaluation log";
			this.showLogCheckBox.UseVisualStyleBackColor = true;
			this.showLogCheckBox.CheckedChanged += new System.EventHandler(this.showLogCheckBox_CheckedChanged);
			// 
			// evaluationProgress
			// 
			this.evaluationProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.evaluationProgress.Location = new System.Drawing.Point(0, 13);
			this.evaluationProgress.Name = "evaluationProgress";
			this.evaluationProgress.Size = new System.Drawing.Size(845, 20);
			this.evaluationProgress.Step = 1;
			this.evaluationProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.evaluationProgress.TabIndex = 0;
			// 
			// evaluationStepLabel
			// 
			this.evaluationStepLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.evaluationStepLabel.Location = new System.Drawing.Point(0, 0);
			this.evaluationStepLabel.Name = "evaluationStepLabel";
			this.evaluationStepLabel.Size = new System.Drawing.Size(845, 13);
			this.evaluationStepLabel.TabIndex = 1;
			// 
			// ObjectViewer
			// 
			this.AcceptButton = this.evaluateButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(845, 473);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.evaluationLogPanel);
			this.Controls.Add(this.buttonsPanel);
			this.Controls.Add(this.evaluatePanel);
			this.Controls.Add(this.toolStrip);
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "ObjectViewer";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Object Viewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObjectViewer_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ObjectViewer_KeyDown);
			this.buttonsPanel.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.evaluatePanel.ResumeLayout(false);
			this.evaluationLogPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel buttonsPanel;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView objectTree;
		private Dile.Controls.CustomListView fieldList;
		private System.Windows.Forms.ColumnHeader fieldNameColumn;
		private System.Windows.Forms.ColumnHeader fieldValue;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton goUpButton;
		private System.Windows.Forms.ToolStripButton displayHexaNumbersButton;
		private System.Windows.Forms.Panel evaluatePanel;
		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.ComboBox expressionComboBox;
		private System.Windows.Forms.Button stopEvaluationButton;
		private System.Windows.Forms.Panel evaluationLogPanel;
		private System.Windows.Forms.ListBox evaluationLogListBox;
		private System.Windows.Forms.CheckBox showLogCheckBox;
		private System.Windows.Forms.ProgressBar evaluationProgress;
		private System.Windows.Forms.Label evaluationStepLabel;
		private System.Windows.Forms.ToolStripButton evaluateMethodsButton;

	}
}