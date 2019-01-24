import Admin from 'admin/Admin.vue';
import AdminPanel from 'admin/AdminPanel.vue';
import CategoriesAdmin from 'admin/CategoriesAdmin.vue';
import AddCategory from 'admin/AddEditCategory/AddCategory.vue';
import EditCategory from 'admin/AddEditCategory/EditCategory.vue';

import {store} from 'store';

const routes = [
  {
    name: 'Admin',
    path: '/admin',
    components: {
      default: Admin,
      navigation: null,
    }
  },
  {
    name: 'CategoriesAdmin',
    path: '/admin/CategoriesAdmin'.toLowerCase(),
    components: {
      default: CategoriesAdmin,
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
  }
]


for (let rote of routes) {
  if (!rote.meta) {
    rote.meta = {
      roles: ["Admin"]
    };
  }
}


export default routes;

