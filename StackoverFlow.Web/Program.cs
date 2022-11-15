using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StackoverflowAPI.Web;
using StackoverflowAPI.Web.Services;
using StackoverflowAPI.Web.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.stackexchange.com/2.3") });

builder.Services.AddScoped<IStackOverflowService, StackOverflowService>();

//builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
