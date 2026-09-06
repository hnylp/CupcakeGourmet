using CupcakeGourmet.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Data;

public static class DbInitializer
{
    public const string PerfilAdministrador = "Administrador";
    public const string PerfilCliente = "Cliente";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var papel in new[] { PerfilAdministrador, PerfilCliente })
        {
            if (!await roleManager.RoleExistsAsync(papel))
                await roleManager.CreateAsync(new IdentityRole(papel));
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        const string adminEmail = "admin@cupcakegourmet.com.br";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                NomeCompleto = "Administrador da Loja",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, PerfilAdministrador);
        }

        if (context.Categorias.Any())
            return;

        var categorias = new List<Categoria>
        {
            new() { Nome = "Cupcakes Tradicionais", Descricao = "Sabores clássicos que nunca saem de moda." },
            new() { Nome = "Cupcakes Recheados", Descricao = "Massa fofinha com recheios especiais." },
            new() { Nome = "Cupcakes Veganos", Descricao = "Sem ingredientes de origem animal, sem perder o sabor." },
            new() { Nome = "Kits e Combos", Descricao = "Caixas com várias unidades para festas e eventos." }
        };
        context.Categorias.AddRange(categorias);
        await context.SaveChangesAsync();

        var produtos = new List<Produto>
        {
            new() { Nome = "Cupcake Baunilha", Descricao = "Massa de baunilha com cobertura de buttercream.", Preco = 8.90m, QuantidadeEmEstoque = 50, CategoriaId = categorias[0].Id, ImagemUrl = "https://images.unsplash.com/photo-1614707267537-b85aaf00c4b7?w=500" },
            new() { Nome = "Cupcake Chocolate", Descricao = "Massa de chocolate meio amargo com ganache.", Preco = 9.50m, QuantidadeEmEstoque = 50, CategoriaId = categorias[0].Id, ImagemUrl = "https://images.unsplash.com/photo-1587668178277-295251f900ce?w=500" },
            new() { Nome = "Cupcake Red Velvet", Descricao = "Clássico americano com cream cheese.", Preco = 10.90m, QuantidadeEmEstoque = 40, CategoriaId = categorias[0].Id, ImagemUrl = "https://images.unsplash.com/photo-1519869325930-281384150729?w=500" },
            new() { Nome = "Cupcake Doce de Leite", Descricao = "Recheado com doce de leite argentino.", Preco = 11.50m, QuantidadeEmEstoque = 35, CategoriaId = categorias[1].Id, ImagemUrl = "https://images.unsplash.com/photo-1607478900766-efe13248b125?w=500" },
            new() { Nome = "Cupcake Ninho com Nutella", Descricao = "Leite Ninho recheado com Nutella.", Preco = 12.90m, QuantidadeEmEstoque = 35, CategoriaId = categorias[1].Id, ImagemUrl = "https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=500" },
            new() { Nome = "Cupcake Vegano de Cacau", Descricao = "Sem leite, ovos ou manteiga — 100% vegano.", Preco = 10.50m, QuantidadeEmEstoque = 25, CategoriaId = categorias[2].Id, ImagemUrl = "https://images.unsplash.com/photo-1519996529931-28324d5a630e?w=500" },
            new() { Nome = "Kit Festa (12 unidades)", Descricao = "Caixa com 12 cupcakes sortidos.", Preco = 99.90m, QuantidadeEmEstoque = 15, CategoriaId = categorias[3].Id, ImagemUrl = "https://images.unsplash.com/photo-1519340333755-56e9c1d04579?w=500" }
        };
        context.Produtos.AddRange(produtos);
        await context.SaveChangesAsync();
    }
}
