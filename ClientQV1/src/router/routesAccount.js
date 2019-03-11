import AddEditMaterial from 'material/AddEditMaterial.vue';
import Login from 'account/Login.vue';
import Register from 'account/Register.vue';
import RegisterEmailConfirmed from 'account/Register/EmailConfirmed.vue';
import ResetPassword from 'account/Password/ResetPassword.vue';
import SetNewPasswordFromReset from 'account/Password/SetNewPasswordFromReset.vue';
import ResetPasswordFailed from 'account/Password/ResetPasswordFailed.vue';

import UserProfile from 'profile/Profile';
import WritePrivateMessage from 'profile/WritePrivateMessage';


const routes = [
  {
    name: 'Login',
    path: '/account/login',
    component: Login,
    meta: {
      roles: ["Unregistered"]
    }
  },
  {
    name: 'Register',
    path: '/account/register',
    component: Register,
    meta: {
      roles: ["Unregistered"]
    }
  },
  {
    name: 'RegisterEmailConfirmed',
    path: '/account/RegisterEmailConfirmed'.toLowerCase(),
    component: RegisterEmailConfirmed,
    meta: {
      notReturnable: true
    }
  },
  {
    name: 'ResetPassword',
    path: '/account/ResetPassword'.toLowerCase(),
    component: ResetPassword
  },
  {
    name: 'SetNewPasswordFromReset',
    path: '/account/SetNewPasswordFromReset'.toLowerCase(),
    component: SetNewPasswordFromReset
  },
  {
    name: 'ResetPasswordFailed',
    path: '/account/ResetPasswordFailed'.toLowerCase(),
    component: ResetPasswordFailed,
    meta: {
      notReturnable: true
    }
  },


];


// TODO
// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}


export default routes;

