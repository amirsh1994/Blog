using Blog.DataBase.Context;
using Blog.DataBase.Entities;

namespace Blog.Core.Utils;

public static class UserRoleExtensions
{
    public static void SyncUserRole(this User user, List<int> newRoleIds,BlogContext db)
    {
        var existingRoleIds = user.UserRoles.Select(x => x.RoleId).ToList();
        //حذف نقش هایی که انتخا نشده اند
        var rolesToRemove = existingRoleIds.Except(newRoleIds).ToList();//چیزایی که تو اولی هست ولی تو دومی نیست پس باید پاک شود
        if (rolesToRemove.Any())
        {
            var removeList = user.UserRoles.Where(x => rolesToRemove.Contains(x.RoleId)).ToList();
            db.UserRoles.RemoveRange(removeList);
        }

        var rolesToAdd = newRoleIds.Except(existingRoleIds).ToList();

        if (rolesToAdd.Any())
        {
            foreach (int id in rolesToAdd)
            {
                user.UserRoles.Add(new UserRole
                {
                    CreationDate = DateTime.Now,
                    UserId = user.Id,
                    RoleId = id,
                });
            }
        }

    }
}