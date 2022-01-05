namespace Application.Dto.Attachments
{
    /// <summary>
    /// ta klasa służy nam do transportu do warstwy WebApi przekonwertowanego pliku na strumień bajtów w przypadku pobierania pliku
    /// w przypadku pobierania tylko info o załącznikach dodanych do danego posta nie chcemy zwracac samego zalacznika, czyli tablicy bajtow, a jedynie informację o id i nazwie
    /// </summary>
    public class AttachmentDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Attachment, AttachmentDto>();
        }
    }
}
