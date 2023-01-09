using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using Restorantt.Data;
using Restorantt.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restorantt
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                CloseButton = true,
                PositionClass = ToastPositions.BottomRight,
                PreventDuplicates = true


            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders() //burda silme i�lemi yapt�k //sonra IdentityRole Ekledik 
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()/////////////////////////////////////////////////////
                .AddGoogle(options =>//google+facebook ayar�
                {
                    options.ClientId = Configuration["App:GoogleClientId"];
                    options.ClientSecret = Configuration["App:GoogleCLientSecret"];
                });
                //.AddFacebook(options => //paket y.k komutlar� �al��t�rd�ktan sonra tan�mlan�r
                //{
                //    options.AppId = Configuration["App:FacebookClientId"];
                //    options.ClientSecret = Configuration["App:FacebookClientSecret"];
                //});
                /////////////////////////////////////////////////////////
            services.AddSingleton<IEmailSender, EmailSender>(); //gerekli servisi a�t�m. 
            services.Configure<EmailOptions>(Configuration); //email i�in gerekli servisi a�t�m cofiguration=yap�land�rma
            services.AddControllersWithViews() //control ve onay veriyor her sayfada user giri�i istemiyor
                .AddRazorRuntimeCompilation(); //front end deki de�i�iklikleri de�i�iklikle g�nceller
            services.AddRazorPages(); ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            services.ConfigureApplicationCookie(options => ///////////------------ASP.NET CORE 5.0 iSKELE k�ML���-----------///////////
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });  ///////////�	B�YLECE KULLANICI YETK�S� OLMAYAN SAYFAYA G�R�� YAPMAK �STERSE LOG�N SAYFASINA Y�NLEND�R�LECEK///////////

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Musteri}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
