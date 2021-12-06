using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Api.PontoDigital.Repository.OperacaoPonto;
using Api.PontoDigital.Repository.PessoaFisica;
using Api.PontoDigital.Repository.PessoaJuridica;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Api.PontoDigital.Repository.PessoaFisicaLogin;
using Api.PontoDigital.Repository.PessoaJuridicaFisica;
using Amazon.S3;
using Api.PontoDigital.Class;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Api.PontoDigital
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Injeção de Dependencia das Classes do Banco de Dados
            services.AddSingleton<IPessoaJuridicaRepository, PessoaJuridicaRepository>();
            services.AddSingleton<IPessoaFisicaRepository, PessoaFisicaRepository>();
            services.AddSingleton<IOperacaoPontoRepository, OperacaoPontoRepository>();
            services.AddSingleton<IPessoaFisicaLoginRepository, PessoaFisicaLoginRepository>();
            services.AddSingleton<IPessoaJuridicaFisicaRepository, PessoaJuridicaFisicaRepository>();
            services.AddSingleton<IExportarExcel, ExportarExcel>();
            #endregion

            services.AddAWSService<IAmazonS3>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                    });
            });
            //JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });


            //Swagger
            services.AddSwaggerGen(c =>
            {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ponto Digital", Version = "v1", Description = "Documentação da API do Ponto Digital" });
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               c.IncludeXmlComments(xmlPath);
               c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                   Description = @"Cabeçalho de autorização JWT usando o esquema Bearer. \r\n\r\n
                      Digite 'Portador' [espaço] e, em seguida, seu token na entrada de texto abaixo.
                      \r\n\r\ nExemplo: 'Portador 12345abcdef'",
                   Name = "Authorization",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.ApiKey,
                   Scheme = "Bearer"
               });

               c.AddSecurityRequirement(new OpenApiSecurityRequirement(){{
                       new OpenApiSecurityScheme
                       {
                           Reference = new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                           },
                           Scheme = "oauth2",
                           Name = "Bearer",
                           In = ParameterLocation.Header,
                       },
                       new List<string>()
                   }
               });
            });

            services.AddMvcCore().AddApiExplorer();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);

            });
            services.AddLocalization();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            var supportedCultures = new[]{ new CultureInfo("pt-BR")
};
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                FallBackToParentCultures = false
            });
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");
        }

        /// <summary>
        /// Swagger - ReApplyOptionalRouteParameterOperationFilter
        /// </summary>
        public class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
        {
            const string captureName = "routeParameter";
            /// <summary>
            /// Apply
            /// </summary>
            /// <param name="operation"></param>
            /// <param name="context"></param>
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var httpMethodAttributes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>();

                var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template?.Contains("?") ?? false);
                if (httpMethodWithOptional == null)
                    return;

                string regex = $"{{(?<{captureName}>\\w+)\\?}}";

                var matches = System.Text.RegularExpressions.Regex.Matches(httpMethodWithOptional.Template, regex);

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    var name = match.Groups[captureName].Value;

                    var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
                    if (parameter != null)
                    {
                        parameter.AllowEmptyValue = true;
                        parameter.Description = "Deve marcar \"Send empty value\" ou o Swagger passa uma vírgula para valores vazios, caso contrário";
                        parameter.Required = false;
                        parameter.Schema.Nullable = true;
                    }
                }
            }
        }
    }
}
