using Client.ConsumeAPI.APIClient;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using UserMSDev.APIClient;
using UserMSDev.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    const string defaultCulture = "en";
    var supportedCultures = new[]
    {
        new CultureInfo(defaultCulture),
        new CultureInfo("ar")
    };

    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    //options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});


builder.Services.AddTransient<IDynamicMenuService, DynamicMenuService>();
builder.Services.AddScoped(typeof(IAPIClientService<>), typeof(APIClientService<>));
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ICommonService, CommonService>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});


builder.Services.AddDataProtection()
.SetApplicationName("User Management System")
.AddKeyManagementOptions(options =>
{
    options.NewKeyLifetime = new TimeSpan(180, 0, 0, 0);
    options.AutoGenerateKeys = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});





var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCors("Open");

app.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}");
app.Run();
