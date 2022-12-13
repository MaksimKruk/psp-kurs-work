using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PSP.GameAPI.Services.CarService;
using PSP.GameAPI.Services.GameService;
using PSP.GameAPI.Services.LevelService;
using PSP.GameAPI.Services.MoveService;
using PSP.GameAPI.Services.PrizeService;

namespace PSP.GameAPI
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
            services.AddSingleton<ICarService, CarService>();
            services.AddSingleton<IMoveService, MoveService>();
            services.AddSingleton<IPrizeService, PrizeService>();
            services.AddSingleton<ILevelService, LevelService>();

            services.AddSingleton<IGameService, GameService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RaceGame.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RaceGame.Api v1"));
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "RaceGame.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
