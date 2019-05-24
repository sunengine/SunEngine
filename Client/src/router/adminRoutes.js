import {AdminPage} from 'sun'
import {AdminPanel} from 'sun'
import {CategoriesAdmin} from 'sun'
import {MenuItemsAdmin} from 'sun'
import {CreateCategory} from 'sun'
import {EditCategory} from 'sun'
import {RolesPermissions} from 'sun'
import {RoleUsers} from 'sun'
import {RolesPage} from 'sun'
import {CacheSettings} from 'sun'
import {CreateMenuItem} from 'sun'
import {EditMenuItem} from 'sun'


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
    name: 'MenuItemsAdmin',
    path: '/admin/MenuItemsAdmin'.toLowerCase(),
    components: {
      default: MenuItemsAdmin,
      navigation: AdminPanel
    }
  },
  {
    name: 'CreateMenuItem',
    path: '/admin/CreateMenuItem/'.toLowerCase() + ':parentMenuItemId?',
    components: {
      default: CreateMenuItem,
      navigation: AdminPanel
    },
    props: {
      default: true,
      navigation: null
    }
  },
  {
    name: 'EditMenuItem',
    path: '/admin/EditMenuItem/'.toLowerCase() + ':menuItemId',
    components: {
      default: EditMenuItem,
      navigation: AdminPanel
    },
    props: {
      default: true,
      navigation: null
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
    name: 'CacheSettings',
    path: '/admin/CacheSettings'.toLowerCase(),
    components: {
      default: CacheSettings,
      navigation: AdminPanel
    }
  },
  {
    name: 'CreateCategory',
    path: '/admin/CreateCategory/'.toLowerCase() + ':parentCategoryId?',
    components: {
      default: CreateCategory,
      navigation: AdminPanel
    },
    props: {
      default: true,
      navigation: null
    }
  },
  {
    name: 'EditCategory',
    path: '/admin/EditCategory/'.toLowerCase() + ':categoryId',
    components: {
      default: EditCategory,
      navigation: AdminPanel
    },
    props: {
      default: true,
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

