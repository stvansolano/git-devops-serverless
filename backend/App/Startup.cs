﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App
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
			services.AddCors(options =>
            {
                options.AddPolicy("ALLOW_ANY_ORIGIN",
                    builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .Build());
            });
			services.AddMvc(options => {
				options.OutputFormatters.Add(
					new JsonOutputFormatter(new Newtonsoft.Json.JsonSerializerSettings
					{
						Formatting = Newtonsoft.Json.Formatting.Indented
					},
					System.Buffers.ArrayPool<char>.Shared)
				);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseCors("ALLOW_ANY_ORIGIN");

			app.UseMvc(routes => 
                routes.MapRoute(
                    name: "api",
                    template: "{controller=Api}/{action=Index}/{id?}"));
		}
	}
}