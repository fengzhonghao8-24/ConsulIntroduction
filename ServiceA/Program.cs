var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConsulRegisterOptions>(builder.Configuration.GetSection("ConsulRegisterOptions"));
builder.Services.AddConsulRegister();

var app = builder.Build();

app.Services.GetService<IConsulRegister>()!.ConsulRegistAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthCheckMiddleware();

app.UseHttpsRedirection();

app.MapGet("/test", (IConfiguration configuration) =>
{
    return $"{Assembly.GetExecutingAssembly().FullName};当前时间：{DateTime.Now:G};Port：{configuration["ConsulRegisterOptions:Port"]}";
});

app.Run();

