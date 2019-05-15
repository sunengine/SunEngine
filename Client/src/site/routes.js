import {BlogMultiCatPage} from 'sun'
import {ActivitiesPage} from 'sun'

import IndexPage from './IndexPage'
import News2ColPage from './News2ColPage'
import MaterialInlinePage from './MaterialInlinePage'

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
      navigation: null
    },
    props: {
      default: {
        pageTitle: "Активность на сайте"
      }
    }
  },
  {
    name: 'News2ColPage',
    path: '/News2ColPage'.toLowerCase(),
    components: {
      default: News2ColPage,
      navigation: null
    }
  },
  {
    name: 'MaterialInlinePage',
    path: '/MaterialInlinePage'.toLowerCase(),
    components: {
      default: MaterialInlinePage,
      navigation: null
    }
  },
  {
    name: 'BlogMulti',
    path: '/BlogMulti'.toLowerCase(),
    components: {
      default: BlogMultiCatPage,
      navigation: null
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
  }
];


export default [...coreRoutes, ...siteRoutes]

