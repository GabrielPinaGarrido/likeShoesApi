using Microsoft.EntityFrameworkCore;

namespace likeshoesapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddControllers();
            //services.AddEndpointsApiExplorer();

            //Cors para poder realizar consultas desde cualquier fuente
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Habilitar CORS - Cors para poder realizar consultas desde cualquier fuente
            app.UseCors("AllowAnyOrigin");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
