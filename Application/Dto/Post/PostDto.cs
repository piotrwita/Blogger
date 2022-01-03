namespace Application.Dto.Post
{
    public class PostDto : IMap
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }     

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Post, PostDto>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.Created));
        }
    }
}
