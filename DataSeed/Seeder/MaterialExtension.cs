using SunEngine.Models.Materials;

namespace DataSeed.Seeder
{
    public static class MaterialExtension
    {
        public static void SetLastMessage(this Material material,Message message)
        {
            if (message != null)
            {
                material.LastMessageId = message.Id;
                material.LastActivity = message.PublishDate;
            }
        }
    }
}