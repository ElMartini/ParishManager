using Microsoft.EntityFrameworkCore;
using ParishManager.Model;

namespace ParishManager;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: "Parishes"));
        
        //Below commands was used to create migration
        // builder.Services.AddDbContext<AppDbContext>(options =>
        //     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.Run();
    }
}