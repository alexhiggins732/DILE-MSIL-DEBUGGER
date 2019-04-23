using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Disassemble;

namespace Dile.UI
{
	public partial class QuickSearchSettingsForm : Form
	{
		private bool updatingControls = false;
		private bool UpdatingControls
		{
			get
			{
				return updatingControls;
			}
			set
			{
				updatingControls = value;
			}
		}

		public QuickSearchSettingsForm()
		{
			InitializeComponent();
		}

		private void InitializeSearchOptionsListBox()
		{
			searchOptionsListBox.BeginUpdate();
			UpdatingControls = true;

			searchOptionsListBox.Items.Clear();

			foreach (SearchOptions enumValue in Enum.GetValues(typeof(SearchOptions)))
			{
				if (enumValue != SearchOptions.None)
				{
					bool checkedOption = ((Settings.Instance.SearchOptions & enumValue) == enumValue);
					searchOptionsListBox.Items.Add(enumValue, checkedOption);
				}
			}

			searchOptionsListBox.Sorted = true;

			UpdatingControls = false;
			searchOptionsListBox.EndUpdate();
		}

		private void QuickFinderSettingsForm_Load(object sender, EventArgs e)
		{
			InitializeSearchOptionsListBox();
		}

		private void UpdateSearchOptions()
		{
			SearchOptions searchOptions = SearchOptions.None;
			foreach (SearchOptions searchOption in searchOptionsListBox.CheckedItems)
			{
				searchOptions |= searchOption;
			}

			Settings.Instance.SearchOptions = searchOptions;
			Settings.SaveConfiguration();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			UpdateSearchOptions();
			DialogResult = DialogResult.OK;
		}
	}
}