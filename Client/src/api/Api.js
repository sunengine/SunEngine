export default {
  Blog: {
    GetPosts: '/Blog/GetPosts',
    GetPostsFromMultiCategories: "/Blog/GetPostsFromMultiCategories"
  },
  Profile: {
    BanUser: '/Profile/BanUser',
    UnBanUser: '/Profile/UnBanUser',
    GetProfile: '/Profile/GetProfile',
    SendPrivateMessage: '/Profile/SendPrivateMessage'
  },
  Personal: {
    SetMyLink: 'Personal/SetMyLink',
    SetMyName: 'Personal/SetMyName',
    SetMyProfileInformation: 'Personal/SetMyProfileInformation',
    GetMyProfileInformation: 'Personal/GetMyProfileInformation',
    RemoveMyAvatar: 'Personal/RemoveMyAvatar',
    GetMyBanList: 'Personal/GetMyBanList',
    GetMySessions: 'Personal/GetMySessions',
    RemoveMySessions: 'Personal/RemoveMySessions'
  },
  UploadImages: {
    UploadUserPhoto: 'UploadImages/UploadUserPhoto',
  },
  Materials: {
    Get: 'Get',
    Update: 'Update',
    Create: 'Create',
    Restore: 'Restore',
    Delete: 'Delete'
  },
  Comments: {
    GetMaterialComments: 'GetMaterialComments',
    MoveToTrash: 'MoveToTrash',
    Update: 'Update',
    Get: 'Get'
  },
  Forum: {
    GetThread: 'GetThread',
    GetNewTopics: 'GetNewTopics'
  },
  Auth: {
    Register: 'Register',
    CheckUserNameInDb: 'CheckUserNameInDb'
  },
  Captcha: {
    GetCaptchaKey: 'GetCaptchaKey'
  }
}
