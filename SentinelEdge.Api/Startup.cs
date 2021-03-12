using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using SentinelEdge.Api.Services;
using System.Reflection;
using System.IO;
using SentinelEdge.Api.HealthChecks;
using SentinelEdge.Api.Unifi;
using Microsoft.AspNetCore.Authorization;

namespace SentinelEdge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));
            services.AddAuthorization(c =>
            {
                c.AddPolicy("FirewallAdmin", o => o.RequireAuthenticatedUser()
                                                   .RequireRole("Task.Firewall.Update")
                                                   .Build());
                c.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                                  .RequireRole("Task.Firewall.Read", "Task.Firewall.Update")
                                                                  .Build();
                c.FallbackPolicy = c.DefaultPolicy;
            });

            services.AddHealthChecks().AddCheck<UsgHealthCheck>("Firewall health check");
            services.AddOptions();
            services.Configure<UsgConfiguration>(Configuration.GetSection("UsgConfiguration"));
            services.AddHttpClient();
            services.AddScoped<ITalkToFirewall, UnifiFirewallService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SentinelEdge.Api",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Frode Hus",
                        Email = "frode@frodehus.dev",
                        Url = new Uri("https://www.frodehus.dev/")
                    },
                    Description = "Api for automatically updating security measures"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "OAuth2 access token"
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SentinelEdge.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapControllers();
            });
        }
    }
}
