using Microsoft.EntityFrameworkCore;
using QuickStart.Application.Common.Audit;
using QuickStart.Application.Common.Handlers;


namespace QuickStart.Application.Features.Identity;

public class Login(IHandlerContext context, IAuditContext auditContext, string key)
 : CommandHandler<LoginRequest>(context, auditContext)
{
    public async override Task<OperationResult> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
      
            var profile = await GetUserByUserNameAsync(request.UserName, cancellationToken);
            if (profile == null) return ErrorResult.Create($"Invalid username {request.UserName}");
            if (!profile.Status) return ErrorResult.Create(
                $"Status of user {profile.UserName} with id {profile.UserName} is false"
            );
            var loginDetail = await GetUserLoginDetailAsync(profile.Id, cancellationToken);
            if (loginDetail == null) return ErrorResult.Create(
                $"Invalid user login detail for profile name {profile!.ProfileName}");

            if (loginDetail!.IsLocked)
            {
                return ErrorResult.Create($"User {profile.UserName} with id {profile.UserName} is locked");
            }
            /// TODO: BCRYPT check is pending

            // if (!BCrypt.BCryptHelper.CheckPassword(request.Password, profile.Password))
            // {
            //     return ErrorResult.Create($"User {profile.UserName} with id {profile.UserName} has used wrong password");

            // }
            loginDetail.LastUpdated = DateTime.UtcNow;
            loginDetail.LoginDateTime = DateTime.UtcNow;
            return SuccessResult.Instance;

    

    }

    private async Task<UserProfile?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var query = from user in context.DBRead.UserProfiles.AsNoTracking()
                    where user.UserName == userName && user.Status == true
                    select user;
        return await query.SingleOrDefaultAsync(cancellationToken);
    }
    private async Task<UserLogin?> GetUserLoginDetailAsync(int profileId, CancellationToken cancellationToken)
    {
        var query = from ul in context.DBRead.UserLogins
                    where ul.ProfileId == profileId
                    select ul;
        return await query.SingleOrDefaultAsync(cancellationToken);
   }
    private async Task UpdateLoginHistoryAsync(int id, CancellationToken cancellationToken) {
        var query = from history in context.DBWrite.UserLoginHistories
                    where history.ProfileId == id
                    select history;
        var existing = await query.SingleOrDefaultAsync(cancellationToken);
        }
    protected override void SetAuditDetail(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}
