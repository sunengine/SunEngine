const config = {

  API: 'http://localhost:5000',
  SiteUrl: "http://localhost:5005",
  UploadedImages: 'http://localhost:5000/UploadImages',
  SiteName: 'SunEngine',

  Messages: {
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
    Users_UserName: 64,
    Users_Email: 64,
    Users_Link: 32,
    Users_PasswordMinLength: 6,
    Materials_Name: 32,
    Materials_Title: 256,
    Materials_Description: 256,
    Tags_Name: 64,
    Roles_Name: 64,
    Roles_Title: 64,
    OperationKey_Name: 100
  },
  PasswordValidation: {
    MinLength: 6,
    MinDifferentChars: 2
  }
};
