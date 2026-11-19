using BlazingPizza.Data;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OrderState>();
builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseSqlite("Data Source=pizza.db"));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapStaticAssets();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();

    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

app.Run();