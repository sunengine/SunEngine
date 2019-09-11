export default {
  ImagesCleaner: {
    GetAllImages: '/Admin/ImagesCleaner/GetAllImages',
    DeleteImages: '/Admin/ImagesCleaner/DeleteImages',
  },
  CypherSecretsAdmin:{
    ResetCypher: '/Admin/CypherSecretsAdmin/ResetCypher',
  },
  AdminCacheSettings: {
    GetCurrentCacheSettings: '/Admin/AdminCacheSettings/GetCurrentCacheSettings',
    ChangeCachePolicy: '/Admin/AdminCacheSettings/ChangeCachePolicy',
  },
  UserRolesAdmin: {
    GetRoleUsers: '/Admin/UserRolesAdmin/GetRoleUsers',
    RemoveUserFromRole:   '/Admin/UserRolesAdmin/RemoveUserFromRole',
    AddUserToRole:   '/Admin/UserRolesAdmin/AddUserToRole',
    GetAllRoles: '/Admin/UserRolesAdmin/GetAllRoles'
  },
  RolesPermissionsAdmin: {
    GetJson: '/Admin/RolesPermissionsAdmin/GetJson',
    UploadJson: '/Admin/RolesPermissionsAdmin/UploadJson',
  },
}
