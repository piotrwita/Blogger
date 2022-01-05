using Application.Dto.Posts;
using FluentValidation;

namespace Application.Validators
{
    public class CreatePostDtoValidatior : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidatior()
        {
            #region Title
            RuleFor(x => x.Title).NotEmpty().WithMessage("Post can not have an empty title");
            RuleFor(x => x.Title).Length(5,100).WithMessage("The title must be between 5 and 100 characters long");
            #endregion
        }
    }
} 