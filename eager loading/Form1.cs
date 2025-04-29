

using Microsoft.EntityFrameworkCore;

namespace eager_loading
{
    public partial class Form1 : Form
    {
        public Form1()
        { 

        }
        public static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // пересоздадим базу данных
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
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                var companies = db.Companies
                     .Include(c => c.Users)  // добавляем данные по пользователям
                     .ToList();
                foreach (var company in companies)
                {
                    Console.WriteLine(company.Name);
                    // выводим сотрудников компании
                    foreach (var user in company.Users)
                        Console.WriteLine(user.Name);
                    Console.WriteLine("----------------------");     // для красоты
                }
            }
        }
    }
}
