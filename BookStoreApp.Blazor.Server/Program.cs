using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Authentication;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Adding localStorage access
builder.Services.AddBlazoredLocalStorage();

// Adding HttpClient to access API
builder.Services.AddHttpClient<IClient, Client>(client =>
{
	client.BaseAddress = new Uri("https://localhost:7091");
});

// Register Authentication Service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Register and overwrite Authentication State Provider
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
	p.GetRequiredService<ApiAuthenticationStateProvider>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
