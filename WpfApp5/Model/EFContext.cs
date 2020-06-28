namespace WpfApp5.Model
{
    using System.Data.Entity;

    public partial class EFContext : DbContext
    {
        public EFContext(): base("name=EFContext")
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .Property(e => e.Track)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }

        public bool HasChanges
        {
            get { return this.ChangeTracker.HasChanges(); }
        }       
    }
}
