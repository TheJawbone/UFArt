using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Identity;
using UFArt.Models.Newsfeed;
using UFArt.Models.TextAssets;

namespace UFArt.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ArtPiece> ArtPieces { get; set; }
        public DbSet<TechniqueDict> Techniques { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TextAsset> TextAssets { get; set; } 
    }
}
