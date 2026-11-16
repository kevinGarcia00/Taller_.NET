using Modules.Sensor.Infrastructure;
using Modules.Sensor.Integration.Endpoints;
using Modules.Sensor.Integration.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AgregarModuloSensor(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Sensor SolarUY");
app.MapHub<SensorHub>("/sensor/hub");
app.MapEstadoInversorEndpoints();

app.Run();
