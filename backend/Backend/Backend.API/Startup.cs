using Backend.API.Delegates;
using Backend.API.Filters;
using Backend.API.Middlewares;
using Backend.Core.App;
using Backend.Core.Options;
using Backend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text;

namespace Backend.API
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(ServiceCollectionActions.SetupAntiForgery);
            services.AddScoped<Sesion>();
            services.AddScoped<MetaData>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "Development", builder =>
                {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true);
                });
            });

            /** Options Configuration **/
            services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));
            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));

            services
                .AddDbProvider(Configuration).AddRepositories().AddServices();

            services
                .AddControllers(options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            services.AddTransient<AuthFilter>();

            services.AddScoped<MetaDataMiddleware>();
            services.AddScoped<MessagesMiddleware>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["SecurityOptions:Issuer"],
                    ValidAudience = configuration["SecurityOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityOptions:SecretKey"] ?? ""))
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            SetCultureInfo();
            app.Use(AppBuilderFunctions.AddSecurityHeaders);
            app.Use(AppBuilderFunctions.RejectForbiddenMethods);
            app.UseHsts();

            app.UseCors("Development");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<MetaDataMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SetCultureInfo()
        {
            var cultureInfo = new CultureInfo("es-EC")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = "."
                }
            };
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
