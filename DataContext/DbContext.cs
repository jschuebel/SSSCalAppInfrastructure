using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using SSSCalApp.Core.Entity;

namespace SSSCalApp.Infrastructure.DataContext
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            :base(options) { }
        public PersonContext(){ }
        public DbSet<Person> People { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        //public virtual DbQuery<Event> Event { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
     
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<PersonHistory> PersonHistorys { get; set; }


       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");
                entity.Property(e => e.Address1).HasColumnName("Address");
                entity.HasIndex(e => e.Address1)
                    .HasName("IX_AddressesText");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.State).HasMaxLength(5);

                entity.Property(e => e.Zip).HasMaxLength(15);
            });


          modelBuilder.Entity<PersonHistory>(entity =>
            {
                entity.ToTable("viewGeneralHistory");
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
            });

          modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topics");
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TopicTitle)
                    .IsRequired()
                    .HasColumnName("Topic Title")
                    .HasMaxLength(50);
            });

           modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");
            });


           modelBuilder.Entity<Event>(entity =>
            {
                entity.Ignore(t => t.UserName);

                entity.HasIndex(e => new { e.TopicId, e.Date })
                    .HasName("IX_date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RepeatMonthly).HasColumnName("repeatMonthly");

                entity.Property(e => e.RepeatYearly).HasColumnName("repeatYearly");

/*
                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);
*/
                entity.Property(e => e.TopicId).HasColumnName("TopicID");
/*                !!! on linq Include this populates all the events for the topic inside the topic inside the event
                entity.HasOne(d => d.topic)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
*/
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Events_General");

            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Ignore(t => t.DateOfBirth);
   
                entity.ToTable("General");
                entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);


                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("Address ID");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EMail)
                    .HasColumnName("E-Mail")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(15);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("Home Phone")
                    .HasMaxLength(15);

                entity.Property(e => e.Mobile).HasMaxLength(15);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Pager).HasMaxLength(15);

                entity.Property(e => e.Work).HasMaxLength(15);

            });
        }
    }
}