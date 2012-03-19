﻿using System.Xml;
using uComponents.Core.XsltExtensions;
using umbraco.cms.businesslogic.datatype;

namespace uComponents.Core.Shared.Data
{
	/// <summary>
	/// Overrides the <see cref="umbraco.cms.businesslogic.datatype.DefaultData"/> object to return the value as XML.
	/// </summary>
	public class CsvToXmlData : DefaultData
	{
		/// <summary>
		/// The separators to split the delimited string.
		/// </summary>
		private string[] separator;

		/// <summary>
		/// Name for the root node.
		/// </summary>
		private string rootName;

		/// <summary>
		/// Name for the element/node that contains the value.
		/// </summary>
		private string elementName;

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvToXmlData"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		public CsvToXmlData(BaseDataType dataType)
			: this(dataType, "values")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvToXmlData"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="rootName">Name of the root.</param>
		public CsvToXmlData(BaseDataType dataType, string rootName)
			: this(dataType, rootName, "value")
		{
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="CsvToXmlData"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="rootName">Name of the root.</param>
		/// <param name="elementName">Name of the element.</param>
		public CsvToXmlData(BaseDataType dataType, string rootName, string elementName)
			: this(dataType, rootName, elementName, new[] { Constants.Common.COMMA.ToString() })
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvToXmlData"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="rootName">Name of the root.</param>
		/// <param name="elementName">Name of the element.</param>
		/// <param name="separator">The separator.</param>
		public CsvToXmlData(BaseDataType dataType, string rootName, string elementName, string[] separator)
			: base(dataType)
		{
			this.rootName = rootName;
			this.elementName = elementName;
			this.separator = separator;
		}

		/// <summary>
		/// Converts the data value to XML.
		/// </summary>
		/// <param name="data">The data to convert to XML.</param>
		/// <returns>Returns the data value as an XmlNode</returns>
		public override XmlNode ToXMl(XmlDocument data)
		{
			// check that the value isn't null
			if (this.Value != null)
			{
				// split the CSV data into an XML document.
				var xml = Xml.Split(new XmlDocument(), this.Value.ToString(), this.separator, this.rootName, this.elementName);

				// return the XML node.
				return data.ImportNode(xml.DocumentElement, true);
			}
			else
			{
				// otherwise render the value as default (in CDATA)
				return base.ToXMl(data);
			}
		}
	}
}