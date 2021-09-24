namespace GVUZ.Model
{
	public interface IAttachmentFactory
	{
		IAttachment AddAttachment();
	}

	public interface IAttachment
	{
		int AttachmentID { get; set; }

		string MimeType { get; set; }

		string Name { get; set; }

		byte[] Body { get; set; }
	}
}