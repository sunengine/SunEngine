import {BlogMultiCatPage} from 'sun'
import {ActivitiesPage} from 'sun'

import IndexPage from './IndexPage'
import News2ColPage from './News2ColPage'
import MaterialInlinePage from './MaterialInlinePage'
import SecretPage from './SecretPage'

import coreRoutes from './coreRoutes'


const siteRoutes = [
  {
    name: 'Home',
    path: '/',
    component: IndexPage
  },
  {
    name: 'News',
    path: '/News'.toLowerCase(),
    components: {
      default: ActivitiesPage,
    },
    props: {
      default: {
        pageTitle: 'Активность на сайте'
      }
    }
  },
  {
    name: 'News2ColPage',
    path: '/News2ColPage'.toLowerCase(),
    components: {
      default: News2ColPage,
    }
  },
  {
    name: 'MaterialInlinePage',
    path: '/MaterialInlinePage'.toLowerCase(),
    components: {
      default: MaterialInlinePage,
    }
  },
  {
    name: 'BlogMulti',
    path: '/BlogMulti'.toLowerCase(),
    components: {
      default: BlogMultiCatPage,
    },
    props: {
      default: {
        pageTitle: 'Новые материалы',
        categoriesNames: 'Forum1,Articles,Blog'.toLowerCase(),
        addButtonLabel: 'Добавить материал',
        caption: 'Посты в виде блога из категорий: Forum, Articles, Blog',
        rolesCanAdd: ['Admin', 'Moderator']
      }
    }
  },
  {
    name: 'Secret',
    path: '/secret',
    components: {
      default: SecretPage,
      navigation: null
    },
    meta: {
      roles: ["Registered"]
    }
  },
];


export default [...coreRoutes, ...siteRoutes]

