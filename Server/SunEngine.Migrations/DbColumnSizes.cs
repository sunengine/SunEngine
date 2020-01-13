using System;

namespace SunEngine.Migrations
{
	internal static class DbColumnSizes
	{
		public const int Categories_Name = 64;
		public const int Categories_Token = 64;
		public const int Categories_CustomUrl = Int32.MaxValue;
		public const int Categories_Title = 256;
		public const int Categories_SubTitle = Int32.MaxValue;
		public const int Categories_LayoutName = 32;
		public const int Categories_MaterialTypeTitle = 32;
		public const int Categories_Icon = 64;
		public const int Users_UserName = 64;
		public const int Users_Email = 64;
		public const int Users_Link = 64;
		public const int FileNameWithDirSize = 28;
		public const int Materials_Name = 64;
		public const int Materials_Title = 256;
		public const int Materials_SubTitle = Int32.MaxValue;
		public const int Tags_Name = 64;
		public const int Roles_Name = 64;
		public const int Roles_Title = 64;
		public const int OperationKey_Name = 100;
		public const int LongSessions_LongToken1 = 16;
		public const int LongSessions_LongToken2 = 16;
		public const int BlackListShortToken_TokenId = 16;
		public const int MenuItems_Name = 32;
		public const int MenuItems_Title = 256;
		public const int MenuItems_SubTitle = Int32.MaxValue;
		public const int MenuItems_RouteName = 64;
		public const int MenuItems_CssClass = 64;
		public const int MenuItems_Icon = 64;
		public const int CipherSecrets_Name = 32;
		public const int CipherSecrets_Secret = 32;
		public const int Components_Name = 64;
		public const int Components_Type = 32;
	}
}