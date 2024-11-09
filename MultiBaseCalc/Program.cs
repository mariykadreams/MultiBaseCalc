var builder = WebApplication.CreateBuilder(args);

// Додаємо необхідні сервіси
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Додаємо сесію
builder.Services.AddDistributedMemoryCache();
// ... rest of your configuration


// Додаємо сесію
builder.Services.AddDistributedMemoryCache(); // Використовуємо пам'ять для збереження сесії
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Час життя сесії
});

var app = builder.Build();

// Налаштування конвеєра обробки HTTP запитів
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Включаємо підтримку сесії
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
