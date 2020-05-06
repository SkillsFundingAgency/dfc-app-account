using System.Collections.Generic;
using System.Xml.Serialization;

namespace DFC.App.Account.Services.SHC.Models
{

	public static class XmlNamespaces
    {
        public const string EntitiesNamespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities";
        public const string SchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        public const string SkillsCentralNamespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0";
        public const string W3XmlNamespace = "http://www.w3.org/2000/xmlns/";
        public const string XmlSoapNamespace = "http://schemas.xmlsoap.org/soap/envelope/";
    }
	

	[XmlRoot(ElementName = "UpdatedAt", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class UpdatedAt
	{
		[XmlAttribute(AttributeName = "nil", Namespace = XmlNamespaces.SchemaInstanceNamespace)]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "DeletedAt", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class DeletedAt
	{
		[XmlAttribute(AttributeName = "nil", Namespace = XmlNamespaces.SchemaInstanceNamespace)]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "DataValues", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class DataValues
	{
		[XmlAttribute(AttributeName = "nil", Namespace = XmlNamespaces.SchemaInstanceNamespace)]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "SkillsDocumentIdentifier", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class SkillsDocumentIdentifierResponse
	{
		[XmlElement(ElementName = "ServiceName", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string ServiceName { get; set; }
		[XmlElement(ElementName = "Value", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "Identifiers", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class Identifiers
	{
		[XmlElement(ElementName = "SkillsDocumentIdentifier", Namespace = XmlNamespaces.EntitiesNamespace)]
		public List<SkillsDocumentIdentifierResponse> SkillsDocumentIdentifier { get; set; }
	}


	[XmlRoot(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResult", Namespace = XmlNamespaces.SkillsCentralNamespace)]
	public class FindDocumentsByServiceNameKeyValueAndDocTypeResult
	{
		[XmlElement(ElementName = "SkillsDocument", Namespace = XmlNamespaces.EntitiesNamespace)]
		public List<SkillsDocument> SkillsDocument { get; set; }
		[XmlAttribute(AttributeName = "a", Namespace = XmlNamespaces.W3XmlNamespace)]
		public string A { get; set; }
		[XmlAttribute(AttributeName = "i", Namespace = XmlNamespaces.W3XmlNamespace)]
		public string I { get; set; }
	}

	[XmlRoot(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResponse", Namespace = XmlNamespaces.SkillsCentralNamespace)]
	public class FindDocumentsByServiceNameKeyValueAndDocTypeResponse
	{
		[XmlElement(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResult", Namespace = XmlNamespaces.SkillsCentralNamespace)]
		public FindDocumentsByServiceNameKeyValueAndDocTypeResult FindDocumentsByServiceNameKeyValueAndDocTypeResult { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Body", Namespace = XmlNamespaces.XmlSoapNamespace)]
	public class Body
	{
		[XmlElement(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResponse", Namespace = XmlNamespaces.SkillsCentralNamespace)]
		public FindDocumentsByServiceNameKeyValueAndDocTypeResponse FindDocumentsByServiceNameKeyValueAndDocTypeResponse { get; set; }
	}

	[XmlRoot(ElementName = "Envelope", Namespace = XmlNamespaces.XmlSoapNamespace)]
	public class ShcDocumentResponse
	{
		[XmlElement(ElementName = "Body", Namespace = XmlNamespaces.XmlSoapNamespace)]
		public Body Body { get; set; }
		[XmlAttribute(AttributeName = "s", Namespace = XmlNamespaces.W3XmlNamespace)]
		public string S { get; set; }
	}

}
