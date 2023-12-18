using Microsoft.EntityFrameworkCore;
using WebApiSignalR.Data;
using WebApiSignalR.Hubs;
using WebApiSignalR.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddSignalR();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://127.0.0.1:5500/").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var conn = builder.Configuration.GetConnectionString("myconn");

builder.Services.AddDbContext<RoomDbContext>(options =>
{
    options.UseSqlServer(conn);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x =>
x.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials());

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/offers");
});
app.MapControllers();

app.Run();
