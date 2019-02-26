import Login from 'account/Login.vue';
import Index from 'pages/Index.vue';
import Register from 'account/Register/Register.vue';


const routes = [
  {
    name: "Home",
    path: '/',
    component: Index
  },
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
];

// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}

export default routes
