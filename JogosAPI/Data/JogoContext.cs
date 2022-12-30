using JogosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JogosAPI.Data;

public class JogoContext : DbContext
{
    public JogoContext(DbContextOptions<JogoContext> opts): base(opts)
    {
    }

    public DbSet<Jogo> Jogos { get; set; }
}
