using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Xsl;

namespace Dile.Controls
{
	public partial class RssBrowser : UserControl
	{
		public event FeedUpdatedEventHandler FeedUpdated;
		public event FileDragDropEventHandler FileDragDrop;

		public string RssUrl
		{
			get;
			set;
		}

		public string RssTransformer
		{
			get;
			set;
		}

		public RssBrowser()
		{
			InitializeComponent();
		}

		public void DisplayDefaultFeed(string html)
		{
			try
			{
				if (!string.IsNullOrEmpty(html))
				{
					webBrowser.DocumentText = html;
				}
			}
			finally
			{
			}
		}

		public void UpdateRssAsync()
		{
			ThreadPool.QueueUserWorkItem(state => UpdateRss());
		}

		private object UpdateRss()
		{
			try
			{
				string rssFeed = string.Empty;
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RssUrl);

				using (WebResponse webResponse = request.GetResponse())
				{
					using (StreamReader rssStreamReader = new StreamReader(webResponse.GetResponseStream()))
					{
						rssFeed = rssStreamReader.ReadToEnd();
					}
				}

				string updatedFeedHtml = Transform(rssFeed);

				OnFeedUpdated(updatedFeedHtml);

				if (!webBrowser.IsDisposed)
				{
					if (webBrowser.InvokeRequired)
					{
						InvokeHelper.InvokeFormMethod(webBrowser, () => webBrowser.DocumentText = updatedFeedHtml);
					}
					else
					{
						webBrowser.DocumentText = updatedFeedHtml;
					}
				}
			}
			catch
			{
				//Ignore errors. This is not such an important feature.
			}
			finally
			{
			}

			return null;
		}

		private string Transform(string rssFeed)
		{
			string result = string.Empty;
			XslCompiledTransform transform = new XslCompiledTransform();

			using (StringReader transformerReader = new StringReader(RssTransformer))
			{
				using (XmlTextReader transformerXmlReader = new XmlTextReader(transformerReader))
				{
					transform.Load(transformerXmlReader);
				}
			}

			using (StringReader rssFeedReader = new StringReader(rssFeed))
			{
				using (XmlTextReader rssFeedXmlReader = new XmlTextReader(rssFeedReader))
				{
					using (MemoryStream htmlStream = new MemoryStream())
					{
						using (XmlTextWriter htmlXmlWriter = new XmlTextWriter(htmlStream, Encoding.UTF8))
						{
							transform.Transform(rssFeedXmlReader, htmlXmlWriter);

							htmlStream.Seek(0, SeekOrigin.Begin);

							using (StreamReader htmlReader = new StreamReader(htmlStream))
							{
								result = htmlReader.ReadToEnd();
							}
						}
					}
				}
			}

			return result;
		}

		protected virtual void OnFeedUpdated(string updatedFeedHtml)
		{
			if (FeedUpdated != null)
			{
				FeedUpdated(this, new FeedUpdatedEventArgs(updatedFeedHtml));
			}
		}

		protected virtual void OnFileDragDrop(Uri draggedObjectUri)
		{
			if (FileDragDrop != null)
			{
				FileDragDrop(this, new FileDragDropEventArgs(draggedObjectUri));
			}
		}

		private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			Rectangle bodyRectangle = webBrowser.Document.Body.ScrollRectangle;

			Height = bodyRectangle.Height;
		}

		private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (string.Equals(e.Url.Scheme, "file", StringComparison.OrdinalIgnoreCase))
			{
				OnFileDragDrop(e.Url);
				e.Cancel = true;
			}
		}
	}
}