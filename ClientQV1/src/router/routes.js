import Login from 'account/Login.vue';
import Index from 'pages/Index.vue';
import Register from 'account/Register/Register.vue';
import {makeArticlesSection, makeBlogSection, makeForumSection} from "./makeSections";
import AddEditMaterial from "material/AddEditMaterial";
import SettingsPage from "personal/SettingsPage";
import ChangeName from "personal/ChangeName";
import ChangeLink from "personal/ChangeLink";
import SettingsPanel from "personal/SettingsPanel";
import Categories1 from 'categories/Categories1';
import Categories2 from 'categories/Categories2';
import ActivitiesPage from 'activities/ActivitiesPage';
import New2Col from 'pages/News2Col';
import AdminPage from 'admin/AdminPage';
import CategoriesAdminPage from 'admin/CategoriesAdminPage';
import AdminPanel from 'admin/AdminPanel';
import AddCategory from 'admin/AddEditCategory/AddCategory';
import EditCategory from 'admin/AddEditCategory/EditCategory';
import Profile from 'profile/Profile';
import {store} from "../store";



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
    name: 'User',
    path: '/user/:link',
    components: {
      default: Profile,
      navigation: null
    },
    props: {
      default: true
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
    name: 'ProfileInSettings',
    path: '/personal/Profile'.toLowerCase(),
    components: {
      default: Profile,
      navigation: SettingsPanel
    },
    props: {
      default: () => {return { link: store.state.auth.userInfo?.link }}
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
    name: 'ChangeLink',
    path: '/personal/ChangeLink'.toLowerCase(),
    components: {
      default: ChangeLink,
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
  {
    name: 'CategoriesAdmin',
    path: '/admin/CategoriesAdmin'.toLowerCase(),
    components: {
      default: CategoriesAdminPage,
      navigation: AdminPanel
    }
  },
  {
    name: 'AddCategory',
    path: '/admin/AddCategory'.toLowerCase(),
    components: {
      default: AddCategory,
      navigation: AdminPanel
    }
  },
  {
    name: 'EditCategory',
    path: '/admin/EditCategory/:id'.toLowerCase(),
    components: {
      default: EditCategory,
      navigation: AdminPanel
    },
    props: {
      default: (route) => {
        return {categoryId: +route.params.id};
      },
      navigation: null
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
