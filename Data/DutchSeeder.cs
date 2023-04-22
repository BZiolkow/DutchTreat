using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("titan@dutchtreat.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Bartek",
                    LastName = "Bartmuth",
                    Email = "titan@dutchtreat.com",
                    UserName = "titan@dutchtreat.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            if (!_ctx.Products.Any())
            {
                var filepath = Path.Combine(_env.ContentRootPath, "data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _ctx.Products.AddRange(products);
                _ctx.SaveChanges();
            }
        }
    }
}
