using System.Collections.Concurrent;
using System.Linq;
using LinqToDB;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Services
{
    public interface IVisitsCounterService
    {
        int CountMaterial(int materialId);
        int CountMaterial(string materialName);
        void UploadToDataBase();
    }

    public class VisitsCounterService : IVisitsCounterService
    {
        protected readonly object lockObject = new object();

        protected readonly ConcurrentDictionary<int, int> visitsByIds = new ConcurrentDictionary<int, int>();
        protected readonly ConcurrentDictionary<string, int> visitsByNames = new ConcurrentDictionary<string, int>();

        protected readonly IDataBaseFactory dbFactory;

        public VisitsCounterService(IDataBaseFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }


        public int CountMaterial(int materialId)
        {
            lock (lockObject)
            {
                if (visitsByIds.TryGetValue(materialId, out int materialVisits))
                    return visitsByIds[materialId] = materialVisits + 1;

                return visitsByIds[materialId] = 1;
            }
        }

        public int CountMaterial(string materialName)
        {
            lock (lockObject)
            {
                if (visitsByNames.TryGetValue(materialName, out int materialVisits))
                    return visitsByNames[materialName] = materialVisits + 1;

                return visitsByNames[materialName] = 1;
            }
        }

        

        public void UploadToDataBase()
        {
            UploadIdVisitsToDataBase();
            UploadNameVisitsToDataBase();
        }

        protected void UploadIdVisitsToDataBase()
        {
            if (visitsByIds.Count == 0) 
                return;
            
            lock (lockObject)
                using (var db = dbFactory.CreateDb())
                using (TempTable<VisitsById> visitsByIdTempTable = new TempTable<VisitsById>(db,
                    visitsByIds.Select(x => new VisitsById {Id = x.Key, Visits = x.Value}).ToArray()))
                {
                    db.BeginTransaction();

                    db.Materials.Where(x => visitsByIdTempTable.Any(y => y.Id == x.Id))
                        .Set(x => x.VisitsCount,
                            x => x.VisitsCount + visitsByIdTempTable.FirstOrDefault(y => y.Id == x.Id).Visits)
                        .Update();

                    visitsByIds.Clear();
                    db.CommitTransaction();
                }
        }

        protected void UploadNameVisitsToDataBase()
        {
            if (visitsByNames.Count == 0) 
                return;
            
            lock (lockObject)
                using (var db = dbFactory.CreateDb())
                using (TempTable<VisitsByName> visitsByNameTempTable = new TempTable<VisitsByName>(db,
                    visitsByNames.Select(x => new VisitsByName {Name = x.Key, Visits = x.Value}).ToArray()))
                {
                    db.BeginTransaction();

                    db.Materials.Where(x => visitsByNameTempTable.Any(y => y.Name == x.Name))
                        .Set(x => x.VisitsCount,
                            x => x.VisitsCount + visitsByNameTempTable.FirstOrDefault(y => y.Name == x.Name).Visits)
                        .Update();

                    visitsByNames.Clear();
                    db.CommitTransaction();
                }
        }
        
        protected class VisitsById
        {
            public int Id { get; set; }
            public int Visits { get; set; }
        }

        protected class VisitsByName
        {
            public string Name { get; set; }
            public int Visits { get; set; }
        }
    }
}
