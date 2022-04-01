using dvt_jibble_backend.DbContexts;
using dvt_jibble_backend.Dependencies;
using dvt_jibble_backend.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var DevelopmentOrigins = "_developmentOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: DevelopmentOrigins,
        builder =>
        {
            builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyMethod().AllowAnyHeader();
        });
});
// Add services to the container.
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EmployeeContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IDatabaseDependency>(x => new NpgsqlDependency(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseCors(DevelopmentOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
