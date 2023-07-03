using BussinessObject.Entities;
using BussinessObject.Models;
using DataAccess.AutoMapper;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace EStoreAPI
{
    public class Startup
    {
        public IConfiguration ConfigRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            ConfigRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", build => build.AllowAnyMethod()
                    .AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(hostName => true).Build());
            });
            services.AddAutoMapper(typeof(MappingProfile));
            InjectDependencyServices(services);
            ConfigureJwt(services);
        }
        private void InjectDependencyServices(IServiceCollection services)
        {
            services.AddDbContext<EStoreDbContext>(option =>
            {
                option.UseSqlServer(ConfigRoot.GetConnectionString("Ass2Str"));
            });
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderDetailService, OrderDetailService>();
        }
        private void ConfigureJwt(IServiceCollection services)
        {
            services.Configure<JWTSetting>(ConfigRoot.GetSection("JwtSetting"));
            services.Configure<AdminAccount>(ConfigRoot.GetSection("AccountAdmin"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<EStoreDbContext>()
    .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = ConfigRoot["JwtSetting:Issuer"],
                    ValidAudience = ConfigRoot["JwtSetting:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigRoot["JwtSetting:Key"]))
                };
            });
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(build =>
            {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();
        }
    }
}
