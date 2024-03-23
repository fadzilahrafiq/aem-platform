using Microsoft.EntityFrameworkCore;

namespace AemAssessment.Models {
  public partial class DataContext : DbContext
    {
      public DataContext(DbContextOptions <DataContext> options)
        : base(options)
      { }

      public virtual DbSet<Platform> Platform { get; set; }
      public virtual DbSet<Well> Well { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        modelBuilder.Entity<Platform>(entity => {
          entity.HasKey(k => k.Id);
        });
        modelBuilder.Entity<Well>(entity => {
          entity.HasKey(k => k.Id);
        });

        OnModelCreatingPartial(modelBuilder);
      }

      partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}