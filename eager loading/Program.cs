﻿using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new ApplicationContext())
{
    // пересоздадим базу данных
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    Position manager = new Position { Name = "Manager" };
    Position developer = new Position { Name = "Developer" };
    db.Positions.AddRange(manager, developer);

    City washington = new City { Name = "Washington" };
    db.Cities.Add(washington);

    Country usa = new Country { Name = "USA", Capital = washington };
    db.Countries.Add(usa);

    Company microsoft = new Company { Name = "Microsoft", Country = usa };
    Company google = new Company { Name = "Google", Country = usa };
    db.Companies.AddRange(microsoft, google);

    User tom = new User { Name = "Tom", Company = microsoft, Position = manager };
    User bob = new User { Name = "Bob", Company = google, Position = developer };
    User alice = new User { Name = "Alice", Company = microsoft, Position = developer };
    User kate = new User { Name = "Kate", Company = google, Position = manager };
    db.Users.AddRange(tom, bob, alice, kate);

    db.SaveChanges();
}
/*//с использованием метода Include
using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    Company microsoft = new Company { Name = "Microsoft" };
    Company google = new Company { Name = "Google" };
    db.Companies.AddRange(microsoft, google);

    User tom = new User { Name = "Tom", Company = microsoft };
    User bob = new User { Name = "Bob", Company = google };
    User alice = new User { Name = "Alice", Company = microsoft };
    User kate = new User { Name = "Kate", Company = google };
    db.Users.AddRange(tom, bob, alice, kate);
    db.SaveChanges();

    var users = db.Users.ToList();  // метод Include не используется
    foreach (var user in users)
        Console.WriteLine($"{user.Name} - {user.Company?.Name}");
}
// с использованием метода db.Companies
using (ApplicationContext db = new ApplicationContext())
{
    var companies = db.Companies.ToList();
    // получаем пользователей
    var users = db.Users
        //.Include(u => u.Company)  // подгружаем данные по компаниям
        .ToList();
    foreach (var user in users)
        Console.WriteLine($"{user.Name} - {user.Company?.Name}");
}*/
// с использованием метода ThenInclude()
using (ApplicationContext db = new ApplicationContext())
{
    // получаем пользователей
    var users = db.Users
                    .Include(u => u.Company)  // добавляем данные по компаниям
                        .ThenInclude(comp => comp!.Country)      // к компании добавляем страну 
                            .ThenInclude(count => count!.Capital)    // к стране добавляем столицу
                    .Include(u => u.Position) // добавляем данные по должностям
                    .ToList();
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Name} - {user.Position?.Name}");
        Console.WriteLine($"{user.Company?.Name} - {user.Company?.Country?.Name} - {user.Company?.Country?.Capital?.Name}");
        Console.WriteLine("----------------------");     // для красоты
    }
}