using Application;
using FarmLand_RPG_CommonIdentityServer;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler("/error");
app.UseAuthorization();
app.MapControllers();
app.Run();
