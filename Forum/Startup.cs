using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Forum.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Add, policy => policy.RequireRole(MyIdentityDataService.TopicAdminRoleName, MyIdentityDataService.SiteAdminRoleName, MyIdentityDataService.AuthenticatedRoleName));
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Edit, policy => policy.RequireRole(MyIdentityDataService.TopicAdminRoleName, MyIdentityDataService.SiteAdminRoleName, MyIdentityDataService.AuthenticatedRoleName));
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Delete, policy => policy.RequireRole(MyIdentityDataService.TopicAdminRoleName, MyIdentityDataService.SiteAdminRoleName));
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Blocked, policy => policy.RequireRole(MyIdentityDataService.BlockedRoleName));
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Block, policy => policy.RequireRole(MyIdentityDataService.TopicAdminRoleName, MyIdentityDataService.SiteAdminRoleName));
                options.AddPolicy(MyIdentityDataService.ForumPolicy_Comment, policy => policy.RequireRole(MyIdentityDataService.TopicAdminRoleName, MyIdentityDataService.SiteAdminRoleName, MyIdentityDataService.AuthenticatedRoleName));
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
             IHostingEnvironment env,
             UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            MyIdentityDataService.SeedData(userManager, roleManager);


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ForumPosts}/{action=Index}/{id?}");
            });
        }
    }
}
