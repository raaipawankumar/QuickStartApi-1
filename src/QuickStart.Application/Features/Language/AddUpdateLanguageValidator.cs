using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Language;

public class AddUpdateLanguageValidator : AbstractValidator<AddUpdateLanguageRequest>
{
    private readonly IHandlerContext context;
    public AddUpdateLanguageValidator(IHandlerContext context)
    {
        this.context = context;

        RuleFor(e => e.Code).NotEmpty()
        .WithErrorCode("code")
        .WithMessage("Language code is required")
        .MinimumLength(2).WithErrorCode("code").WithMessage("Language code minimum length is 2 character")
        .MaximumLength(5).WithErrorCode("code").WithMessage("Language code maximum length is 5 character")
        .Matches(ValidationPatterns.NameString).WithErrorCode("code")
        .WithMessage(@"Please use only letters, numbers, spaces, and these characters: . - _ ")
        .MustAsync(async (language, code, cancellationToken)
         => await MustUniqueCode(code ?? string.Empty, language.Id, cancellationToken))
        .WithErrorCode("code")
        .WithMessage("Language code is in use");

        RuleFor(e => e.Name).NotEmpty()
       .WithErrorCode("name")
       .WithMessage("Language name is required")
       .MustAsync(async (language, name, cancellationToken)
        => await MustUniqueName(name ?? string.Empty, language.Id, cancellationToken))
       .WithErrorCode("name")
       .WithMessage("Language name is in use");

        RuleFor(e => e.Description).Matches(ValidationPatterns.DetailString)
        .WithErrorCode("description")
        .WithMessage(@"Please use only letters, numbers, spaces, and these characters: . , - _ @ # & + / ( )");

    }
    private async Task<bool> MustUniqueCode(string code, Guid id, CancellationToken cancellationToken) =>
      !await context.DBRead.Languages.AnyAsync(language => language.Code == code
       && language.Id != id,
       cancellationToken);


 private async Task<bool> MustUniqueName(string name, Guid id, CancellationToken cancellationToken) =>
    !await context.DBRead.Languages.AnyAsync(language => language.Name == name
        && language.Id != id,
        cancellationToken);
}
