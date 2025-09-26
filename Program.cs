var builder = WebApplication.CreateBuilder(args);

// Leer cadetería desde JSON
var cadeteriaData = File.ReadAllText("cadeteria.json");
var cadeteria = System.Text.Json.JsonSerializer.Deserialize<Cadeteria>(cadeteriaData);
// Add services to the container.
builder.Services.AddSingleton(cadeteria);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddSingleton<tl2_tp4_2025_ramiro_2k2>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

   
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
