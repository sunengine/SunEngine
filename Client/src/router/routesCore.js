import AddEditMaterial from 'material/AddEditMaterial.vue';
import Login from 'auth/Login.vue';
import Register from 'auth/Register/Register.vue';
import EmailConfirmed from 'auth/Register/EmailConfirmed.vue';
import ResetPassword from 'auth/Password/ResetPassword.vue';
import SetNewPasswordFromReset from 'auth/Password/SetNewPasswordFromReset.vue';
import ResetPasswordFailed from 'auth/Password/ResetPasswordFailed.vue';

import UserProfile from 'profile/Profile';
import WritePrivateMessage from 'profile/WritePrivateMessage';


const routes = [
  {
    name: 'Login',
    path: '/auth/login',
    component: Login,
    meta: {
      roles: ["Unregistered"]
    }
  },
  {
    name: 'Register',
    path: '/auth/register',
    component: Register,
    meta: {
      roles: ["Unregistered"]
    }
  },
  {
    name: 'EmailConfirmed',
    path: '/auth/EmailConfirmed'.toLowerCase(),
    component: EmailConfirmed,
    meta: {
      notReturnable: true
    }
  },
  {
    name: 'ResetPassword',
    path: '/auth/ResetPassword'.toLowerCase(),
    component: ResetPassword
  },
  {
    name: 'SetNewPasswordFromReset',
    path: '/auth/SetNewPasswordFromReset'.toLowerCase(),
    component: SetNewPasswordFromReset
  },
  {
    name: 'ResetPasswordFailed',
    path: '/auth/ResetPasswordFailed'.toLowerCase(),
    component: ResetPasswordFailed,
    meta: {
      notReturnable: true
    }
  },
  {
    name: 'User',
    path: '/user/:link',
    components: {
      default: UserProfile,
      navigation: null
    },
    props: {
      default: true
    }
  },
  {
    name: 'WritePrivateMessage',
    path: '/WritePrivateMessage'.toLowerCase(),
    components: {
      default: WritePrivateMessage,
      navigation: null
    },
    props: {
      default: (route) => {
        return {
          userId: route.query.userId,
          userName: route.query.userName
        }
      }
    },
    meta: {
      roles: ["Registered"]
    }
  },
  {
    name: "AddEditMaterial",
    path: '/AddEditMaterial'.toLowerCase(),
    components: {
      default: AddEditMaterial,
      navigation: null
    },
    props: {
      default: (route) => {
        return {
          categoryName: route.query.categoryName,
          id: +route.query.id
        }
      },
      navigation: null
    }
  },

]


// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}


export default routes;

