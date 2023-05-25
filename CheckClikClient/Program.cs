using CheckClickClient;
using CheckClikClient;
using CheckClikClient.Handlers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<AppSettingsKeysInfo>(builder.Configuration);
builder.Services.AddScoped<ErrorHandler, ErrorHandler>();
builder.Services.AddSingleton<CommonHeader, CommonHeader>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
AppUtils.Configure(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IHttpContextAccessor>());

//app.UseMvc();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=NIndex}/{id?}");

app.Run();
