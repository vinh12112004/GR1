using Microsoft.EntityFrameworkCore;

namespace GR1.Models;


public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<StudentModel> Students { get; set; }
    public DbSet<RoomModel> Rooms { get; set; }
    public DbSet<LogModel> Logs { get; set; }

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

        // modelBuilder.Entity<StudentModel>().HasData(
        // new StudentModel { StudentCode = 20225779, FullName = "Nguyen Van A" },
        // new StudentModel { StudentCode = 20225780, FullName = "Le Thi B" }
        // );
        // var fixedTimestamp = new DateTime(2023, 1, 1, 0, 0, 0);
            
        // // Dữ liệu mẫu cho Rooms
        // modelBuilder.Entity<RoomModel>().HasData(
        //     new RoomModel { RoomCode = 101 },
        //     new RoomModel { RoomCode = 102 }
        // );

        // // Dữ liệu mẫu cho Logs
        // modelBuilder.Entity<LogModel>().HasData(
        //     new LogModel { StudentCode = 20225779, RoomCode = 101, Status = "IN",  Timestamp = fixedTimestamp.AddHours(1) },
        //     new LogModel { StudentCode = 20225780, RoomCode = 102, Status = "OUT", Timestamp = fixedTimestamp.AddHours(2) }
        // );
        }
}

