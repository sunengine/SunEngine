const config = {

  API: 'http://localhost:5000',
  SiteUrl: "http://localhost:5005",
  UploadedImages: 'http://localhost:5000/UploadImages',
  SiteName: 'SunEngine Demo',

  OpenExternalLinksAtNewTab: true,

  VueDevTools: true,      // Do not use on production
  VueAppInWindow: true,   // Do not use on production

  Log: {
    InitExtended: true,
    Requests: true,
    MoveTo: true,
  },

  Comments: {
    "TimeToOwnEditInMinutes": 15,
    "TimeToOwnDeleteInMinutes": 15
  },
  Materials: {
    "TimeToOwnEditInMinutes": 15,
    "TimeToOwnDeleteInMinutes": 15,
    "TimeToOwnMoveInMinutes": 15,
  },
  DbColumnSizes: {
    Categories_Name: 64,
    Categories_Title: 256,
    Categories_Icon: 64,
    Users_UserName: 64,
    Users_Email: 64,
    Users_Link: 32,
    Users_PasswordMinLength: 6,
    Materials_Name: 32,
    Materials_Title: 256,
    Materials_SubTitle: 256,
    Tags_Name: 64,
    Roles_Name: 64,
    Roles_Title: 64,
    OperationKey_Name: 100,
    MenuItems_Name: 32,
    MenuItems_Title: 64,
    MenuItems_SubTitle: 64,
    MenuItems_RouteName: 64,
    MenuItems_CssClass: 64,
    MenuItems_Icon: 64
  },
  PasswordValidation: {
    MinLength: 6,
    MinDifferentChars: 2
  },
  Misc: {
    AdminRoleUsersMaxUsersTake: 40,
    DefaultAvatar: "default-avatar.svg"
  }
};


if(config.SiteUrl.startsWith("http://"))
  config.SiteSchema = "http://";
else if(config.SiteUrl.startsWith("https://"))
  config.SiteSchema = "https://";
else
  throw "SiteUrl in config.js have to start with 'http://' or 'https://'.";

