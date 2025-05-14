using Microsoft.EntityFrameworkCore;

namespace GR1.Models;


public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<StudentModel> Students { get; set; }
    public DbSet<RoomModel> Rooms { get; set; }
    public DbSet<LogModel> Logs { get; set; }
    public DbSet<UserModel> Users { get; set; } 
    public DbSet<ClassModel> Classes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<LogModel>()
            .HasKey(a => new { a.StudentCode, a.RoomCode, a.Timestamp });
        // Configure relationships
        modelBuilder.Entity<LogModel>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Logs)  // Một sinh viên có thể có nhiều bản ghi Log
            .HasForeignKey(a => a.StudentCode)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LogModel>()
            .HasOne(a => a.Room)
            .WithMany(r => r.Logs)  // Một phòng có thể có nhiều bản ghi Log
            .HasForeignKey(a => a.RoomCode)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<ClassModel>()
            .HasOne(c => c.Teacher)
            .WithMany() // Một giáo viên có thể dạy nhiều lớp
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}

