using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using System.Collections;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public partial class DocumentSelectorForm : Form
	{
		private DockPanel dockPanel;
		public DockPanel DockPanel
		{
			get
			{
				return dockPanel;
			}
			set
			{
				dockPanel = value;
			}
		}

		private List<HighlightLabel> labels;
		private List<HighlightLabel> Labels
		{
			get
			{
				return labels;
			}
			set
			{
				labels = value;
			}
		}

		private IList<DocumentContent> ActiveDocuments
		{
			get;
			set;
		}

		public DocumentSelectorForm()
		{
			InitializeComponent();
		}

		private void AddDocuments(Graphics graphics)
		{
			int documentIndex = 0;
			bool isActiveContentDocument = DockPanel.ActiveContent is DocumentContent;

			for (int index = 0; index < ActiveDocuments.Count; index++)
			{
				DocumentContent documentContent = ActiveDocuments[index];

				if (documentContent.VisibleState != DockState.Hidden)
				{
					HighlightLabel label = CreateLabel(graphics, documentContent);

					Labels.Add(label);
					documentsPanel.Controls.Add(label, 1, documentIndex++);

					if ((isActiveContentDocument && index == 1) || index == 0)
					{
						documentsPanel.Tag = label;
					}
				}
			}
		}

		private void AddPanels(Graphics graphics)
		{
			int panelIndex = 0;

			foreach (DockContent content in DockPanel.Contents)
			{
				if (content.VisibleState != DockState.Hidden && content.DockState != DockState.Document && content.DockState != DockState.Hidden)
				{
					HighlightLabel label = CreateLabel(graphics, content);

					Labels.Add(label);
					documentsPanel.Controls.Add(label, 0, panelIndex++);

					if (ActiveDocuments.Count == 0 && content == DockPanel.ActiveContent)
					{
						documentsPanel.Tag = label;
					}
				}
			}
		}

		private HighlightLabel CreateLabel(Graphics graphics, DockContent content)
		{
			HighlightLabel result = new HighlightLabel();

			result.Dock = DockStyle.Fill;
			result.Tag = content;
			result.Text = content.TabText;
			result.TextAlign = ContentAlignment.MiddleLeft;

			SizeF measuredSize = graphics.MeasureString(result.Text, result.Font);
			result.Height = 17;
			result.Width = Convert.ToInt32(measuredSize.Width) + 5;

			result.Click += new EventHandler(contentLabel_Click);

			return result;
		}

		private void contentLabel_Click(object sender, EventArgs e)
		{
			documentsPanel.Tag = sender;
			DisplayChosenContent();
		}

		public void Display(Form owner, IList<DocumentContent> activeDocuments)
		{
			ActiveDocuments = activeDocuments;

			if (DockPanel != null)
			{
				documentsPanel.Controls.Clear();
				Labels = new List<HighlightLabel>(ActiveDocuments.Count + DockPanel.Contents.Count);

				using (Graphics graphics = CreateGraphics())
				{
					AddPanels(graphics);
					AddDocuments(graphics);
				}

				Left = (owner.Width - owner.Left - Width) / 2;
				Top = (owner.Height - owner.Top - Height) / 2;
				Show(owner);

				HighlightLabel labelOfActiveContent = documentsPanel.Tag as HighlightLabel;

				if (labelOfActiveContent != null)
				{
					labelOfActiveContent.Focus();
				}
			}
		}

		private void DocumentSelectorForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Control)
			{
				DisplayChosenContent();
			}
		}

		private void DisplayChosenContent()
		{
			Hide();

			HighlightLabel label = (HighlightLabel)documentsPanel.Tag;
			DockContent content = (DockContent)label.Tag;

			if (!content.IsDisposed)
			{
				content.Activate();
			}
		}

		private void DocumentSelectorForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				DisplayChosenContent();
			}
			else
			{
				int index = Labels.IndexOf((HighlightLabel)documentsPanel.Tag);

				switch (e.KeyCode)
				{
					case Keys.Tab:
						index++;

						if (index >= Labels.Count)
						{
							index = Labels.Count - ActiveDocuments.Count;
						}
						break;

					case Keys.Down:
						index++;
						break;

					case Keys.Up:
						index--;
						break;

					case Keys.Left:
					case Keys.Right:
						int panelCount = Labels.Count - ActiveDocuments.Count;

						if (index >= panelCount)
						{
							index -= panelCount;

							if (index >= panelCount)
							{
								index = panelCount - 1;
							}
						}
						else
						{
							index += panelCount;
						}
						break;
				}

				if (index >= Labels.Count)
				{
					index = 0;
				}
				else if (index < 0)
				{
					index = Labels.Count - 1;
				}

				Label nextLabel = Labels[index];
				documentsPanel.Tag = nextLabel;
				nextLabel.Focus();
			}
		}

		private void DocumentSelectorForm_Deactivate(object sender, EventArgs e)
		{
			Owner.Activate();
			Hide();
		}
	}
}