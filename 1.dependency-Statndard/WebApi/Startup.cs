using Common.Shared;
using Core;
using Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Web.Http;

namespace SwaggerID
{
    public class Startup
    {
        private const string AllowAllCors = "AllowAllCors";
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCors,
                builder =>
                {
                    builder.WithOrigins("https://*")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
                });
            });
            #region Autentication
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                string jwtKey = string.Empty;
                string jwtIssuer = string.Empty;
                jwtKey = Configuration[$"{Enviroment.SETTINGS}:JwtSettings:Key"];
                jwtIssuer = Configuration[$"{Enviroment.SETTINGS}:JwtSettings:Issuer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            #endregion
            IConfiguration configuration = Configuration;
            Settings options;
            options = configuration.GetSection("Settings").Get<Settings>();
            services.AddSingleton(options);
            // Other configurations
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<IWeatherForecast, WeatherForecast>();
            services.AddSingleton<ICustomer, CustomerService>();

            #region cors
            //services.AddCors();

            services.AddControllers();
            #endregion

            #region CahceSection
            services.AddMemoryCache();
            services.AddResponseCaching();
            #endregion

            #region MvcSection
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.Filters.Add(new ErrorHandlingFilter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            #endregion MvcSection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // For the wwwroot folder
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Templates")),
            //    RequestPath = "/Templates"
            //});
            // Use the CORS policy
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(AllowAllCors);
            app.UseAuthentication();
            app.UseAuthorization();

            // Also add RequireCors to the controllers:
            app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireCors(AllowAllCors); });

            //app.UseCors();
            #region MvcSection

            app.UseStatusCodePages();
            app.UseCors();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}",
                    defaults: new { id = RouteParameter.Optional });
            });

            #endregion MvcSection
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
            });
        }
    }
}
