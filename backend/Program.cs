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
        policy.WithOrigins("http://localhost:3000")  // React development server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowSpecificOrigin");

app.MapControllers();
app.Run();
