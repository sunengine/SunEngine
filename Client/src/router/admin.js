import AdminPage from 'admin/AdminPage';
import AdminPanel from 'admin/AdminPanel';
import CategoriesAdmin from 'admin/CategoriesAdmin';
import CreateCategory from 'admin/CreateEditCategory/CreateCategory';
import EditCategory from 'admin/CreateEditCategory/EditCategory';
import RolesPermissions from 'admin/RolesPermissions';
import RoleUsers from 'admin/RoleUsers';
import RolesPage from 'admin/RolesPage';


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
    name: 'CreateCategory',
    path: '/admin/CreateCategory'.toLowerCase(),
    components: {
      default: CreateCategory,
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
  {
    name: 'RolesPage',
    path: '/admin/RolesPage'.toLowerCase(),
    components: {
      default: RolesPage,
      navigation: AdminPanel
    },
    children: [
      {
        name: 'RoleUsers',
        path: ':roleName',
        component: RoleUsers,
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

