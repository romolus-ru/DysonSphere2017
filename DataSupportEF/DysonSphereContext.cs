using DataSupport.Data;
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

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AtlasFiles>().HasKey(u => u.IdAtlasFile);
		}

	}
}
