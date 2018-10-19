using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;

namespace UFArt.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ArtPiece> ArtPieces { get; set; }
        public DbSet<TechniqueDict> Techniques { get; set; }
        public DbSet<News> News { get; set; }
    }
}
