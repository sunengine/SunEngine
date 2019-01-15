import AdminMenu from 'admin/AdminMenu.vue';
import CategoriesAdmin from 'admin/CategoriesAdmin.vue';

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

