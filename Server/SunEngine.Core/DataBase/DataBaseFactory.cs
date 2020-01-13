using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Identity;

namespace SunEngine.Core.DataBase
{
	public interface IDataBaseFactory : IConnectionFactory
	{
		DataBaseConnection CreateDb();
	}

	public class DataBaseFactory : IDataBaseFactory
	{
		public static DataBaseFactory DefaultDataBaseFactory;

		private readonly string connectionString;
		private readonly IDataProvider dataProvider;
		private readonly DbMappingSchema mappingSchema;

		public DataBaseFactory(string providerName, string connectionString, DbMappingSchema mappingSchema)
		{
			this.dataProvider = DataConnection.GetDataProvider(providerName, connectionString);
			this.connectionString = connectionString;
			this.mappingSchema = mappingSchema;
		}

		public DataBaseFactory(IDataProvider dataProvider, string connectionString, DbMappingSchema mappingSchema)
		{
			this.dataProvider = dataProvider;
			this.connectionString = connectionString;
			this.mappingSchema = mappingSchema;
		}

		public IDataContext GetContext()
		{
			return new DataContext(dataProvider, connectionString)
			{
				MappingSchema = mappingSchema
			};
		}

		public DataConnection GetConnection()
		{
			return new DataBaseConnection(dataProvider, connectionString, mappingSchema);
		}

		public DataBaseConnection CreateDb()
		{
			return new DataBaseConnection(dataProvider, connectionString, mappingSchema);
		}
	}
}