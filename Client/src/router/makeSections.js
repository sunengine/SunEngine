import Material from 'material/Material';
import Thread from 'forum/Thread';
import NewTopics from 'forum/NewTopics';
import ArticlesPage from 'articles/ArticlesPage';
import ForumPanel from 'forum/ForumPanel';
import BlogPage from "blog/BlogPage";
import Categories1 from "categories/Categories1";

export function makeForumSection(name, categoriesPanel) {
  let nameLower = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLower,
      components: {
        default: NewTopics,
        navigation: ForumPanel
      },
      props: {
        default: {categoryName: nameLower},
        navigation: {categories: categoriesPanel, categoryName: nameLower}
      },
    },
    {
      path: `/${nameLower}/:categoryName`,
      components: {
        default: Thread,
        navigation: ForumPanel
      },
      props: {
        default: true,
        navigation: {categories: categoriesPanel, categoryName: nameLower}
      }
    },
    {
      path: `/${nameLower}/:categoryName/:id`,
      components: {
        default: Material,
        navigation: ForumPanel
      },
      props: {
        default: (route) => {
          return {categoryName: route.params.categoryName, idOrName: route.params.id}
        },
        navigation: {categories: categoriesPanel, categoryName: nameLower}
      }
    }
  ]
}


export function makeArticlesSection(name) {
  let nameLower = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLower,
      components: {
        default: ArticlesPage,
        navigation: null
      },
      props: {
        default: {
          categoryName: nameLower
        }
      }
    },
    {
      path: `/${nameLower}/:idOrName`,
      components: {
        default: Material,
        navigation: null
      },
      props: {
        default: (route) => {
          return {
            categoryName: nameLower,
            idOrName: route.params.idOrName
          }
        }
      }
    }
  ];
}

export function makeArticlesSectionWithMenu(name) {
  let nameLower = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLower,
      components: {
        default: ArticlesPage,
        navigation: Categories1
      },
      props: {
        default: {
          categoryName: nameLower
        }
      }
    },
    {
      path: `/${nameLower}/:idOrName`,
      components: {
        default: Material,
        navigation: Categories1
      },
      props: {
        default: (route) => {
          return {
            categoryName: nameLower,
            idOrName: route.params.idOrName
          }
        }
      }
    }
  ];
}


export function makeBlogSection(name) {
  let nameLower = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLower,
      components: {
        default: BlogPage,
        navigation: null
      },
      props: {
        default: {
          categoryName: nameLower
        }
      }
    },
    {
      path: `/${nameLower}/:id`,
      components: {
        default: Material,
        navigation: null
      },
      props: {
        default: (route) => {
          return {
            categoryName: nameLower,
            idOrName: route.params.id
          }
        }
      }
    }
  ];
}

