using System;
using Microsoft.AspNetCore.Identity;
using Forum.Models;
using Forum.Data;

namespace Forum.Services
{
    public class MyIdentityDataService
    {
        public const string TopicAdminRoleName = "TopicAdmin";
        public const string SiteAdminRoleName = "SiteAdmin";
        public const string AuthenticatedRoleName = "Authenticated";
        public const string AnonymousRoleName = "Anonymous";

        public const string ForumPolicy_Add = "CanAddForumPosts";
        public const string ForumPolicy_Edit = "CanEditForumPosts";
        public const string ForumPolicy_Delete = "CanDeleteForumPosts";
        public const string ForumPolicy_Block = "CanBlockForumPosts";
        public const string ForumPolicy_Comment = "CanCommentForumPosts";

        internal static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in new[] { TopicAdminRoleName, SiteAdminRoleName, AuthenticatedRoleName, AnonymousRoleName })
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    roleManager.CreateAsync(role).GetAwaiter().GetResult();
                }
            }

            foreach (var userName in new[] { "topic_admin@snow.edu", "site_admin@snow.edu", "authenticated@snow.edu", "anonymous@snow.edu" })
            {
                var user = userManager.FindByNameAsync(userName).Result;
                if (user == null)
                {
                    user = new IdentityUser(userName);
                    user.Email = userName;
                    userManager.CreateAsync(user, "Abc123!").GetAwaiter().GetResult();
                }
                if (userName.StartsWith("topic"))
                {
                    userManager.AddToRoleAsync(user, TopicAdminRoleName).GetAwaiter().GetResult();
                }
                if (userName.StartsWith("site"))
                {
                    userManager.AddToRoleAsync(user, SiteAdminRoleName).GetAwaiter().GetResult();
                }
                if (userName.StartsWith("authenticated"))
                {
                    userManager.AddToRoleAsync(user, AuthenticatedRoleName).GetAwaiter().GetResult();
                }
                if (userName.StartsWith("anonymous"))
                {
                    userManager.AddToRoleAsync(user, AnonymousRoleName).GetAwaiter().GetResult();
                }
            }
        }
    }
}