using Application.Dto.Posts;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Services
{
    public class PostServiceTests
    {
        [Fact]
        public async Task add_post_async_should_invoke_add_async_on_post_repositotory()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            var postService = new PostService(postRepositoryMock.Object,
                                              mapperMock.Object,
                                              loggerMock.Object);

            var postDto = new CreatePostDto()
            {
                Title = "Title 1",
                Content = "Content 1"
            };

            //powiazanie miedzy typem post a createpostdto
            //jezeli gdziekolwiek w aplikacji wystapi takie mapowanie to wlasnie wykona nam sie przygotowany setup ktory zwroci odpowiednie wartosci
            mapperMock.Setup(x => x.Map<Post>(postDto))
                .Returns(new Post() 
                        { 
                            Title = postDto.Title, 
                            Content = postDto.Content 
                        });

            var userId = "f44256b4-abef-4ed7-82bf-7a9bda7c4d96";

            //Act
            await postService.AddNewPostAsync(postDto, userId);

            //Assert

            //Argumenty to akcja i ilosc zwroconych rezultatow
            //akcja - It.IsAny<Post> - sprawdza czy do repozytorium dodany zostal jakikolwiek obiekt post
            //ilosc - Times.Once - dodajemy tylko 1 post
            postRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task update_post_async_should_invoke_update_async_on_post_repositotory()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            var postService = new PostService(postRepositoryMock.Object,
                                              mapperMock.Object,
                                              loggerMock.Object);

            var id = 1;
            var title = "Title 1";
            var content = "Content 1";
            var post = new Post(id, title, content);

            var updatePostDto = new UpdatePostDto()
            {
                Id = post.Id,
                Content = "Content 1 update"
            };

            mapperMock
                .Setup(x => x.Map<Post>(updatePostDto))
                .Returns(post);

            postRepositoryMock
                .Setup(x => x.UpdateAsync(post)).Verifiable();

            //Act
            await postService.UpdatePostAsync(updatePostDto);

            //Assert
            postRepositoryMock.Verify(x => x.GetByIdAsync(post.Id), Times.Once);
            updatePostDto.Should().NotBeNull();
            updatePostDto.Content.Should().NotBeNull();
            updatePostDto.Content.Should().NotBeSameAs(post.Content);
        }

        [Fact]
        public async Task delete_post_async_should_invoke_delete_async_on_post_repositotory()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            var postService = new PostService(postRepositoryMock.Object,
                                              mapperMock.Object,
                                              loggerMock.Object);

            var id = 1;
            var title = "Title 1";
            var content = "Content 1";
            var post = new Post(id, title, content);

            postRepositoryMock
                .Setup(x => x.DeleteAsync(post)).Verifiable();

            //Act
            await postService.DeletePostAsync(post.Id);

            //Assert
            postRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task when_invoking_get_post_async_it_should_invoke_get_async_on_post_repositotory()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            var postService = new PostService(postRepositoryMock.Object,
                                              mapperMock.Object,
                                              loggerMock.Object);

            var id = 1;
            var title = "Title 1";
            var content = "Content 1";
            var post = new Post(id, title, content);

            var postDto = new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };

            mapperMock
                .Setup(x => x.Map<Post>(postDto))
                .Returns(post);

            postRepositoryMock
                .Setup(x => x.GetByIdAsync(post.Id))
                .ReturnsAsync(post);

            //Act
            var existingPostDto = await postService.GetPostByIdAsync(post.Id);

            //Assert
            postRepositoryMock.Verify(x => x.GetByIdAsync(post.Id), Times.Once);
            postDto.Should().NotBeNull();
            postDto.Title.Should().NotBeNull();
            postDto.Title.Should().BeEquivalentTo(post.Title);
            postDto.Content.Should().NotBeNull();
            postDto.Content.Should().BeEquivalentTo(post.Content);
        }
    }
}
