import AdminMenu from 'admin/AdminMenu.vue';
import CategoriesAdmin from 'admin/CategoriesAdmin.vue';
import AddCategory from 'admin/AddEditCategory/AddCategory.vue';
import EditCategory from 'admin/AddEditCategory/EditCategory.vue';

import {store} from 'store';

const routes = [
  {
    name: 'AdminPanel',
    path: '/admin',
    components: {
      default: null,
      navigation: AdminMenu
    }
  },
  {
    name: 'CategoriesAdmin',
    path: '/admin/CategoriesAdmin'.toLowerCase(),
    components: {
      default: CategoriesAdmin,
      navigation: AdminMenu
    }
  },
  {
    name: 'AddCategory',
    path: '/admin/AddCategory'.toLowerCase(),
    components: {
      default: AddCategory,
      navigation: AdminMenu
    }
  },
  {
    name: 'EditCategory',
    path: '/admin/EditCategory'.toLowerCase(),
    components: {
      default: EditCategory,
      navigation: AdminMenu
    }
  }
]


for(let rote of routes) {
  if(!rote.meta) {
    rote.meta = {
      roles: ["Admin"]
    };
  }
}


export default routes;

