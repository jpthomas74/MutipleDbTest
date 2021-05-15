using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace AspNetCoreEfTest
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

          
            services.AddDbContext<Db>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("APIConnectionString")));

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.WithOrigins("https://localhost:44331").AllowAnyHeader().AllowAnyMethod());
            });

            services.AddControllers();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("https://localhost:44331",
            //                                              "https://localhost:44332")
            //                                              .AllowAnyHeader()
            //                                              .AllowAnyMethod();
            //                      });
            //});

            // services.AddHttpContextAccessor();

            //services.AddDbContext<Db>((sp, options) =>
            //{

            //    //var requestContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            //    //var constr = GetConnectionStringFromRequestContext(requestContext);

            //    var constr = "Server=(localdb)\\mssqllocaldb;Database=AspNetCoreEfTest;Integrated Security=true";
            //    options.UseSqlServer(constr, o => o.UseRelationalNulls());

            //});

            //For Authentication
            //-CORS




            //---JWT---Put On Hold
            //// Adding Authentication  
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})

            //// Adding Jwt Bearer  
            //.AddJwtBearer(options =>
            //{
            //    options.SaveToken = true;
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = Configuration["JWT:ValidAudience"],
            //        ValidIssuer = Configuration["JWT:ValidIssuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
            //    };
            //});

            //End Authentication

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Open", builder => builder.WithOrigins("https://localhost:44331").AllowAnyHeader().AllowAnyMethod());
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetCoreEfTest", Version = "v1" });
            });

        }

        private string GetConnectionStringFromRequestContext(HttpContext requestContext)
        {
            var host = requestContext.User;
            var dbname = "AspNetCoreEfTest";
            if (host.Claims.Count(e => e.Type == "db") > 0)
                {
                    var db = host.Claims.FirstOrDefault(e => e.Type == "db").Value;
                    if (db == "db1")
                    {
                        dbname = "AspNetCoreEfTest";
                    }
                    else if (db == "db2")
                    {
                        dbname = "AspNetCoreEfTestTwo";
                    }
                }
          
             return "Server=(localdb)\\mssqllocaldb;Database=" + dbname + ";Integrated Security=true";
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCoreEfTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //var ob  = new DbContextOptionsBuilder<Db>();
            //ob.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AspNetCoreEfTest;Integrated Security=true");
            //using (var db = new Db(ob.Options ))
            //{
            //    db.Database.EnsureCreated();
            //}
        }
    }
}
