
using HomeApp.Common;
using HomeApp.Services;
using HomeApp.SqlModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Reflection;
using System.Text;

namespace HomeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173",
                        "http://78.23.192.63:5064",
                        "http://78.23.192.63",
                        "http://amaidassimpel.be",
                        "http://127.0.0.1:5173",
                        "http://192.168.1.139:5173",
                        "http://127.0.0.1:5064",
                        "http://192.168.1.139",
                        "http://192.168.1.139:5064"
                        ).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            builder.Services.AddDbContext<HomeDbContext>((options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
            });
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Auth:HashPass"]!
                )
            ),

            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    $"JWT ERROR: {context.Exception.Message}"
                );

                return Task.CompletedTask;
            }
        };
    });
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",

                    Title = "Mextend REST api",
                    Description = "An ASP.NET Core Web API for managing Mextend items",
                    TermsOfService = new Uri("https://cosnt.be/"),
                    Contact = new OpenApiContact
                    {
                        Name = "COSNT",
                        Url = new Uri("https://cosnt.be/")
                    },

                });
                options.AddServer(new OpenApiServer
                {
                    Url = builder.Configuration.GetConnectionString("RestServerApiUrl"),
                    Description = "HTTP Development Server"
                });

                // using System.Reflection;
                //  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            var app = builder.Build();
            CopyFilesFromUploadsToOptimizedIfNotExist(builder).Wait();

            var location = builder.Configuration.GetValue<string>("Paths:FileUploadLocationOptimized");
            //app.UseHttpsRedirection()
            app.MapStaticAssets();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(location!),
                RequestPath = "/uploadsoptimized"

            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // app.MapOpenApi();
            }
            app.UseCors("AllowReactApp");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            app.MapGet("/v1/swagger.json", async context =>
            {
                context.Response.Redirect("/swagger/v1/swagger.json", permanent: false);
            });
            //app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private async static Task CopyFilesFromUploadsToOptimizedIfNotExist(WebApplicationBuilder builder)
        {
            var destinationFolder = builder.Configuration.GetValue<string>("Paths:FileUploadLocation");
            var destinationFolderOptimized = builder.Configuration.GetValue<string>("Paths:FileUploadLocationOptimized");
            if (string.IsNullOrEmpty(destinationFolder)) return;
            if (string.IsNullOrEmpty(destinationFolderOptimized)) return;
            if (!Directory.Exists(destinationFolder))
            {
                try
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                catch (Exception)
                {

                }

            }
            if (!Directory.Exists(destinationFolderOptimized))
            {
                try
                {
                    Directory.CreateDirectory(destinationFolderOptimized);
                }
                catch (Exception)
                {

                }

            }

            var files = Directory.GetFiles(destinationFolder);


            foreach (var file in files)
            {
                string extension = Path.GetExtension(file).ToLower();// get extension
                string fileName = Path.GetFileName(file);//get filename
                string destinationOptimized = Path.Combine(destinationFolderOptimized, fileName);// destination optimized file path
                if (File.Exists(destinationOptimized)) continue;
                if (extension.Contains(".png") || extension.Contains(".jpg") || extension.Contains(".jpeg"))
                {
                    using var image = await Image.LoadAsync(file);

                    image.Mutate(x =>
                    {
                        x.AutoOrient();
                        x.Resize(new ResizeOptions
                        {
                            Size = new Size(600, 600),
                            Mode = ResizeMode.Max
                        });
                    });
                    var outPutStream = new FileStream(destinationOptimized, FileMode.Create);
                    await image.SaveAsync(outPutStream, new WebpEncoder
                    {
                        Quality = 80
                    });
                }
            }

        }
    }
}
