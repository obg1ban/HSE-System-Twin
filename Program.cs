using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HseDashboard.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<HseDashboard.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the mock/live data service as a singleton so the "live" timer
// keeps ticking and pushing updates to any component that subscribes.
builder.Services.AddSingleton<DashboardDataService>();

await builder.Build().RunAsync();
