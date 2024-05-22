using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using OenskeBroenden.Components;
using OenskeBroenden.Utils;
using Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddSingleton<TokenUpdateService>();
builder.Services.AddSingleton<ITokenRepo, TokenRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IWishRepo, WishRepo>();
builder.Services.AddScoped<IHistoryRepo, HistoryRepo>();
builder.Services.AddScoped<IChatRepo, ChatRepo>();

builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<Auth>());
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<SignalREvents>();
builder.Services.AddTransient<ChatConnector>();
builder.Services.AddScoped<SignalRUtil>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
