﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Controller;
using WebApiAutores.Filtros;
using WebApiAutores.Midllewares;
using WebApiAutores.Servicios;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get;}
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddDbContext<ApplicationsDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddTransient<IServicio, ServicioA>();
            //services.AddTransient<ServicioA>();
            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();
            services.AddTransient<MiFiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            app.UseLoguearRespuestaHTTP();

            app.Map("/ruta1", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Estoy interceptando la tuberia");
                });
            });   

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","WebAPIAutores v1"));
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers();
            });
        }
    }

}