import AdminPage from 'admin/AdminPage';
import AdminPanel from 'admin/AdminPanel.vue';
import CategoriesAdmin from 'admin/CategoriesAdmin.vue';
import AddCategory from 'admin/AddEditCategory/AddCategory.vue';
import EditCategory from 'admin/AddEditCategory/EditCategory.vue';
import RolesPermissions from 'admin/RolesPermissions.vue';
// import UsersFromRole from 'admin/UsersFromRole.vue';
// import RolesUsers from 'admin/RolesUsers.vue';


import {store} from 'store';

const routes = [
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
    name: 'RolesPermissions',
    path: '/admin/RolesPermissions'.toLowerCase(),
    components: {
      default: RolesPermissions,
      navigation: AdminPanel
    }
  },
 /*  {
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
  }*/
];


for (let rote of routes) {
  if (!rote.meta) {
    rote.meta = {
      roles: ["Admin"]
    };
  }
}


export default routes;

