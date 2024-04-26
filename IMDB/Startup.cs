using IMDB.Repositories;
using IMDB.Repositories.Interfaces;
using IMDB.Services;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDB
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IMDB", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));

            // adding custom services
            services.TryAddSingleton<IActorRepository,ActorRepository> ();
            services.TryAddSingleton<IGenreRepository, GenreRepository>();
            services.TryAddSingleton<IMovieRepository, MovieRepository>();
            services.TryAddSingleton<IProducerRepository, ProducerRepository>();
            services.TryAddSingleton<IReviewRepository, ReviewRepository>();

            services.TryAddSingleton<IActorService, ActorService>();
            services.TryAddSingleton<IGenreService, GenreService>();
            services.TryAddSingleton<IMovieService, MovieService>();
            services.TryAddSingleton<IProducerService, ProducerService>();
            services.TryAddSingleton<IReviewService, ReviewService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IMDB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
