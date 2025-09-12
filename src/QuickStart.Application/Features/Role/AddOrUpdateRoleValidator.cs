using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common;
using QuickStart.Application.Common.Handlers;

namespace QuickStart.Application.Features.Role;

public class AddOrUpdateRoleValidator : AbstractValidator<AddOrUpdateRoleRequest>
{
    private readonly IHandlerContext context;
    public AddOrUpdateRoleValidator(IHandlerContext context)
    {
        this.context = context;
        RuleFor(e => e.Code)
        .NotEmpty().WithErrorCode("code").WithMessage("Code is required")
        .Matches(ValidationPatterns.NameString).WithErrorCode("code")
        .WithMessage("Please use only letters, numbers, spaces, and these characters: . - _ ")
        .MaximumLength(10).WithErrorCode("code")
        .WithMessage("Code can not be more than 10 characters")
        .MustAsync(async (role, code, cancellationToke) =>
            await MustUniqueCodeAsync(code, role.Id, cancellationToke))
        .WithErrorCode("code")
        .WithMessage("Code is in use");

        RuleFor(e => e.Name)
       .NotEmpty().WithErrorCode("name").WithMessage("Name is required")
       .Matches(ValidationPatterns.NameString).WithErrorCode("name")
       .WithMessage("Please use only letters, numbers, spaces, and these characters: . - _ ")
       .MaximumLength(10).WithErrorCode("name")
       .WithMessage("Name can not be more than 10 characters")
       .MustAsync(async (role, code, cancellationToke) =>
           await MustUniqueCodeAsync(code, role.Id, cancellationToke))
       .WithErrorCode("name")
       .WithMessage("Name is in use");



    }
    private async Task<bool> MustUniqueCodeAsync(string code, int id, CancellationToken cancellationToken) =>
     !await context.DBRead.Roles.AnyAsync(r => r.Code == code && r.Id != id, cancellationToken);

    private async Task<bool> MustUniqueNameAsync(string name, int id, CancellationToken cancellationToken) =>
     !await context.DBRead.Roles.AnyAsync(r => r.Name == name && r.Id != id, cancellationToken);


}
