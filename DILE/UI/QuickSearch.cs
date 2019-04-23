using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Disassemble;
using System.Threading;
using System.Windows.Forms;

namespace Dile.UI
{
	public class QuickSearch
	{
		private FoundItem foundItemCallback;
		private FoundItem FoundItemCallback
		{
			get
			{
				return foundItemCallback;
			}
			set
			{
				foundItemCallback = value;
			}
		}

		private Form callbackForm;
		private Form CallbackForm
		{
			get
			{
				return callbackForm;
			}
			set
			{
				callbackForm = value;
			}
		}

		public QuickSearch(Form callbackForm, FoundItem foundItemCallback)
		{
			FoundItemCallback = foundItemCallback;
			CallbackForm = callbackForm;
		}

		public void StartSearch(string searchText)
		{
			if (CallbackForm != null && FoundItemCallback != null)
			{
				Thread searchThread = new Thread(new ParameterizedThreadStart(Search));
				searchThread.Name = "Quick Finder thread";
				searchThread.Priority = ThreadPriority.Lowest;
				searchThread.Start(searchText);
			}
		}

		private void Search(object searchTextObject)
		{
			bool cancel = false;
			string searchText = (string)searchTextObject;
			string upperCaseSearchText = searchText.ToUpperInvariant();

			if (Project.Instance.Assemblies != null)
			{
				List<Assembly>.Enumerator assemblyEnumerator = Project.Instance.Assemblies.GetEnumerator();

				while (!cancel && assemblyEnumerator.MoveNext())
				{
					Dictionary<uint, TokenBase>.ValueCollection.Enumerator tokenEnumerator = assemblyEnumerator.Current.AllTokens.Values.GetEnumerator();

					if (((Settings.Instance.SearchOptions & SearchOptions.Assembly) == SearchOptions.Assembly
						&& assemblyEnumerator.Current.Name != null && assemblyEnumerator.Current.Name.Contains(searchText))
						|| (Settings.Instance.SearchOptions & SearchOptions.TokenValues) == SearchOptions.TokenValues
						&& assemblyEnumerator.Current.Token.ToString("X").Contains(upperCaseSearchText))
					{
						FoundItemEventArgs eventArgs = OnFoundItem(assemblyEnumerator.Current, assemblyEnumerator.Current);

						cancel = eventArgs.Cancel;
					}

					while (!cancel && tokenEnumerator.MoveNext())
					{
						TokenBase tokenObject = tokenEnumerator.Current;

						if ((tokenObject.ItemType != SearchOptions.None
							&& (Settings.Instance.SearchOptions & tokenObject.ItemType) == tokenObject.ItemType
							&& ((searchText.Length == 0 ||
							(tokenObject.Name != null && tokenObject.Name.Contains(searchText)))))
							|| ((Settings.Instance.SearchOptions & SearchOptions.TokenValues) == SearchOptions.TokenValues
							&& tokenObject.Token.ToString("X").PadLeft(8, '0').Contains(upperCaseSearchText)
							&& tokenObject.ItemType != SearchOptions.None))
						{
							FoundItemEventArgs eventArgs = OnFoundItem(assemblyEnumerator.Current, tokenObject);

							cancel = eventArgs.Cancel;
						}
					}
				}
			}
		}

		private FoundItemEventArgs OnFoundItem(Assembly assembly, TokenBase tokenObject)
		{
			FoundItemEventArgs eventArgs = new FoundItemEventArgs(assembly, tokenObject);
			IAsyncResult asyncResult = CallbackForm.BeginInvoke(FoundItemCallback, this, eventArgs);
			asyncResult.AsyncWaitHandle.WaitOne();

			return eventArgs;
		}
	}
}