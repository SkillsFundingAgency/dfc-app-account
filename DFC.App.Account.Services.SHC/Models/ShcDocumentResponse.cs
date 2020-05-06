using System.Collections.Generic;
using System.Xml.Serialization;

namespace DFC.App.Account.Services.SHC.Models
{
	[XmlRoot(ElementName = "UpdatedAt", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class UpdatedAt
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "DeletedAt", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class DeletedAt
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "DataValues", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class DataValues
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "SkillsDocumentIdentifier", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class SkillsDocumentIdentifierResponse
	{
		[XmlElement(ElementName = "ServiceName", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string ServiceName { get; set; }
		[XmlElement(ElementName = "Value", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "Identifiers", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class Identifiers
	{
		[XmlElement(ElementName = "SkillsDocumentIdentifier", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public List<SkillsDocumentIdentifierResponse> SkillsDocumentIdentifier { get; set; }
	}


	[XmlRoot(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResult", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0")]
	public class FindDocumentsByServiceNameKeyValueAndDocTypeResult
	{
		[XmlElement(ElementName = "SkillsDocument", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public List<SkillsDocument> SkillsDocument { get; set; }
		[XmlAttribute(AttributeName = "a", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string A { get; set; }
		[XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string I { get; set; }
	}

	[XmlRoot(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResponse", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0")]
	public class FindDocumentsByServiceNameKeyValueAndDocTypeResponse
	{
		[XmlElement(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResult", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0")]
		public FindDocumentsByServiceNameKeyValueAndDocTypeResult FindDocumentsByServiceNameKeyValueAndDocTypeResult { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public class Body
	{
		[XmlElement(ElementName = "FindDocumentsByServiceNameKeyValueAndDocTypeResponse", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0")]
		public FindDocumentsByServiceNameKeyValueAndDocTypeResponse FindDocumentsByServiceNameKeyValueAndDocTypeResponse { get; set; }
	}

	[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public class ShcDocumentResponse
	{
		[XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public Body Body { get; set; }
		[XmlAttribute(AttributeName = "s", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string S { get; set; }
	}

}
