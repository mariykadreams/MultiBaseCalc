var builder = WebApplication.CreateBuilder(args);

// ������ �������� ������
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// ������ ����
builder.Services.AddDistributedMemoryCache();
// ... rest of your configuration


// ������ ����
builder.Services.AddDistributedMemoryCache(); // ������������� ���'��� ��� ���������� ���
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ��� ����� ���
});

var app = builder.Build();

// ������������ ������� ������� HTTP ������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// �������� �������� ���
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
