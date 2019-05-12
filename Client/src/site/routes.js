import {Categories1} from 'sun'
import {Categories2} from 'sun'
import {IndexPage} from 'sun'
import {News2Col} from 'sun'
import {BlogMultiCatPage} from 'sun'
import {ActivitiesPage} from 'sun'
import {PageWithMaterialInline} from 'sun'
import {makeArticlesSection, makeForumSection, makeBlogSection} from'sun'
import {mainRoutes} from 'sun'

const siteRoutes = [
  {
    name: "Home",
    path: '/',
    component: IndexPage
  },
  {
    name: "News",
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
    name: "News2Col",
    path: '/News2Col'.toLowerCase(),
    components: {
      default: News2Col,
      navigation: null
    }
  },
  {
    name: "PageWithMaterialInline",
    path: '/PageWithMaterialInline'.toLowerCase(),
    components: {
      default: PageWithMaterialInline,
      navigation: null
    }
  },
  {
    name: "BlogMulti",
    path: '/BlogMulti'.toLowerCase(),
    components: {
      default: BlogMultiCatPage,
      navigation: null
    },
    props: {
      default: {
        pageTitle: "Новые материалы",
        categoriesNames: "Forum,Articles,Blog".toLowerCase(),
        addButtonLabel: "Добавить материал",
        caption: "Посты в виде блога из категорий: Forum, Articles, Blog",
        rolesCanAdd: ["Admin", "Moderator"]
      }
    }
  },
  ...makeForumSection("Forum", Categories1),
  ...makeForumSection("Forum2L", Categories2),
  ...makeArticlesSection("Articles"),
  ...makeBlogSection("Blog")
];


export default [...mainRoutes, ...siteRoutes]

