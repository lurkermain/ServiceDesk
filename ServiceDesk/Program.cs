using Diddi;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args);

// Разрешаем CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		policy =>
		{
			policy.AllowAnyOrigin()
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
});

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

/*    app.UseExceptionHandler("/Home/Error");
	app.UseHsts();*/

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultControllerRoute();

app.MapFallbackToFile("index.html");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
