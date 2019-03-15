import Categories1 from 'categories/Categories1';
import Categories2 from 'categories/Categories2';
//import TestExt from 'pages/TestExt';
import Index from 'site/Index';
import News2Col from 'site/News2Col'
import ActivitiesPage from 'activities/ActivitiesPage';
import {makeArticlesSection, makeForumSection, makeBlogSection} from "router/makeSections";


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
/*  {
    path: '/TestExt'.toLowerCase(),
    component: TestExt,
  },*/
  ...makeForumSection("Forum", Categories1),
  ...makeForumSection("Forum2L", Categories2),
  ...makeArticlesSection("Articles"),
  ...makeBlogSection("Blog")
];


export default routes;

