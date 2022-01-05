﻿namespace Application.Dto.Posts
{
    public class UpdatePostDto : IMap
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePostDto, Post>();
        }
    }
}
