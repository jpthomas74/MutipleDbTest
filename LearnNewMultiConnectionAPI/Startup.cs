using LearnNewMultiConnectionAPI.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnNewMultiConnectionAPI
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
            //From Mr Browne

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddDbContext<CustomerDataContext>((sp, options) =>
            {

                var requestContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var constr = GetConnectionStringFromRequestContext(requestContext);
                options.UseSqlServer(constr, o => o.UseRelationalNulls());

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetCoreEfTest", Version = "v1" });
            });



            //Original 
            //services.AddDbContext<CustomerDataContext>(options =>
            //                                           options.UseSqlServer(Configuration.GetConnectionString("APIConnectionString")));

            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnNewMultiConnectionAPI", Version = "v1" });
            //});
        }

        private string GetConnectionStringFromRequestContext(HttpContext requestContext)
        {
            var host = requestContext.User;
            var dbname = "AspNetCoreEfTest";


            var claims = requestContext.User.Claims;

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnNewMultiConnectionAPI v1"));
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
