using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParishManager.Controller;
using ParishManager.Model;

namespace UnitTests;

public class UnitTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task AddParishToDatabase_ShouldReturnOk_AndSaveToDb()
    {
        await using var context = GetDatabaseContext();
        var controller = new ParishController(context);
        var parish = new Parish()
        {
            City = "Szczecin",
            Address = "Broniewskiego 18",
            Patronage = "Św. Kazimierz",
            Parishioners = 1500
        };


        var result = await controller.Create(parish);
        var parishInDb = await context.Parishes.FirstOrDefaultAsync(p => p.Address == "Broniewskiego 18");


        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(parishInDb);
        Assert.Equal("Św. Kazimierz", parishInDb.Patronage);
    }

    [Fact]
    public async Task GetAllFromDatabase()
    {
        await using var context = GetDatabaseContext();
        var controller = new ParishController(context);
        var parish01 = new Parish()
        {
            City = "Szczecin",
            Address = "Broniewskiego 18",
            Patronage = "Św. Kazimierz",
            Parishioners = 1500
        };
        var parish02 = new Parish()
        {
            City = "Szczecin",
            Address = "Plater 25",
            Patronage = "Bożego Ciała",
            Parishioners = 2000
        };
        context.Parishes.Add(parish01);
        context.Parishes.Add(parish02);
        await context.SaveChangesAsync();


        var result = await controller.GetAll();


        var okResult = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<List<Parish>>(okResult.Value);


        Assert.Equal(2, list.Count);
        Assert.Contains(list, p => p.Address == "Broniewskiego 18");
        Assert.Contains(list, p => p.Address == "Plater 25");
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenDbIsEmpty()
    {
        await using var context = GetDatabaseContext();
        var controller = new ParishController(context);

        var result = await controller.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<List<Parish>>(okResult.Value);

        Assert.Empty(list);
    }

    [Fact]
    public async Task GetById_ShouldFindParish()
    {
        await using var context = GetDatabaseContext();
        var controller = new ParishController(context);
        var parish01 = new Parish()
        {
            City = "Szczecin",
            Address = "Broniewskiego 18",
            Patronage = "Św. Kazimierz",
            Parishioners = 1500
        };
        var parish02 = new Parish()
        {
            City = "Szczecin",
            Address = "Plater 25",
            Patronage = "Bożego Ciała",
            Parishioners = 2000
        };
        context.Parishes.Add(parish01);
        context.Parishes.Add(parish02);
        await context.SaveChangesAsync();


        var result = await controller.GetById(2);


        var okResult = Assert.IsType<OkObjectResult>(result);
        var foundParish = Assert.IsAssignableFrom<Parish>(okResult.Value);

        Assert.Equal(parish02.Id, foundParish.Id);
        Assert.Equal(parish02.Address, foundParish.Address);
        Assert.Equal(parish02.City, foundParish.City);
        Assert.Equal(parish02.Patronage, foundParish.Patronage);
    }


    [Fact]
    public async Task GetById_LackOfSearchingId()
    {
        await using var context = GetDatabaseContext();
        var controller = new ParishController(context);

        var result = await controller.GetById(2);

        Assert.IsType<NotFoundResult>(result);
        
    }
}