using Ditransa.Domain.Common;
using Ditransa.Domain.Common.Interfaces;
using Ditransa.Domain.Entities.WTSA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Ditransa.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<User> User => Set<User>();
        public DbSet<MenuRole> MenuRole => Set<MenuRole>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<Inspection> Inspection => Set<Inspection>();
        public DbSet<Evidence> Evidence => Set<Evidence>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users", "dbo");
            modelBuilder.Entity<MenuRole>().ToTable("MenuRoles", "dbo");
            modelBuilder.Entity<Role>().ToTable("Roles", "dbo");
            modelBuilder.Entity<Inspection>().ToTable("Inspection", "dbo");
            modelBuilder.Entity<Evidence>().ToTable("Evidences", "dbo");

            // Configuración de llaves primarias compuestas

            //modelBuilder.Entity<UsuarioClientes>()
            //    .HasOne(u => u.cliente) 
            //    .WithMany()
            //    .HasForeignKey(u => u.EmpresaNit) 
            //    .HasPrincipalKey(c => c.nit);

            modelBuilder.Entity<MenuRole>().Ignore("UserId");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}