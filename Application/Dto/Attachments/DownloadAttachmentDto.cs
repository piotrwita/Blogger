namespace Application.Dto.Attachments
{
    /// <summary>
    /// ta klasa służy nam do transportu do warstwy WebApi przekonwertowanego pliku na strumień bajtów w przypadku pobierania pliku
    /// </summary>
    public class DownloadAttachmentDto : AttachmentDto
    {
        public byte[] Content { get; set; }
    }
}
