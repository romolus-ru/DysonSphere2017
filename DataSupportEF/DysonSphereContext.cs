using Engine.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupportEF
{
	public partial class DysonSphereContext : DbContext
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>
		/// name=ConnectionName позволит выявить что connectionstring из другого проекта не видна
		/// entity framework connection string in code
		/// c# No connection string named could be found in the application config file.
		/// </remarks>
		public DysonSphereContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=e:\FTP\Programs\C#Projects02\DysonSphere3\_files\DS3_Session001Begin.mdf;Integrated Security=True")
		{
			// блокируем изменения таблиц в базе
			Database.SetInitializer(new DontCreateDB<DysonSphereContext>());
		}

		// Отражение таблиц базы данных на свойства с типом DbSet
		public DbSet<AtlasFiles> AtlasFiles { get; set; }
		public DbSet<AtlasTextures> AtlasTextures { get; set; }
		public DbSet<CollectClass> CollectClasses { get; set; }
		public DbSet<_Settings> Settings { get; set; }
		public DbSet<UserRegistration> UserRegistration { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AtlasFiles>().HasKey(u => u.IdAtlasFile);
			modelBuilder.Entity<AtlasTextures>().HasKey(u => u.IdAtlasLink);
			modelBuilder.Entity<CollectClass>()
				.ToTable("_FilesClasses")
				.HasKey(u => u.Id)
				.Property(u => u.Id).HasColumnName("IdFilesClasses");
			modelBuilder.Entity<_Settings>().HasKey(u => u.IdSettings);
			modelBuilder.Entity<UserRegistration>().HasKey(u => u.UserId);
		}

	}
}
