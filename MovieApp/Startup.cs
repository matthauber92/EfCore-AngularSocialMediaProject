﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieApp.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using MovieApp.Services;

namespace MovieApp
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
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Movie API", Description = "Swagger Core API" });
            });

            services.AddDbContextPool<APIContext>( // replace "YourDbContext" with the class name of your DbContext
                options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<APIContext>();

            services.AddScoped<BaseService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddCors(options =>
            {
                options.AddPolicy(
                  "CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });


            //Jwt Authentication
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x =>   //Start Authntication to send differen types of Auth
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Set tolken
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => //Configure tolken
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false; //Set false to not save tolken in server
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters //Define tolken for user Auth
                {
                    ValidateIssuerSigningKey = true, //system validate key
                    IssuerSigningKey = new SymmetricSecurityKey(key), //Assign key for Jwt signature
                    ValidateIssuer = false,
                    ValidateAudience = false, //Defines who generated tolken
                    ClockSkew = TimeSpan.Zero //Set expiration time of tolken
                };
            });

        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
            });

            app.UseMvc();
        }
    }
}
