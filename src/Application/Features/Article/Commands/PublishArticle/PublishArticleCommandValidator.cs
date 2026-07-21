using FluentValidation;

namespace AuthService.Application.Features.Article.Commands.PublishArticle
{
    public class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
    {
        public PublishArticleCommandValidator()
        {
            RuleFor(x => x.ArticleId)
                .NotEmpty()
                .WithMessage("ArticleId can't null");
        }
    }
}
