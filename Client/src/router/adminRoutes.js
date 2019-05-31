import {AdminPage} from 'sun'
import {AdminPanel} from 'sun'
import {CategoriesAdmin} from 'sun'
import {CreateCategory} from 'sun'
import {EditCategory} from 'sun'
import {ImagesCleaner} from 'sun'
import {RolesPermissions} from 'sun'
import {RoleUsers} from 'sun'
import {RolesPage} from 'sun'
import {CacheSettings} from 'sun'


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
    name: 'ImagesCleaner',
    path: '/admin/ImagesCleaner'.toLowerCase(),
    components: {
      default: ImagesCleaner,
      navigation: AdminPanel
    }
  },
  {
    name: 'RolesPermissions',
    path: '/admin/RolesPermissions'.toLowerCase(),
    components: {
      default: RolesPermissions,
      navigation: null
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

