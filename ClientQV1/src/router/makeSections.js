import Material from 'material/Material';
import Thread from 'forum/Thread';
import NewTopics from 'forum/NewTopics';
import ArticlesPage from 'articles/ArticlesPage';
import ForumPanel from 'forum/ForumPanel';
import BlogPage from "blog/BlogPage";

export function makeForumSection(name, categoriesPanel) {
  let nameLover = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLover,
      components: {
        default: NewTopics,
        navigation: ForumPanel
      },
      props: {
        default: {categoryName: nameLover},
        navigation: {categories: categoriesPanel, categoryName: nameLover}
      },
    },
    {
      path: `/${nameLover}/:categoryName`,
      components: {
        default: Thread,
        navigation: ForumPanel
      },
      props: {
        default: true,
        navigation: {categories: categoriesPanel, categoryName: nameLover}
      }
    },
    {
      path: `/${nameLover}/:categoryName/:id`,
      components: {
        default: Material,
        navigation: ForumPanel
      },
      props: {
        default: (route) => {
          return {categoryName: route.params.categoryName, id: +route.params.id}
        },
        navigation: {categories: categoriesPanel, categoryName: nameLover}
      }
    }
  ]
}


export function makeArticlesSection(name) {
  let nameLover = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLover,
      components: {
        default: ArticlesPage,
        navigation: null
      },
      props: {
        default: {
          categoryName: nameLover
        }
      }
    },
    {
      path: `/${nameLover}/:id`,
      components: {
        default: Material,
        navigation: null
      },
      props: {
        default: (route) => {
          return {
            categoryName: nameLover,
            id: +route.params.id
          }
        }
      }
    }
  ];
}


export function makeBlogSection(name) {
  let nameLover = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLover,
      components: {
        default: BlogPage,
        navigation: null
      },
      props: {
        default: {
          categoryName: nameLover
        }
      }
    },
    {
      path: `/${nameLover}/:id`,
      components: {
        default: Material,
        navigation: null
      },
      props: {
        default: (route) => {
          return {
            categoryName: nameLover,
            id: +route.params.id
          }
        }
      }
    }
  ];
}

