using Azure.Identity;
using Feature_Flags;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri("https://mtestazure.azconfig.io"), new DefaultAzureCredential());
    options.UseFeatureFlags();
});


//var connectionString = builder.Configuration.GetConnectionString("appConfiguration");
//builder.Configuration.AddAzureAppConfiguration(option =>
//{
//    option.Connect(connectionString);
//});


builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<BrowserFilter>();

builder.Services.AddControllers()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.IgnoreNullValues = true;
                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAzureAppConfiguration();
app.UseAuthorization();

app.MapControllers();

app.Run();
