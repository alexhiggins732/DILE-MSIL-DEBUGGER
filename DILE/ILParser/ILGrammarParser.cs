using System;
using System.Collections.Generic;
using System.Text;

using Dile.Properties;
using System.IO;

namespace Dile.ILParser
{
	public class ILGrammarParser
	{
		private const string StartElementName = "START";
		private const string EmptyElementName = "/* EMPTY */";

		private LanguageElement startElement;
		private LanguageElement StartElement
		{
			get
			{
				return startElement;
			}
			set
			{
				startElement = value;
			}
		}

		public void ParseGrammar()
		{
			Dictionary<string, LanguageElement> languageElements = new Dictionary<string, LanguageElement>();

			LanguageElement emptyElement = new LanguageElement(LanguageElementType.Empty, EmptyElementName);
			languageElements[EmptyElementName] = emptyElement;

			using (MemoryStream asmparseStream = new MemoryStream(Resources.asmparse))
			{
				using (StreamReader reader = new StreamReader(asmparseStream))
				{
					bool startLineFound = false;
					LanguageElement currentElement = null;

					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();

						if (!startLineFound && line.Contains(StartElementName))
						{
							startLineFound = true;
						}

						if (startLineFound)
						{
							string elementDefinition = string.Empty;

							if (currentElement == null)
							{
								int colonIndex = line.IndexOf(':');

								if (colonIndex > -1)
								{
									string elementName = line.Substring(0, colonIndex).Trim();

									if (languageElements.ContainsKey(elementName))
									{
										currentElement = languageElements[elementName];
									}
									else
									{
										currentElement = CreateLanguageElement(elementName);
										languageElements.Add(elementName, currentElement);
									}

									if (StartElement == null)
									{
										StartElement = currentElement;
									}

									elementDefinition = line.Substring(colonIndex + 1);
								}
							}
							else
							{
								int pipeIndex = line.IndexOf('|');

								if (pipeIndex > -1)
								{
									elementDefinition = line.Substring(pipeIndex + 1);
								}
								else
								{
									currentElement = null;
								}
							}

							if (currentElement != null && !string.IsNullOrEmpty(elementDefinition))
							{
								elementDefinition = elementDefinition.Trim();

								if (elementDefinition == EmptyElementName)
								{
									currentElement.AddDefinitionElementNames(new string[] { EmptyElementName });
								}
								else
								{
									string[] definitionParts = elementDefinition.Split(' ');

									foreach (string definitionPart in definitionParts)
									{
										if (!languageElements.ContainsKey(definitionPart))
										{
											languageElements.Add(definitionPart, CreateLanguageElement(definitionPart));
										}
									}

									currentElement.AddDefinitionElementNames(definitionParts);
								}
							}
						}
					}
				}
			}

			StartElement.AssociateDefinitionElements(languageElements);
		}

		private LanguageElement CreateLanguageElement(string name)
		{
			LanguageElement result = null;
			LanguageElementType type = LanguageElementType.Declaration;

			switch(name.ToUpperInvariant())
			{
				case "ID":
					type = LanguageElementType.ID;
					break;

				case "QSTRING":
					type = LanguageElementType.QString;
					break;

				case "SQSTRING":
					type = LanguageElementType.SQString;
					break;

				case "INT32":
					type = LanguageElementType.Int32;
					break;

				case "INT64":
					type = LanguageElementType.Int64;
					break;

				case "FLOAT64":
					type = LanguageElementType.Float64;
					break;

				case "INSTR_*":
					type = LanguageElementType.ILInstruction;
					break;

				case "/* EMPTY */":
					type = LanguageElementType.Empty;
					break;

				case "START":
					type = LanguageElementType.Start;
					break;
			}

			result = new LanguageElement(type, name);

			return result;
		}

		public ILParser CreateParser()
		{
			return new ILParser(StartElement);
		}
	}
}