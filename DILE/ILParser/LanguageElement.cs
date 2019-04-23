using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public class LanguageElement
	{
		private const string EmptyString = "/* EMPTY */";

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			private set
			{
				name = value;
			}
		}

		private List<string[]> definitionElementNames = new List<string[]>();
		private List<string[]> DefinitionElementNames
		{
			get
			{
				return definitionElementNames;
			}
			set
			{
				definitionElementNames = value;
			}
		}

		private List<LanguageElement[]> definitionElements;
		public List<LanguageElement[]> DefinitionElements
		{
			get
			{
				return definitionElements;
			}
			set
			{
				definitionElements = value;
			}
		}

		private LanguageElementType type;
		public LanguageElementType Type
		{
			get
			{
				return type;
			}
			private set
			{
				type = value;
			}
		}

		public LanguageElement(LanguageElementType type, string name)
		{
			Type = type;
			Name = name;

			if (Type == LanguageElementType.Declaration)
			{
				Name = Name.Trim('\'');
			}
		}

		public void AddDefinitionElementNames(string[] elementNames)
		{
			DefinitionElementNames.Add(elementNames);
		}

		public void AssociateDefinitionElements(Dictionary<string, LanguageElement> languageElements)
		{
			if (DefinitionElements == null)
			{
				DefinitionElements = new List<LanguageElement[]>(DefinitionElementNames.Count);

				if (DefinitionElementNames.Count > 0)
				{
					foreach (string[] elementNames in DefinitionElementNames)
					{
						LanguageElement[] elements = new LanguageElement[elementNames.Length];

						for (int index = 0; index < elementNames.Length; index++)
						{
							elements[index] = languageElements[elementNames[index]];

							elements[index].AssociateDefinitionElements(languageElements);
						}

						DefinitionElements.Add(elements);
					}
				}

				DefinitionElementNames = null;
			}
		}

		public IParsedElement Parse(string text)
		{
			return Parse(new IndexedString(text));
		}

		private IParsedElement Parse(IndexedString text)
		{
			IParsedElement result = null;

			if (DefinitionElements != null && DefinitionElements.Count > 0)
			{
				int definitionElementSetIndex = 0;
				bool foundEmptyElement = false;

				while (result == null && definitionElementSetIndex < DefinitionElements.Count)
				{
					LanguageElement[] definitionElementSet = DefinitionElements[definitionElementSetIndex++];
					IParsedElement[] parsedElements = new IParsedElement[definitionElementSet.Length];
					int index = 0;
					bool succesfullyParsed = true;
					bool repeat = false;

					while (succesfullyParsed && index < definitionElementSet.Length)
					{
						LanguageElement definitionElement = definitionElementSet[index];

						if (definitionElement == this)
						{
							repeat = true;
							index++;
						}
						else
						{
							IParsedElement parsedElement = definitionElement.Parse(text);

							if (parsedElement == null)
							{
								if (repeat)
								{
									repeat = false;
								}
								else
								{
									succesfullyParsed = false;
								}
							}
							else if (parsedElement is EmptyParsedElement)
							{
								if (repeat)
								{
									repeat = false;
								}
								else
								{
									succesfullyParsed = false;
									foundEmptyElement = true;
								}
							}
							else
							{
								parsedElements[index] = parsedElement;
								index++;
							}
						}
					}

					if (succesfullyParsed)
					{
						result = new ParsedElement<IParsedElement[]>(this, parsedElements);
					}
				}

				if (result == null && foundEmptyElement)
				{
					result = new ParsedElement<IParsedElement[]>(this, null);
				}
			}
			else
			{
				switch(Type)
				{
					case LanguageElementType.Declaration:
						if (text.ReadExpression(Name))
						{
							result = new ParsedElement<string>(this, Name);
						}
						break;

					case LanguageElementType.Empty:
						result = new EmptyParsedElement();
						break;

					case LanguageElementType.Float64:
						break;

					case LanguageElementType.ID:
						result = new ParsedElement<string>(this, text.ReadID());
						break;

					case LanguageElementType.ILInstruction:
						break;

					case LanguageElementType.Int32:
						break;

					case LanguageElementType.Int64:
						break;

					case LanguageElementType.QString:
						break;

					case LanguageElementType.SQString:
						break;

					case LanguageElementType.Start:
						break;

					default:
						throw new NotImplementedException();
				}
			}

			return result;
		}
	}
}