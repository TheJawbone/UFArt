using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;

namespace UFArt
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:UFArtDb:ConnectionString"]));
            services.AddTransient<IGalleryRepository, GalleryRepository>();
            services.AddTransient<ITechniqueRepository, TechniqueRepository>();
            services.AddTransient<INewsfeedRepository, NewsfeedRepository>();
            services.Configure<StorageSettings>(Configuration.GetSection("StorageSettings"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "oil_paintings_pagination",
                    template: "OilPaintings/Page{pageNumber}",
                    defaults: new { Controller = "Gallery", action = "ListOilPaintings" });
                routes.MapRoute(
                    name: "watercolor_paintings_pagination",
                    template: "WatercolorPaintings/Page{pageNumber}",
                    defaults: new { Controller = "Gallery", action = "ListWatercolorPaintings" });
                routes.MapRoute(
                    name: "pottery_pagination",
                    template: "Pottery/Page{pageNumber}",
                    defaults: new { Controller = "Gallery", action = "ListPottery" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //DBInitializer.ClearDatabase(app);
            DBInitializer.EnsurePopulated(app);
        }
    }
}
