const config = {

  API: 'http://localhost:5000',
  SiteUrl: "http://localhost:5005",
  UploadedImages: 'http://localhost:5000/UploadImages',
  Skin: 'http://localhost:5000/CurrentSkin/styles.css',
  //Skin: 'http://localhost:5005/statics/CurrentSkin/styles.css', // use to skin dev and place skin in statics/CurrentSkin

  OpenExternalLinksAtNewTab: true,

  VueDevTools: true,        // Do not use on production
  VueAppInWindow: true,     // Do not use on production

  Log: {
    InitExtended: true,     // Do not use on production
    Requests: true,         // Do not use on production
    MoveTo: true,           // Do not use on production
  },





  Global: {
    SiteName: 'SunEngine Demo',
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
  PasswordValidation: {
    MinLength: 6,
    MinDifferentChars: 2
  },
  // auto-end

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
    MenuItems_Icon: 64,
    Components_Name: 32,
    Components_Type: 32
  },
  Misc: {
    AdminRoleUsersMaxUsersTake: 40,
    DefaultAvatar: "default-avatar.svg"
  },
};
