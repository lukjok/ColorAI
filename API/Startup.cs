using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorAIML.Model.DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using API.Tools;
using API.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace API
{
    public class Startup
    {
        private readonly string _onnxModelFilePath;
        private readonly string _mlnetModelFilePath;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _onnxModelFilePath = GetAbsolutePath(Configuration["MLModel:OnnxModelFilePath"]);
            _mlnetModelFilePath = GetAbsolutePath(Configuration["MLModel:MLNETModelFilePath"]);

            OnnxModelConfigurator onnxModelConfigurator = new OnnxModelConfigurator(_onnxModelFilePath);

            onnxModelConfigurator.SaveMLNetModel(_mlnetModelFilePath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPredictionEnginePool<ColorData, ColorDataPrediction>().FromFile(_mlnetModelFilePath);
            services.AddSingleton<IColorRecognisionService, ColorRecognisionService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ColorAI", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(options.DefaultPolicyName,
                builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ColorAI API V1");
            });
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
