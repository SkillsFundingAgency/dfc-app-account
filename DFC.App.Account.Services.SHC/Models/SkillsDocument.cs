using System.Xml.Serialization;

namespace DFC.App.Account.Services.SHC.Models
{
	[XmlRoot(ElementName = "SkillsDocument", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
	public class SkillsDocument
    {

		[XmlElement(ElementName = "DocumentId", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string DocumentId { get; set; }
		[XmlElement(ElementName = "SkillsDocumentType", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string SkillsDocumentType { get; set; }
		[XmlElement(ElementName = "CreatedAt", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string CreatedAt { get; set; }
		[XmlElement(ElementName = "CreatedBy", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string CreatedBy { get; set; }
		[XmlElement(ElementName = "UpdatedAt", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public UpdatedAt UpdatedAt { get; set; }
		[XmlElement(ElementName = "UpdatedBy", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string UpdatedBy { get; set; }
		[XmlElement(ElementName = "DeletedAt", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public DeletedAt DeletedAt { get; set; }
		[XmlElement(ElementName = "DeletedBy", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string DeletedBy { get; set; }
		[XmlElement(ElementName = "ExpiresTimespan", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string ExpiresTimespan { get; set; }
		[XmlElement(ElementName = "ExpiresType", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string ExpiresType { get; set; }
		[XmlElement(ElementName = "DataValues", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public DataValues DataValues { get; set; }
		[XmlElement(ElementName = "Identifiers", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public Identifiers Identifiers { get; set; }
		[XmlElement(ElementName = "SkillsDocumentTitle", Namespace = "http://schemas.skillsfundingagency.bis.gov.uk/alse/skillscentral/1.0/Entities")]
		public string SkillsDocumentTitle { get; set; }
	}
}
