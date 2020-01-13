using System;
using System.Collections.Concurrent;
using System.Linq;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Filters;

namespace SunEngine.Core.Cache.Services.Counters
{
	public interface IMaterialsVisitsCounterCache
	{
		/// <summary>
		/// Add 1 and return cached value.
		/// </summary>
		int CountMaterial(string userOrIpKey, int materialId);

		/// <summary>
		/// Upload cached values to data base
		/// </summary>
		void UploadToDataBase();
	}

	public class MaterialsVisitsCounterCache : IMaterialsVisitsCounterCache
	{
		protected const string SpamProtectionKeyStart = "VISM";

		protected readonly TimeSpan SpamProtectionTimeInterval = TimeSpan.FromMinutes(20);

		protected readonly object lockObject = new object();

		/// <summary>
		/// Dictionary of cached visits contains only new visits.
		/// To get full count you need to sum base value from database and this value.
		/// </summary>
		protected readonly ConcurrentDictionary<int, int> visits = new ConcurrentDictionary<int, int>();

		protected readonly IDataBaseFactory dbFactory;
		protected readonly SpamProtectionCache spamProtectionCache;

		public MaterialsVisitsCounterCache(
			IDataBaseFactory dbFactory,
			SpamProtectionCache spamProtectionCache)
		{
			this.dbFactory = dbFactory;
			this.spamProtectionCache = spamProtectionCache;
		}


		protected static string GenerateKey(string userOrIpKey, int materialId) =>
			SpamProtectionKeyStart + "-" + userOrIpKey + "-" + materialId;

		public int CountMaterial(string userOrIpKey, int materialId)
		{
			var key = GenerateKey(userOrIpKey, materialId);

			lock (lockObject)
			{
				if (spamProtectionCache.HasWorkingKey(key))
				{
					return visits.TryGetValue(materialId, out int rez)
						? rez
						: 0;
				}

				spamProtectionCache.AddOrUpdate(key, new RequestFree(SpamProtectionTimeInterval));


				if (visits.TryGetValue(materialId, out int materialVisits))
					return visits[materialId] = materialVisits + 1;

				return visits[materialId] = 1;
			}
		}

		public void UploadToDataBase()
		{
			UploadIdVisitsToDataBase();
		}

		protected void UploadIdVisitsToDataBase()
		{
			lock (lockObject)
			{
				if (visits.Count == 0)
					return;

				using var db = dbFactory.CreateDb();

				var vss = visits.Select(x => new VisitsById {Id = x.Key, Visits = x.Value});

				db.BeginTransaction();

				db.DropTable<VisitsById>(throwExceptionIfNotExists: false);
				using (TempTable<VisitsById> visitsByIdTempTable = new TempTable<VisitsById>(db, vss))
				{
					db.Materials.Where(x => visitsByIdTempTable.Any(y => y.Id == x.Id))
						.Set(x => x.VisitsCount,
							x => x.VisitsCount + visitsByIdTempTable.FirstOrDefault(y => y.Id == x.Id).Visits)
						.Update();

					visits.Clear();
				}

				db.CommitTransaction();
			}
		}

		protected class VisitsById
		{
			public int Id { get; set; }
			public int Visits { get; set; }
		}
	}
}