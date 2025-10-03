using GiftOfTheGivers.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        public DbSet<User> Users { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<DisasterReport> DisasterReports { get; set; }
        public DbSet<ReliefProject> ReliefProjects { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
        public DbSet<TaskApplication> TaskApplications { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
//work 