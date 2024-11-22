using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PruebaIngresoBibliotecario.Infrastructure.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Infrastructure;
using PruebaIngresoBibliotecario.Core.Abstractions.Repotories;
using PruebaIngresoBibliotecario.Core.Abstractions.Services;
using PruebaIngresoBibliotecario.Core.Services;
using PruebaIngresoBibliotecario.Infrastructure.Repositories;
using AutoMapper;

namespace PruebaIngresoBibliotecario.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerDocument();

            services.AddDbContext<PersistenceContext>(opt =>
            {
                opt.UseInMemoryDatabase("PruebaIngreso");
            });

            services.AddControllers(mvcOpts =>
            {
            });
            services.AddTransient<IPrestamoService, PrestamoService>();
            services.AddTransient<IPrestamoRepository, PrestamoRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }


        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();

        }
    }
}
