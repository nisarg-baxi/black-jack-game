using GameAPIClient.Services;
using GameAPIClient.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddSingleton<IMatchmakingService, MatchmakingService>();
builder.Services.AddSingleton<IStoreService, StoreService>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
