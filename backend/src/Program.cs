using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<VnExpressScrapService>();
builder.Services.AddHttpClient<TuoiTreScrapService>();
builder.Services.AddControllers();
builder.Services.AddTransient<VnExpressScrapService>();
builder.Services.AddTransient<TuoiTreScrapService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowSpecificOrigin");

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
});

app.MapControllers();
app.Run();
