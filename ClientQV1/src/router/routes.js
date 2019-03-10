import Login from 'account/Login.vue';
import Index from 'pages/Index.vue';
import Register from 'account/Register/Register.vue';
import {makeArticlesSection, makeBlogSection, makeForumSection} from "./makeSections";
import AddEditMaterial from "material/AddEditMaterial";
import SettingsPage from "personal/SettingsPage";
import ChangeName from "personal/ChangeName";
import SettingsPanel from "personal/SettingsPanel";
import Categories1 from 'categories/Categories1';
import Categories2 from 'categories/Categories2';
import ActivitiesPage from 'activities/ActivitiesPage';
import New2Col from 'pages/News2Col';
import AdminPage from 'admin/AdminPage';

const routes = [
  {
    name: "Home",
    path: '/',
    component: Index
  },
  {
    name: "News",
    path: '/News'.toLowerCase(),
    components: {
      default: ActivitiesPage
    }
  },
  {
    name: "News2Col",
    path: '/News2Col'.toLowerCase(),
    components: {
      default: New2Col
    }
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
  {
    name: 'Admin',
    path: '/admin',
    components: {
      default: AdminPage,
      navigation: null,
    }
  },
  ...makeForumSection("Forum", Categories1),
  ...makeForumSection("Forum2L", Categories2),
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
