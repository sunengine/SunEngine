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

