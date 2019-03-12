import Login from 'account/Login.vue';
import Register from 'account/Register.vue';
import RegisterEmailConfirmed from 'account/RegisterEmailConfirmed.vue';
import ResetPassword from 'account/ResetPassword.vue';
import ResetPasswordSetNew from 'account/ResetPasswordSetNew';
import ResetPasswordFailed from 'account/ResetPasswordFailed.vue';



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
    name: 'ResetPasswordSetNew',
    path: '/account/ResetPasswordSetNew'.toLowerCase(),
    component: ResetPasswordSetNew
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





export default routes;

