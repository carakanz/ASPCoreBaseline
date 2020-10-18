using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
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
            // Read db settings
            var databaseSettings = Configuration.GetSection(nameof(Models.DatabaseOptions));
            services.Configure<Models.DatabaseOptions>(databaseSettings);

            services.AddSingleton<Models.IDatabaseOptions>(sp =>
                sp.GetRequiredService<IOptions<Models.DatabaseOptions>>().Value);

            // Add MongoDB GridFS
            services.AddSingleton<Services.DocumentService>();

            // Add MySql
            services.AddDbContext<Models.ApplicationContext>(options =>
               options.UseMySql(databaseSettings[nameof(Models.DatabaseOptions.MySqlConnection)]));

            // Set DateFormatString
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "dd.MM.yyyy";
            });

            // Read IdentitySettings
            var identityOptions = Configuration.GetSection(nameof(IdentityOptions))
                .Get<IdentityOptions>();
            // Add Identity
            services.AddIdentity<Models.User, IdentityRole>(options =>
            {
                options.Password = identityOptions.Password;
                options.Lockout = identityOptions.Lockout;
                options.User = identityOptions.User;
            }).AddEntityFrameworkStores<Models.ApplicationContext>();
            services.AddScoped<RoleManager<IdentityRole>>();

            // Read AuthorizationSettings
            var authorizationSection = Configuration.GetSection(nameof(Models.AuthorizationOptions));
            services.Configure<Models.AuthorizationOptions>(authorizationSection);
            var authorizationSettings = authorizationSection.Get<Models.AuthorizationOptions>();

            // Add JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = authorizationSettings.ValidateIssuer,
                    // строка, представляющая издателя
                    ValidIssuer = authorizationSettings.ValidIssuer,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = authorizationSettings.ValidateAudience,
                    // установка потребителя токена
                    ValidAudience = authorizationSettings.ValidAudience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = authorizationSettings.ValidateLifetime,

                    // установка ключа безопасности
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(
                            authorizationSettings.Key + "__IssuerSigningKeyCode__")),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = authorizationSettings.ValidateIssuerSigningKey,
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
