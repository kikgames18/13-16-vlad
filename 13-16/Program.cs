using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/api/users", async (ApplicationDbContext db) =>
{
    return await db.profiles.ToListAsync();
});

app.MapPost("/api/users", async (ApplicationDbContext db, Profile user) =>
{
    await db.profiles.AddAsync(user);

    await db.SaveChangesAsync();

    return "пользователь создан";
});

app.MapGet("/api/users/{id}", async (int id, ApplicationDbContext db) =>
{
    return await db.profiles.FindAsync(id);
});

app.MapDelete("/api/users/{id}", (int id, ApplicationDbContext db) =>
{

    var user = db.profiles.Find(id);

    db.profiles.Remove(user);

    db.SaveChanges();

    return "пользователь удален";
});

app.MapPatch("/api/users/{id}", (ApplicationDbContext db, Profile user, int id) =>
{
    var usr = db.profiles.Find(id);
    usr.name = user.name;

    db.SaveChanges();
    return "информаци€ успешно о пол-е успешно обновлена";
});

app.Run();
public class Profile
{
    public int id { get; set; }
    public string? name { get; set; }
}

// ќпредел€ем класс, который наследуетс€ от базового класса контекста бд
public class ApplicationDbContext : DbContext
{
    public DbSet<Profile> profiles { get; set; }

    //  онструктор вызывает, констуктор базового класса
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
