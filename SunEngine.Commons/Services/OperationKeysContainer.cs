using System.Collections.Generic;
using System.Linq;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Services
{
    /// <summary>
    /// This is registered Singleton Service. This container made for Main module. Others modules
    /// must have different name like: ModuleNameOperationKeysContainer
    /// </summary>
    public class OperationKeysContainer
    {
        public OperationKeysContainer(IDataBaseFactory dbFactory)
        {
            using (DataBaseConnection db = dbFactory.CreateDb())
            {
                Dictionary<string, int> dictionary = db.OperationKeys
                    .ToDictionary(x => x.Name, x => x.OperationKeyId);

                foreach (var propertyInfo in typeof(OperationKeysContainer).GetProperties())
                {
                    propertyInfo.SetValue(this, dictionary[propertyInfo.Name]);
                }
            }
        }
        
        
        public int MaterialAndMessagesRead {get; private set;}
        public int MaterialWrite {get; private set;}
        public int MaterialEditOwn {get; private set;}
        public int MaterialEditOwnIfTimeNotExceeded {get; private set;}
        public int MaterialEditOwnIfHasReplies {get; private set;}
        public int MaterialDeleteOwn {get; private set;}
        public int MaterialDeleteOwnIfTimeNotExceeded {get; private set;}
        public int MaterialDeleteOwnIfHasReplies {get; private set;}

        
        public int MessageWrite {get; private set;}
        public int MessageEditOwn {get; private set;}
        public int MessageEditOwnIfTimeNotExceeded {get; private set;}
        public int MessageEditOwnIfHasReplies {get; private set;}
        public int MessageDeleteOwn {get; private set;}
        public int MessageDeleteOwnIfTimeNotExceeded {get; private set;}
        public int MessageDeleteOwnIfHasReplies {get; private set;}
        
      
        // moderator

        public int MaterialEditAny {get; private set;}
        public int MaterialDeleteAny {get; private set;}
        //public int MaterialMoveAny {get; private set;}

        public int MessageEditAny {get; private set;}
        public int MessageDeleteAny {get; private set;}
        
        
        public int MessageMoveAny {get; private set;}
        

        // global TODO вынести в отдельный класс глобальных ключей

//        public int UseFileManager {get; private set;}  
//        public int BanUsers {get; private set;}
//        public int RestoreMaterials {get; private set;} // TODO add restore own messages and materials rights
//        public int RestoreMessages {get; private set;}
//        public int AdminOperations {get; private set;}

        /*public int MaterialRestoreOwn {get; private set;}
              public int MaterialRestoreOwnIfTimeNotExceeded {get; private set;}
               public int MessageRestoreOwn {get; private set;} // TODO можно удалять если удалено не админом
              public int MessageRestoreOwnIfTimeNotExceeded {get; private set;}*/
        
        
        
        
        public static IList<string> GetAllOperationKeys()
        {
            var allKeys = typeof(OperationKeysContainer).GetProperties();
            return allKeys.Select(propertyInfo => propertyInfo.Name).ToList();
        }
    }
}