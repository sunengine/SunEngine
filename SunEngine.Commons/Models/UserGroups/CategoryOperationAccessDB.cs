namespace SunEngine.Commons.Models.UserGroups
{
    public class CategoryOperationAccessDB
    {
        public int CategoryAccessId { get; set; }
        public CategoryAccessDB CategoryAccessDb { get; set; }
        
        public int OperationKeyId { get; set; }
        public OperationKeyDB OperationKeyDb { get; set; }

        public bool Access { get; set; }
    }
}