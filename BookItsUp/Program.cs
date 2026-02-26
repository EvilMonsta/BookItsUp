using BookItsUp.DataAccess;         
using BookItsUp.DataAccess.Repositories;
using BookItsUp.Application.Services;
using BookItsUp.Domain.Abstractions;
using BookItsUp.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("BookingDb"))
       .UseSnakeCaseNamingConvention());
builder.Services.AddRazorPages();


builder.Services.AddControllers();           
builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IWeeklyScheduleRepository, WeeklyScheduleRepository>();
builder.Services.AddScoped<IScheduleExceptionRepository, ScheduleExceptionRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IWeeklyScheduleService, WeeklyScheduleService>();
builder.Services.AddScoped<IScheduleExceptionService, ScheduleExceptionService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

var app = builder.Build();

app.UseMiddleware<ApiExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookItsUp API v1");
        c.RoutePrefix = "swagger"; 
    });
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();
