using Microsoft.Extensions.ML;
using MovieRecommender.DataStructures;
using MovieRecommender.Services;

namespace MovieRecommender;

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
        services.AddSingleton<IProfileService, ProfileService>();
        services.AddSingleton<IMovieService, MovieService>();
        services.AddPredictionEnginePool<MovieRating, MovieRatingPrediction>().FromFile(Configuration["MLModelPath"]);

        services.AddControllersWithViews();
        services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler("/Movies/Error");

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllerRoute("default", "{controller=Movies}/{action=Profiles}/{id?}");
        });
    }
}