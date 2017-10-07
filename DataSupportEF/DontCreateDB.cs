using System.Data.Entity;

namespace DataSupportEF
{
	class DontCreateDB<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
	{
		public void InitializeDatabase(TContext context)
		{
			// не проверяем модель на изменения и не пересоздаем таблицы в БД
		}
	}
}
