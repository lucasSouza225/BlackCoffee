using BlackCoffee.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlackCoffee.API.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedUsuarioPadrao(builder);
        SeedCategoriaPadrao(builder);
        SeedProdutoPadrao(builder);
    }

    private static void SeedUsuarioPadrao(ModelBuilder builder)
    {
        #region Roles
        List<IdentityRole> roles = new()
        {
            new IdentityRole() {
               Id = "0b44ca04-f6b0-4a8f-a953-1f2330d30894",
               Name = "Administrador",
               NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole() {
               Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
               Name = "Usuário",
               NormalizedName = "USUÁRIO"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Usuário
        List<Usuario> usuarios = new()
        {
            new Usuario(){
                Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
                Email = "lucas2469@ygmail.com",
                NormalizedEmail = "LUCAS2469Y@GMAIL.COM",
                UserName = "lucas2469@ygmail.com",
                NormalizedUserName = "LUCAS2469Y@GMAIL.COM",
                LockoutEnabled = true,
                EmailConfirmed = true,
                Nome = "Lucas de Souza Santos",
                DataNascimento = DateTime.Parse("1997-06-27"),
                Foto = "/img/usuarios/avatar.png"
            }
        };

        foreach (var user in usuarios)
        {
            PasswordHasher<Usuario> pass = new();
            user.PasswordHash = pass.HashPassword(user, "123456");
        }

        builder.Entity<Usuario>().HasData(usuarios);
        #endregion

        #region UserRole
        List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>()
            {
                UserId = usuarios[0].Id,
                RoleId = roles[0].Id
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion
    }

    private static void SeedCategoriaPadrao(ModelBuilder builder)
    {
        List<Categoria> categorias = new()
        {
            new Categoria { Id = 1, Nome = "Cafés Especiais" },
            new Categoria { Id = 2, Nome = "Cápsulas" },
            new Categoria { Id = 3, Nome = "Grãos" },
            new Categoria { Id = 4, Nome = "Acessórios" },
            new Categoria { Id = 5, Nome = "Doces e acompanhamentos" }
        };
        builder.Entity<Categoria>().HasData(categorias);
    }

    private static void SeedProdutoPadrao(ModelBuilder builder)
{
    var produtos = new List<Produto>
    {
        // Categoria 1 - Cafés em Grãos
        new Produto { Id = 1, CategoriaId = 1, Nome = "Café em Grãos Arábica Premium", Descricao = "Grãos 100% arábica torrados artesanalmente, com aroma intenso e notas de chocolate.", Qtde = 50, ValorCusto = 30.00m, ValorVenda = 45.90m, Destaque = true, Foto = "graos-arabica-premium.jpg" },
        new Produto { Id = 2, CategoriaId = 1, Nome = "Café em Grãos Gourmet 500g", Descricao = "Blend equilibrado com torra média e acidez suave.", Qtde = 40, ValorCusto = 25.00m, ValorVenda = 39.50m, Destaque = false, Foto = "graos-gourmet.jpg" },
        new Produto { Id = 3, CategoriaId = 1, Nome = "Café em Grãos Espresso Forte", Descricao = "Café encorpado, ideal para máquinas de espresso.", Qtde = 60, ValorCusto = 27.00m, ValorVenda = 42.00m, Destaque = false, Foto = "graos-espresso-forte.jpg" },

        // Categoria 2 - Cafés Moídos
        new Produto { Id = 4, CategoriaId = 2, Nome = "Café Moído Tradicional 500g", Descricao = "Café com moagem fina, pronto para coadores e cafeteiras elétricas.", Qtde = 100, ValorCusto = 20.00m, ValorVenda = 29.90m, Destaque = true, Foto = "moido-tradicional.jpg" },
        new Produto { Id = 5, CategoriaId = 2, Nome = "Café Moído Extra Forte", Descricao = "Blend intenso com sabor marcante e aroma persistente.", Qtde = 80, ValorCusto = 22.00m, ValorVenda = 32.00m, Destaque = false, Foto = "moido-extra-forte.jpg" },
        new Produto { Id = 6, CategoriaId = 2, Nome = "Café Moído Orgânico 250g", Descricao = "Café certificado, cultivado sem agrotóxicos e com torra artesanal.", Qtde = 70, ValorCusto = 18.00m, ValorVenda = 27.50m, Destaque = false, Foto = "moido-organico.jpg" },

        // Categoria 3 - Cápsulas de Café
        new Produto { Id = 7, CategoriaId = 3, Nome = "Cápsulas Espresso Intenso (10 un)", Descricao = "Cápsulas compatíveis com Nespresso®, com sabor encorpado e final prolongado.", Qtde = 90, ValorCusto = 18.00m, ValorVenda = 29.90m, Destaque = true, Foto = "capsulas-intenso.jpg" },
        new Produto { Id = 8, CategoriaId = 3, Nome = "Cápsulas Cappuccino Cremoso (10 un)", Descricao = "Mistura equilibrada de café e leite, com espuma cremosa.", Qtde = 60, ValorCusto = 20.00m, ValorVenda = 32.90m, Destaque = false, Foto = "capsulas-cappuccino.jpg" },
        new Produto { Id = 9, CategoriaId = 3, Nome = "Cápsulas Descafeinado Suave (10 un)", Descricao = "Sabor leve e aromático, ideal para consumo à noite.", Qtde = 50, ValorCusto = 17.00m, ValorVenda = 28.50m, Destaque = false, Foto = "capsulas-descafeinado.jpg" },

        // Categoria 4 - Bebidas Especiais
        new Produto { Id = 10, CategoriaId = 4, Nome = "Latte Vanilla 300ml", Descricao = "Bebida cremosa à base de café espresso e essência natural de baunilha.", Qtde = 30, ValorCusto = 12.00m, ValorVenda = 18.90m, Destaque = true, Foto = "latte-vanilla.jpg" },
        new Produto { Id = 11, CategoriaId = 4, Nome = "Cold Brew Clássico 500ml", Descricao = "Café extraído a frio por 12 horas, com sabor suave e refrescante.", Qtde = 25, ValorCusto = 15.00m, ValorVenda = 22.00m, Destaque = false, Foto = "cold-brew.jpg" },
        new Produto { Id = 12, CategoriaId = 4, Nome = "Mocha Tradicional 300ml", Descricao = "Café espresso com leite vaporizado e calda de chocolate belga.", Qtde = 40, ValorCusto = 13.00m, ValorVenda = 19.50m, Destaque = false, Foto = "mocha.jpg" },

        // Categoria 5 - Acessórios para Café
        new Produto { Id = 13, CategoriaId = 5, Nome = "Coador de Pano Reutilizável", Descricao = "Filtro de pano tradicional, ideal para café coado artesanal.", Qtde = 120, ValorCusto = 8.00m, ValorVenda = 15.00m, Destaque = false, Foto = "coador-pano.jpg" },
        new Produto { Id = 14, CategoriaId = 5, Nome = "Prensa Francesa 600ml", Descricao = "Cafeteira de vidro e aço inox para preparo de café prensado.", Qtde = 20, ValorCusto = 85.00m, ValorVenda = 120.00m, Destaque = true, Foto = "prensa-francesa.jpg" },
        new Produto { Id = 15, CategoriaId = 5, Nome = "Xícara de Cerâmica 200ml", Descricao = "Xícara artesanal com design minimalista e acabamento fosco.", Qtde = 100, ValorCusto = 15.00m, ValorVenda = 25.00m, Destaque = false, Foto = "xicara-ceramica.jpg" },

        // Categoria 6 - Máquinas e Equipamentos
        new Produto { Id = 16, CategoriaId = 6, Nome = "Máquina Espresso Automática Barista Pro", Descricao = "Equipamento automático com vaporizador e ajuste de moagem.", Qtde = 5, ValorCusto = 2500.00m, ValorVenda = 3500.00m, Destaque = true, Foto = "maquina-barista-pro.jpg" },
        new Produto { Id = 17, CategoriaId = 6, Nome = "Moedor de Café Elétrico Inox", Descricao = "Moagem ajustável para diferentes métodos de preparo.", Qtde = 15, ValorCusto = 180.00m, ValorVenda = 280.00m, Destaque = false, Foto = "moedor-inox.jpg" },
        new Produto { Id = 18, CategoriaId = 6, Nome = "Balança Digital para Café", Descricao = "Balança de precisão com temporizador integrado.", Qtde = 30, ValorCusto = 100.00m, ValorVenda = 160.00m, Destaque = false, Foto = "balanca-digital.jpg" },

        // Categoria 7 - Doces e Acompanhamentos
        new Produto { Id = 19, CategoriaId = 7, Nome = "Biscoito Amanteigado Artesanal", Descricao = "Biscoito leve e crocante, ideal para acompanhar o café.", Qtde = 80, ValorCusto = 10.00m, ValorVenda = 18.00m, Destaque = false, Foto = "biscoito-artesanal.jpg" },
        new Produto { Id = 20, CategoriaId = 7, Nome = "Chocolate 70% Cacau Gourmet", Descricao = "Chocolate artesanal com notas de café e textura cremosa.", Qtde = 60, ValorCusto = 15.00m, ValorVenda = 22.50m, Destaque = true, Foto = "chocolate-70.jpg" },
        new Produto { Id = 21, CategoriaId = 7, Nome = "Mini Brownie de Nozes (6 un)", Descricao = "Brownies artesanais com sabor intenso e textura macia.", Qtde = 50, ValorCusto = 18.00m, ValorVenda = 28.00m, Destaque = false, Foto = "brownie-nozes.jpg" }
    };

    builder.Entity<Produto>().HasData(produtos);
}

}
