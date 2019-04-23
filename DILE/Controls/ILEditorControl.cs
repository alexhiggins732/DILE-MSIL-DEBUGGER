using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Disassemble;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Properties;
using Dile.UI;
using Dile.UI.Debug;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Dile.Controls
{
	partial class ILEditorControl : RichTextBox
	{
		private const int IndentationSize = 4;
		private const int EM_POSFROMCHAR = 0xd6;

		private static readonly int NativePointSize = Marshal.SizeOf(typeof(NativePoint));

		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
		public struct CHARFORMAT2
		{
			public int cbSize;
			public int dwMask;
			public int dwEffects;
			public int yHeight;
			public int yOffset;
			public int crTextColor;
			public byte bCharSet;
			public byte bPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szFaceName;
			public short wWeight;
			public short sSpacing;
			public int crBackColor;
			public int lcid;
			public int dwReserved;
			public short sStyle;
			public short wKerning;
			public byte bUnderlineType;
			public byte bAnimation;
			public byte bRevAuthor;
			public byte bReserved1;
		}

		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		private static readonly string[] opCodeNames = typeof(OpCodes).GetFields()
			.Select(field => field.Name)
			.OrderBy(fieldName => fieldName)
			.ToArray();
		private static string[] OpCodeNames
		{
			get
			{
				return opCodeNames;
			}
		}

		private bool showKeywords = false;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool ShowKeywords
		{
			get
			{
				return showKeywords;
			}

			set
			{
				showKeywords = value;
			}
		}

		private bool refreshKeyword = true;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool RefreshKeyword
		{
			get
			{
				return refreshKeyword;
			}

			set
			{
				refreshKeyword = value;
			}
		}

		private Parser parser = new Parser(string.Empty);
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public Parser Parser
		{
			get
			{
				return parser;
			}

			set
			{
				parser = value;
			}
		}

		private IMultiLine codeObject = null;
		public IMultiLine CodeObject
		{
			get
			{
				return codeObject;
			}

			set
			{
				codeObject = value;
			}
		}

		private static readonly DefaultLine defaultLine = new DefaultLine();
		private static DefaultLine DefaultLine
		{
			get
			{
				return defaultLine;
			}
		}

		private bool isRedrawDisabled = false;
		private bool IsRedrawDisabled
		{
			get
			{
				return isRedrawDisabled;
			}
			set
			{
				isRedrawDisabled = value;
			}
		}

		private BaseLineDescriptor previousCurrentLine;
		private BaseLineDescriptor PreviousCurrentLine
		{
			get
			{
				return previousCurrentLine;
			}
			set
			{
				previousCurrentLine = value;
			}
		}

		private BaseLineDescriptor currentLine = null;
		public BaseLineDescriptor CurrentLine
		{
			get
			{
				return currentLine;
			}
			set
			{
				PreviousCurrentLine = currentLine;
				currentLine = value;
			}
		}

		private List<BaseLineDescriptor> specialLines = new List<BaseLineDescriptor>();
		public List<BaseLineDescriptor> SpecialLines
		{
			get
			{
				return specialLines;
			}
		}

		private Point LastCursorPosition
		{
			get;
			set;
		}

		public ILEditorControl()
		{
			InitializeComponent();
			SelectionIndent = 70;
			keywordListBox.Parent = this;
			ReadOnly = true;
			BackColor = Color.White;

			if (!DesignMode)
			{
				keywordListBox.BeginUpdate();
				keywordListBox.Items.Clear();
				OpCodeItem[] opCodeItemsArray = OpCodeGroups.OpCodeItemsByOpCode.Values.ToArray();
				Array.Sort(opCodeItemsArray);
				keywordListBox.Items.AddRange(opCodeItemsArray);
				keywordListBox.EndUpdate();
			}
		}

		public void ShowCodeObject(IMultiLine codeObject)
		{
			CodeObject = codeObject;
			StringBuilder ilCodeString = new StringBuilder();

			for (int ilCodeIndex = 0; ilCodeIndex < CodeObject.CodeLines.Count; ilCodeIndex++)
			{
				CodeLine codeLine = CodeObject.CodeLines[ilCodeIndex];
				string text = codeLine.Text;
				for (int indentationIndex = 0; indentationIndex < codeLine.Indentation; indentationIndex++)
				{
					ilCodeString.Append("    ");
				}

				ilCodeString.AppendLine(text);
			}

			ilCodeString = ilCodeString.Replace("\0", string.Empty);
			Text = ilCodeString.ToString();
		}

		private Point GetScrollPosition()
		{
			Point result = new Point();
			IntPtr resultPointer = IntPtr.Zero;

			try
			{
				resultPointer = Marshal.AllocHGlobal(Marshal.SizeOf(result));

				SendMessage(Handle, Constants.EM_GETSCROLLPOS, IntPtr.Zero, resultPointer);
				result = (Point)Marshal.PtrToStructure(resultPointer, typeof(Point));
			}
			catch
			{
				throw;
			}
			finally
			{
				if (resultPointer != IntPtr.Zero)
				{
					Marshal.DestroyStructure(resultPointer, typeof(Point));
					Marshal.FreeHGlobal(resultPointer);
				}
			}

			return result;
		}

		private Point GetPositionFromIndex(int charIndex)
		{
			Point result = new Point();
			IntPtr nativePointPointer = IntPtr.Zero;

			try
			{
				NativePoint nativePoint = new NativePoint();
				nativePointPointer = Marshal.AllocHGlobal(NativePointSize);
				Marshal.StructureToPtr(nativePoint, nativePointPointer, false);
				IntPtr charIndexPointer = new IntPtr(charIndex);

				SendMessage(Handle, EM_POSFROMCHAR, nativePointPointer, charIndexPointer);

				Marshal.PtrToStructure(nativePointPointer, nativePoint);

				result.X = nativePoint.X;
				result.Y = nativePoint.Y;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (nativePointPointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(nativePointPointer);
				}
			}

			return result;
		}

		private void SetScrollPosition(Point position)
		{
			IntPtr positionPointer = IntPtr.Zero;

			try
			{
				positionPointer = Marshal.AllocHGlobal(Marshal.SizeOf(position));
				Marshal.StructureToPtr(position, positionPointer, false);

				SendMessage(Handle, Constants.EM_SETSCROLLPOS, IntPtr.Zero, positionPointer);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (positionPointer != IntPtr.Zero)
				{
					Marshal.DestroyStructure(positionPointer, typeof(Point));
					Marshal.FreeHGlobal(positionPointer);
				}
			}
		}

		private void SetLineColor(int position, string line, int indentationCount, BaseLineDescriptor lineDescriptor)
		{
			int indentation = indentationCount * IndentationSize;
			position += indentation;
			line = line.Substring(indentation);

			SelectionStart = position;
			SelectionLength = line.Length;
			SetColor(lineDescriptor.BackColor, lineDescriptor.ForeColor);
		}

		private void SetColor(Color backColor)
		{
			CHARFORMAT2 charFormat = new CHARFORMAT2();
			charFormat.cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
			charFormat.dwMask = Constants.CFM_BACKCOLOR;
			charFormat.crBackColor = MakeColorRef(backColor.R, backColor.G, backColor.B);

			IntPtr lparam = IntPtr.Zero;

			try
			{
				lparam = Marshal.AllocHGlobal(charFormat.cbSize);
				IntPtr wparam = new IntPtr(Constants.SCF_SELECTION);

				Marshal.StructureToPtr(charFormat, lparam, false);

				SendMessage(Handle, Constants.EM_SETCHARFORMAT, wparam, lparam);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (lparam != IntPtr.Zero)
				{
					Marshal.DestroyStructure(lparam, typeof(CHARFORMAT2));
					Marshal.FreeHGlobal(lparam);
				}
			}
		}

		private void SetColor(Color backColor, Color foreColor)
		{
			CHARFORMAT2 charFormat = new CHARFORMAT2();
			charFormat.cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
			charFormat.dwMask = Constants.CFM_BACKCOLOR | Constants.CFM_COLOR;
			charFormat.crBackColor = MakeColorRef(backColor.R, backColor.G, backColor.B);
			charFormat.crTextColor = MakeColorRef(foreColor.R, foreColor.G, foreColor.B);

			IntPtr lparam = IntPtr.Zero;

			try
			{
				lparam = Marshal.AllocHGlobal(charFormat.cbSize);
				IntPtr wparam = new IntPtr(Constants.SCF_SELECTION);

				Marshal.StructureToPtr(charFormat, lparam, false);

				SendMessage(Handle, Constants.EM_SETCHARFORMAT, wparam, lparam);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (lparam != IntPtr.Zero)
				{
					Marshal.DestroyStructure(lparam, typeof(CHARFORMAT2));
					Marshal.FreeHGlobal(lparam);
				}
			}
		}

		private static int MakeColorRef(byte r, byte g, byte b)
		{
			return (int)(((uint)r) | (((uint)g) << 8) | (((uint)b) << 16));
		}

		protected override void OnHScroll(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnHScroll(e);
			}
		}

		protected override void OnVScroll(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnVScroll(e);
			}
		}

		private void RefreshCurrentLine(bool isFormattingCleared, bool keepScrollPosition)
		{
			int position = 0;
			string line = string.Empty;
			BaseILCode ilCode = null;
			bool isScrollingNeeded = false;
			int selectionStart = SelectionStart;
			int selectionLength = SelectionLength;
			int firstVisibleCharacter = -1;
			int lastVisibleCharacter = -1;
			Point scrollPosition = new Point();

			if (CurrentLine != null)
			{
				position = FindPositionOfILCodeByOffset(CurrentLine.InstructionOffset, out line, out ilCode);
				firstVisibleCharacter = GetCharIndexFromPosition(new Point(0, 0));
				lastVisibleCharacter = GetCharIndexFromPosition(new Point(ClientSize.Width, ClientSize.Height));
				isScrollingNeeded = (position < firstVisibleCharacter || position > lastVisibleCharacter);
			}

			if (!isScrollingNeeded && keepScrollPosition)
			{
				scrollPosition = GetScrollPosition();
			}

			if (!isFormattingCleared && PreviousCurrentLine != null && CodeObject != null)
			{
				string previousLine;
				BaseILCode previousILCode;
				int previousPosition = FindPositionOfILCodeByOffset(PreviousCurrentLine.InstructionOffset, out previousLine, out previousILCode);
				int indentation = (previousILCode == null ? 0 : previousILCode.Indentation);

				SetLineColor(previousPosition, previousLine, indentation, DefaultLine);
			}

			if (CurrentLine != null && CodeObject != null)
			{
				int indentation = (ilCode == null ? 0 : ilCode.Indentation);
				SetLineColor(position, line, indentation, CurrentLine);
			}

			if (keepScrollPosition)
			{
				if (isScrollingNeeded)
				{
					SelectionLength = 0;
					ScrollToCaret();
				}
				else
				{
					SelectionLength = selectionLength;
					SelectionStart = selectionStart;
					SetScrollPosition(scrollPosition);
				}
			}
		}

		public int RefreshSpecialLines()
		{
			int result = -1;

			foreach (BaseLineDescriptor lineDescriptor in SpecialLines)
			{
				string line;
				BaseILCode ilCode;
				int position = FindPositionOfILCodeByOffset(lineDescriptor.InstructionOffset, out line, out ilCode);
				int indentation = (ilCode == null ? 0 : ilCode.Indentation);

				if (lineDescriptor.ScrollToOffset)
				{
					result = position;
				}

				SetLineColor(position, line, indentation, lineDescriptor);
			}

			return result;
		}

		public void RefreshControl(bool forceFormatRefresh)
		{
			RefreshControl(forceFormatRefresh, true);
		}

		public void RefreshControl(bool forceFormatRefresh, uint scrollToOffset)
		{
			RefreshControl(forceFormatRefresh, false);

			string line;
			BaseILCode ilCode;
			int position = FindPositionOfILCodeByOffset(Convert.ToInt32(scrollToOffset), out line, out ilCode);
			SelectionLength = 0;
			SelectionStart = position;
			ScrollToCaret();
		}

		private void RefreshControl(bool forceFormatRefresh, bool keepScrollPosition)
		{
			DisableRedraw();
			bool isFormattingCleared = false;
			MethodDefinition methodDefinition = CodeObject as MethodDefinition;
			bool displayBreakpoints = (methodDefinition != null && Project.Instance.HasBreakpointsInMethod(methodDefinition));
			bool displaySpecialLines = (SpecialLines.Count > 0);
			Point scrollPosition = new Point();
			int selectionStart = 0;
			int selectionLength = 0;
			bool restoreScrollPosition = true;

			if (keepScrollPosition && (forceFormatRefresh || displayBreakpoints || displaySpecialLines))
			{
				scrollPosition = GetScrollPosition();
				selectionStart = SelectionStart;
				selectionLength = SelectionLength;
				SelectAll();
				SetColor(DefaultLine.BackColor);
				isFormattingCleared = true;
			}

			if (displaySpecialLines)
			{
				int newSelectionStart = RefreshSpecialLines();

				if (newSelectionStart >= 0)
				{
					selectionStart = newSelectionStart;
					selectionLength = 0;
					restoreScrollPosition = false;
				}
			}

			if (displayBreakpoints)
			{
				RefreshBreakpoints(methodDefinition);
			}

			if (keepScrollPosition && (forceFormatRefresh || displayBreakpoints || displaySpecialLines))
			{
				SelectionStart = selectionStart;
				SelectionLength = selectionLength;

				if (restoreScrollPosition)
				{
					SetScrollPosition(scrollPosition);
				}
			}

			RefreshCurrentLine(isFormattingCleared, keepScrollPosition);

			EnableRedraw();
			Refresh();
		}

		private void RefreshBreakpoints(MethodDefinition methodDefinition)
		{
			foreach (FunctionBreakpointInformation functionBreakpoint in Project.Instance.FunctionBreakpoints)
			{
				if (functionBreakpoint.MethodDefinition == methodDefinition)
				{
					RefreshBreakpoint(functionBreakpoint);
				}
			}
		}

		private void RefreshBreakpoint(FunctionBreakpointInformation breakpoint)
		{
			if (CurrentLine == null || breakpoint.Offset != CurrentLine.InstructionOffset)
			{
				int breakpointOffset = Convert.ToInt32(breakpoint.Offset);
				BreakpointLine breakpointLine = new BreakpointLine(breakpoint.State, breakpointOffset);
				string line;
				BaseILCode ilCode;

				int position = FindPositionOfILCodeByOffset(breakpointOffset, out line, out ilCode);

				if (ilCode != null)
				{
					SetLineColor(position, line, ilCode.Indentation, breakpointLine);
				}
			}
		}

		public void UpdateBreakpoint(FunctionBreakpointInformation breakpoint)
		{
			DisableRedraw();
			int selectionStart = SelectionStart;
			int selectionLength = SelectionLength;

			RefreshBreakpoint(breakpoint);

			SelectionStart = selectionStart;
			SelectionLength = selectionLength;

			EnableRedraw();
			Refresh();
		}

		protected override void WndProc(ref Message m)
		{
			if (!IsRedrawDisabled || (m.Msg != 0x000F && m.Msg != 0x0014 && m.Msg != 0x0085))
			{
				base.WndProc(ref m);

				if (!DesignMode && m.Msg == 0x000F && CodeObject != null)
				{
					Point firstCharPosition = GetPositionFromCharIndex(0);

					if (firstCharPosition.X >= 0)
					{
						using (Graphics graphics = CreateGraphics())
						{
							Font regularFont = Font;
							Font boldFont = new Font(Font, FontStyle.Bold);
							Font boldItalicFont = new Font(Font, FontStyle.Bold | FontStyle.Italic);
							int charIndex = 1;
							int firstVisibleCharacter = GetCharIndexFromPosition(new Point(0, 0));
							int lastVisibleCharacter = GetCharIndexFromPosition(new Point(ClientSize.Width, ClientSize.Height));

							if (lastVisibleCharacter == 0)
							{
								lastVisibleCharacter = Text.Length;
							}

							for (int ilCodeIndex = 0; ilCodeIndex < CodeObject.CodeLines.Count && charIndex < lastVisibleCharacter; ilCodeIndex++)
							{
								CodeLine codeLine = CodeObject.CodeLines[ilCodeIndex];

								if (charIndex >= firstVisibleCharacter && charIndex <= lastVisibleCharacter && codeLine is BaseILCode)
								{
									//Point position = GetPositionFromCharIndex(charIndex);
									//Calling my own version of the method, it's much faster than the default implementation.
									Point position = GetPositionFromIndex(charIndex);

									BaseILCode ilCode = (BaseILCode)codeLine;

									Brush brush = Brushes.Black;
									Font currentFont = regularFont;

									graphics.DrawString(ilCode.Address, currentFont, brush, 0 + firstCharPosition.X - SelectionIndent, position.Y);
								}

								charIndex += codeLine.Text.Length + 1 + codeLine.Indentation * IndentationSize;
							}
						}
					}
				}
			}
		}

		protected void EnableRedraw()
		{
			Message m = new Message();
			m.HWnd = this.Handle;
			m.LParam = IntPtr.Zero;
			m.Msg = 0x000B; //wm_setredraw
			m.WParam = new IntPtr(1);
			WndProc(ref m);

			IsRedrawDisabled = false;
		}

		protected void DisableRedraw()
		{
			Message m = new Message();
			m.HWnd = this.Handle;
			m.LParam = IntPtr.Zero;
			m.Msg = 0x000B; //wm_setredraw
			m.WParam = new IntPtr(0);
			WndProc(ref m);

			IsRedrawDisabled = true;
		}

		private string GetLine(int charPosition)
		{
			string result = string.Empty;
			bool found = false;
			int tempCharIndex = 0;
			int lineIndex = 0;

			while (!found && lineIndex < Lines.Length)
			{
				string line = Lines[lineIndex++];

				if (charPosition >= tempCharIndex && charPosition <= tempCharIndex + line.Length)
				{
					found = true;
					result = line;
				}
				else
				{
					tempCharIndex += line.Length + 1;
				}
			}

			return result;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (!IsRedrawDisabled)
			{
				base.OnTextChanged(e);

				DisableRedraw();

				int selectionStart = SelectionStart;
				int selectionLength = SelectionLength;
				Font regularFont = new Font(Font, FontStyle.Regular);
				Font boldFont = new Font(Font, FontStyle.Bold);

				Parser = new Parser(Text);
				SelectAll();
				SelectionIndent = 70;
				SelectionColor = Color.Black;
				SelectionFont = regularFont;

				foreach (Comment comment in Parser.Comments)
				{
					SelectionStart = comment.StartPosition;

					if (comment.EndPosition == 0)
					{
						SelectionLength = selectionStart - comment.StartPosition;
					}
					else
					{
						SelectionLength = comment.Length;
					}

					SelectionColor = Color.Green;
				}

				if (!DesignMode)
				{
					foreach (Word word in Parser.Words)
					{
						if (Array.BinarySearch(OpCodeNames, word.WordBuilder.ToString()) >= 0 && word.IsFirstWordInLine)
						{
							SelectionStart = word.StartPosition;
							SelectionLength = word.Length;
							SelectionFont = boldFont;
						}
					}
				}

				SelectionStart = selectionStart;
				SelectionLength = selectionLength;

				EnableRedraw();
				Invalidate();
			}
		}

		private void SetKeyword(string keywordHint)
		{
			int keywordHintIndex = keywordListBox.FindString(keywordHint);

			if (keywordHintIndex != -1)
			{
				keywordListBox.SelectedIndex = keywordHintIndex;
			}
		}

		private void ShowKeywordsListBox(Point position, string keywordHint)
		{
			position.Y += 20;
			keywordListBox.Location = position;
			SetKeyword(keywordHint);
			keywordListBox.Show();
			keywordListBox.Refresh();
		}

		private void SendKeyDownMessage(IntPtr handle, Keys keys)
		{
			SendMessage(handle, Constants.WM_KEYDOWN, new IntPtr((int)keys), new IntPtr(0x20000000));
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;
			RefreshKeyword = true;

			if (msg.Msg == Constants.WM_KEYDOWN || msg.Msg == Constants.WM_SYSKEYDOWN)
			{
				ShowKeywords = ((keyData == (Keys.Control | Keys.Space)) || (keyData == (Keys.Control | Keys.J)));

				if (ShowKeywords)
				{
					result = true;
				}

				if (!ShowKeywords && keywordListBox.Visible)
				{
					switch (keyData)
					{
						case Keys.Up:
							if (keywordListBox.SelectedIndex > 0)
							{
								keywordListBox.SelectedIndex--;
							}
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.Down:
							if (keywordListBox.SelectedIndex < keywordListBox.Items.Count - 1)
							{
								keywordListBox.SelectedIndex++;
							}
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.PageUp:
							keywordListBox.SelectedIndex -= (keywordListBox.SelectedIndex < 8 ? keywordListBox.SelectedIndex : 7);
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.PageDown:
							keywordListBox.SelectedIndex += (keywordListBox.Items.Count - keywordListBox.SelectedIndex < 8 ? keywordListBox.Items.Count - keywordListBox.SelectedIndex - 1 : 7);
							result = true;
							RefreshKeyword = false;
							break;

						case Keys.Enter:
						case Keys.Tab:
							result = true;
							RefreshKeyword = false;
							break;
					}
				}

				if (keyData == (Keys.Control | Keys.Tab))
				{
					SendKeyDownMessage(Parent.Handle, keyData);
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		private int FindPositionOfILCodeByOffset(int offset)
		{
			string line;
			BaseILCode ilCode;

			return FindPositionOfILCodeByOffset(offset, out line, out ilCode);
		}

		private int FindPositionOfILCodeByOffset(int offset, out string line, out BaseILCode ilCode)
		{
			int result = 0;
			int codeLineIndex = 0;
			int lineIndex = 0;
			bool found = false;
			line = string.Empty;
			ilCode = null;

			while (!found && codeLineIndex < CodeObject.CodeLines.Count)
			{
				CodeLine codeLine = CodeObject.CodeLines[codeLineIndex];
				ilCode = codeLine as BaseILCode;

				if (ilCode != null && ilCode.Offset >= offset)
				{
					int instructionLength = 0;
					for (int lineNumber = 0; lineNumber < codeLine.TextLineNumber + 1; lineNumber++)
					{
						instructionLength += Lines[lineIndex + lineNumber].Length + 1;
					}
					line = Text.Substring(result, instructionLength);
					found = true;
				}
				else
				{
					for (int lineNumber = 0; lineNumber < codeLine.TextLineNumber + 1; lineNumber++)
					{
						result += Lines[lineIndex].Length + 1;
						lineIndex++;
					}
				}

				codeLineIndex++;
			}

			return result;
		}

		public BaseILCode GetILCodeAtMouseCursor()
		{
			int mouseCharIndex = GetCharIndexFromPosition(PointToClient(Cursor.Position));

			return FindILCodeByIndex(mouseCharIndex);
		}

		private int GetCodeLineIndexFromPosition(int position)
		{
			int result = 0;
			int lineIndex = 0;
			bool found = false;

			while (!found && result < CodeObject.CodeLines.Count)
			{
				CodeLine codeLine = CodeObject.CodeLines[result];
				position -= Lines[lineIndex].Length + 1;

				for (int lineNumber = 0; lineNumber < codeLine.TextLineNumber; lineNumber++)
				{
					lineIndex++;
					position -= Lines[lineIndex].Length + 1;
				}

				if (position < 0)
				{
					found = true;
				}
				else
				{
					lineIndex++;
					result++;
				}
			}

			if (!found)
			{
				result = -1;
			}

			return result;
		}

		private BaseILCode FindILCodeByIndex(int position)
		{
			BaseILCode result = null;

			int codeLineIndex = GetCodeLineIndexFromPosition(position);
			if (codeLineIndex > -1)
			{
				result = CodeObject.CodeLines[codeLineIndex] as BaseILCode;
			}

			return result;
		}

		public string GetSelectionWithILAddresses()
		{
			StringBuilder result = new StringBuilder();
			int selectionEnd = SelectionStart + SelectionLength;
			int codeLineIndex = 0;
			int lineIndex = 0;
			int lineStartCharIndex = 0;
			int lineEndCharIndex = 0;

			while (lineEndCharIndex < selectionEnd)
			{
				CodeLine codeLine = CodeObject.CodeLines[codeLineIndex++];

				for (int textLineIndex = 0; textLineIndex <= codeLine.TextLineNumber; textLineIndex++)
				{
					lineStartCharIndex = lineEndCharIndex;
					string line = Lines[lineIndex++];
					lineEndCharIndex += line.Length + 1;

					int copyStartCharIndex = 0;
					int copyEndCharIndex = line.Length;
					if (lineStartCharIndex < selectionEnd
						&& SelectionStart < lineEndCharIndex)
					{
						copyStartCharIndex = SelectionStart - lineStartCharIndex;
						if (copyStartCharIndex < 0)
						{
							copyStartCharIndex = 0;
							BaseILCode ilCodeLine = codeLine as BaseILCode;

							if (ilCodeLine != null)
							{
								result.Append("    ");
								result.Append(ilCodeLine.Address);
							}
						}

						if (lineStartCharIndex < selectionEnd)
						{
							copyEndCharIndex = line.Length - (lineEndCharIndex - selectionEnd - 1);

							if (copyEndCharIndex > line.Length)
							{
								copyEndCharIndex = line.Length;
							}
						}

						if (copyEndCharIndex != line.Length)
						{
							result.Append(line.Substring(copyStartCharIndex, copyEndCharIndex - copyStartCharIndex));
						}
						else if (copyStartCharIndex != 0)
						{
							result.AppendLine(line.Substring(copyStartCharIndex, copyEndCharIndex - copyStartCharIndex));
						}
						else
						{
							result.AppendLine(line);
						}
					}
				}
			}

			return result.ToString();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (Configuration.Settings.Instance.CopyILAddresses)
			{
				if ((e.KeyData == (Keys.Control | Keys.C))
					|| (e.KeyData == (Keys.Control | Keys.Insert)))
				{
					Clipboard.SetText(GetSelectionWithILAddresses());
					e.Handled = true;
				}
				else if ((e.KeyData == (Keys.Control | Keys.X))
					|| (e.KeyData == (Keys.Shift | Keys.Delete)))
				{
					Clipboard.SetText(GetSelectionWithILAddresses());
					SelectedText = string.Empty;
					e.Handled = true;
				}
			}

			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			Word hintWord = Parser.FindWordByExactPosition(SelectionStart);

			if (keywordListBox.Visible && RefreshKeyword && hintWord != null)
			{
				SetKeyword(hintWord.WordBuilder.ToString());
			}
			else if (ShowKeywords)
			{
				if (hintWord == null)
				{
					hintWord = Parser.FindWordByPosition(SelectionStart);

					if (hintWord == null || (hintWord != null && hintWord.LineNumber != GetLineFromCharIndex(SelectionStart)))
					{
						ShowKeywordsListBox(GetPositionFromCharIndex(SelectionStart), string.Empty);
					}
				}
				else if (hintWord.IsFirstWordInLine)
				{
					ShowKeywordsListBox(GetPositionFromCharIndex(hintWord.StartPosition), hintWord.WordBuilder.ToString());
				}
			}

			if (keywordListBox.Visible && e.KeyData == Keys.Escape)
			{
				keywordListBox.Visible = false;
			}

			if (!e.Alt && !e.Control && !e.Shift && (e.KeyData == Keys.Tab || e.KeyData == Keys.Space || e.KeyData == Keys.Enter) && keywordListBox.Visible)
			{
				DisableRedraw();

				if (hintWord != null)
				{
					Select(hintWord.StartPosition, hintWord.Length);
				}

				SelectedText = ((OpCodeItem)keywordListBox.SelectedItem).OpCode.Name;
				keywordListBox.Visible = false;
				EnableRedraw();
			}

			base.OnKeyUp(e);
		}

		public void SetBreakpointAtSelection()
		{
			MethodDefinition methodDefinition = CodeObject as MethodDefinition;

			if (methodDefinition != null)
			{
				BaseILCode ilCode = FindILCodeByIndex(SelectionStart);

				if (ilCode != null)
				{
					FunctionBreakpointInformation breakpointInformation = BreakpointHandler.Instance.AddRemoveBreakpoint(methodDefinition, ilCode.Offset, false);

					if (breakpointInformation != null)
					{
						UpdateBreakpoint(breakpointInformation);
						Project.Instance.IsSaved = false;
					}
				}
			}
		}

		public void SetRunToCursorAtSelection()
		{
			MethodDefinition methodDefinition = CodeObject as MethodDefinition;

			if (methodDefinition != null)
			{
				BaseILCode ilCode = FindILCodeByIndex(SelectionStart);

				if (ilCode != null)
				{
					BreakpointHandler.Instance.RunToCursor(methodDefinition, ilCode.Offset, false);
				}
			}
		}

		private void ILEditorControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (Dile.Configuration.Settings.Instance.SyncOpCodeHelper)
			{
				Point clientPoint = PointToClient(Cursor.Position);
				if (ClientRectangle.Contains(clientPoint))
				{
					BaseILCode ilCode = FindILCodeByIndex(GetCharIndexFromPosition(clientPoint));
					if (ilCode != null)
					{
						OpCodeItem opCodeItem = OpCodeGroups.OpCodeItemsByOpCode[ilCode.OpCode];
						UIHandler.Instance.DisplayOpCodeHelp(opCodeItem);
					}
				}
			}
		}
	}
}