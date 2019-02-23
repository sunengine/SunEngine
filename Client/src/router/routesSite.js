import Categories1 from 'categories/Categories1.vue';
import Categories2 from 'categories/Categories2.vue';
import TestExt from 'pages/TestExt.vue';
import Blog from 'blog/Blog';
import Index from 'pages/Index.vue';
import ActivitiesPage from 'activities/ActivitiesPage.vue';
import {makeArticlesSection, makeForumSection, makeBlogSection} from "./makeSections";


const routes = [
  {
    name: "Home",
    path: '/',
    component: Index
  },
  {
    name: "News",
    path: '/News'.toLowerCase(),
    components: {
      default: ActivitiesPage,
      navigation: null
    }
  },
  {
    path: '/TestExt'.toLowerCase(),
    component: TestExt,
  },
  ...makeForumSection("Forum", Categories1),
  ...makeForumSection("Forum2L", Categories2),
  ...makeArticlesSection("Articles"),
  ...makeBlogSection("Blog")
];


export default routes;

