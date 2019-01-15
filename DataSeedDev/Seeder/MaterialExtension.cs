using SunEngine.Commons.Models;

namespace SunEngine.Seeder
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