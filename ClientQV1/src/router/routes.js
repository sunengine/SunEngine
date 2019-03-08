import Login from 'account/Login.vue';
import Index from 'pages/Index.vue';
import Register from 'account/Register/Register.vue';
import {makeArticlesSection, makeBlogSection} from "./makeSections";
import AddEditMaterial from "material/AddEditMaterial";
import SettingsPage from "personal/SettingsPage";
import ChangeName from "personal/ChangeName";
import SettingsPanel from "personal/SettingsPanel";

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
  {
    name: 'Personal',
    path: '/personal',
    components: {
      default: SettingsPage,
      navigation: null
    }
  },
  {
    name: 'ChangeName',
    path: '/personal/ChangeName'.toLowerCase(),
    components: {
      default: ChangeName,
      navigation: SettingsPanel
    }
  },
  ...makeArticlesSection("Articles"),
  ...makeBlogSection("Blog"),
];

// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}

export default routes
