import {ArticlesPage, Material, wrapInPage} from 'sun'
import {wrapInPanel} from 'sun'
import {CategoriesAdmin} from 'sun'
import {MenuItemsAdmin} from 'sun'
import {CreateCategory} from 'sun'
import {EditCategory} from 'sun'
import {ImagesCleaner} from 'sun'
import {RolesPermissions} from 'sun'
import {RoleUsers} from 'sun'
import {RolesPage} from 'sun'
import {CacheSettings} from 'sun'
import {CreateMenuItem} from 'sun'
import {EditMenuItem} from 'sun'
import {CypherSecrets} from 'sun'
import {DeletedElements} from 'sun'
import {AdminMenu} from 'sun'


const AdminPage = wrapInPage("AdminPage", AdminMenu, null, "fas fa-cog");
const AdminPanel = wrapInPanel("AdminPage", AdminMenu, null, {name: 'Admin'}, "fas fa-cog");


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
    path: '/admin/MenuItems'.toLowerCase(),
    components: {
      default: MenuItemsAdmin,
      navigation: AdminPanel
    }
  },
  {
    name: 'CreateMenuItem',
    path: '/admin/MenuItems/Create/'.toLowerCase() + ':parentMenuItemId?',
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
    path: '/admin/MenuItems/Edit/'.toLowerCase() + ':menuItemId',
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
    path: '/admin/Categories'.toLowerCase(),
    components: {
      default: CategoriesAdmin,
      navigation: AdminPanel
    }
  },
  {
    name: 'CreateCategory',
    path: '/admin/Categories/Create/'.toLowerCase() + ':parentCategoryId?',
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
    path: '/admin/Categories/Edit/'.toLowerCase() + ':categoryId',
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
    name: 'CypherSecrets',
    path: '/admin/CypherSecrets'.toLowerCase(),
    components: {
      default: CypherSecrets,
      navigation: AdminPanel
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
    name: 'DeletedElements',
    path: '/admin/DeletedElements'.toLowerCase(),
    components: {
      default: DeletedElements,
      navigation: AdminPanel
    }
  },
  {
    name: 'CatView',
    path: '/admin/CatView/'.toLowerCase() + ':categoryName',
    components: {
      default: ArticlesPage,
      navigation: AdminPanel
    },
    props: {
      default: true
    }
  },
  {
    name: 'CatView-mat',
    path: '/admin/CatView-mat/'.toLowerCase() + ':categoryName/:idOrName',
    components: {
      default: Material,
      navigation: AdminPanel
    },
    props: {
      default: true
    }
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

