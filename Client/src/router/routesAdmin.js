import Admin from 'admin/Admin.vue';
import AdminPanel from 'admin/AdminPanel.vue';
import CategoriesAdmin from 'admin/CategoriesAdmin.vue';
import AddCategory from 'admin/AddEditCategory/AddCategory.vue';
import EditCategory from 'admin/AddEditCategory/EditCategory.vue';
import RolesAdmin from 'admin/RolesAdmin.vue';
import UsersFromRole from 'admin/UsersFromRole.vue';
import RolesUsers from 'admin/RolesUsers.vue';


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
  },
  {
    name: 'RolesAdmin',
    path: '/admin/RolesAdmin'.toLowerCase(),
    components: {
      default: RolesAdmin,
      navigation: AdminPanel
    }
  },
  {
    name: 'RolesUsers',
    path: '/admin/RolesUsers'.toLowerCase(),
    components: {
      default: RolesUsers,
      navigation: AdminPanel
    },
    children: [
      {
        name: 'UsersFromRole',
        path: ':roleName',
        component: UsersFromRole,
        props: true
      }
    ]
  }
];


for (let rote of routes) {
  if (!rote.meta) {
    rote.meta = {
      roles: ["Admin"]
    };
  }
}


export default routes;

