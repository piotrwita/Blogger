using Blogger.Contracts.Requests.Blogger;
using Blogger.Contracts.Requests.Identity;
using Blogger.Sdk;
using Refit;

var client = "https://localhost:44322/";

var cachedToken = string.Empty;
var settings = new RefitSettings
{
    AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
};

var identityApi = RestService.For<IIdentityApi>(client);
var bloggerApi = RestService.For<IBloggerApi>(client, settings);

//var registerModel = new RegisterModel()
//{
//    UserName = "sdkaccout111",
//    Email = "pit_allen1@o2.pl",
//    Password = "Psdssd@@@222ooo"
//};

//var register = await identityApi.RegisterAsync(registerModel);

var loginModel = new LoginModel()
{
    UserName = "sdkaccout111",
    Password = "Psdssd@@@222ooo"
};

var login = await identityApi.LoginAsync(loginModel);

cachedToken = login.Content.Token;

var newPost = new CreatePostDto
{
    Title = "post sdk",
    Content = "tresc sdk"
};

var createdPost = await bloggerApi.CreatePostAsync(newPost);

var postId = createdPost.Content.Data.Id;

var retrievedPost = await bloggerApi.GetPostAsync(postId);

var updatePost = new UpdatePostDto
{
    Id = postId,
    Content = "tresc nowa sdk"
};

await bloggerApi.UpdatePostAsync(updatePost);

await bloggerApi.DeletePostAsync(postId);
