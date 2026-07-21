using FluentValidation;

namespace AuthService.Application.Features.Article.Commands.CreateArticle
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.")
                .MinimumLength(3)
                .WithMessage("Title must be at least 3 characters.");

            RuleFor(x => x.AuthorEmail)
                .NotEmpty()
                .WithMessage("Author email is required.")
                .EmailAddress()
                .WithMessage("Author email must be a valid email address.")
                .MaximumLength(128)
                .WithMessage("Author email must not exceed 128 characters.");
        }
    }
}
