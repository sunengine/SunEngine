import Login from 'auth/Login.vue';
import Register from 'auth/Register.vue';
import RegisterEmailResult from 'auth/RegisterEmailResult.vue';


const routes = [
  {
    name: 'Login',
    path: '/auth/login',
    component: Login,
  },
  {
    name: 'Register',
    path: '/auth/register',
    component: Register,
  },
  {
    name: 'RegisterEmailResult',
    path: '/auth/RegisterEmailResult'.toLowerCase(),
    component: RegisterEmailResult
  }
];

for (let rote of routes) {
  if (!rote.meta) {
    rote.meta = {
      roles: ["Unregistered"]
    };
  }
}

export default routes;

