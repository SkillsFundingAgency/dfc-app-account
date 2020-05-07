using System.Xml.Serialization;

namespace DFC.App.Account.Services.SHC.Models
{
	[XmlRoot(ElementName = "SkillsDocument", Namespace = XmlNamespaces.EntitiesNamespace)]
	public class SkillsDocument
    {

		[XmlElement(ElementName = "DocumentId", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string DocumentId { get; set; }
		[XmlElement(ElementName = "SkillsDocumentType", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string SkillsDocumentType { get; set; }
		[XmlElement(ElementName = "CreatedAt", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string CreatedAt { get; set; }
		[XmlElement(ElementName = "CreatedBy", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string CreatedBy { get; set; }
		[XmlElement(ElementName = "UpdatedAt", Namespace = XmlNamespaces.EntitiesNamespace)]
		public UpdatedAt UpdatedAt { get; set; }
		[XmlElement(ElementName = "UpdatedBy", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string UpdatedBy { get; set; }
		[XmlElement(ElementName = "DeletedAt", Namespace = XmlNamespaces.EntitiesNamespace)]
		public DeletedAt DeletedAt { get; set; }
		[XmlElement(ElementName = "DeletedBy", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string DeletedBy { get; set; }
		[XmlElement(ElementName = "ExpiresTimespan", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string ExpiresTimespan { get; set; }
		[XmlElement(ElementName = "ExpiresType", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string ExpiresType { get; set; }
		[XmlElement(ElementName = "DataValues", Namespace = XmlNamespaces.EntitiesNamespace)]
		public DataValues DataValues { get; set; }
		[XmlElement(ElementName = "Identifiers", Namespace = XmlNamespaces.EntitiesNamespace)]
		public Identifiers Identifiers { get; set; }
		[XmlElement(ElementName = "SkillsDocumentTitle", Namespace = XmlNamespaces.EntitiesNamespace)]
		public string SkillsDocumentTitle { get; set; }
	}
}
