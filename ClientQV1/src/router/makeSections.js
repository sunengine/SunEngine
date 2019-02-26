import Material from 'material/Material.vue';
import Thread from 'forum/Thread.vue';
import NewTopics from 'forum/NewTopics.vue';
import ArticlesList from 'articles/ArticlesList.vue';
import ForumNavPanel from 'forum/ForumNavPanel';
import Blog from "blog/Blog";

export function makeForumSection(name, categoriesPanel) {
  let nameLover = name.toLowerCase();
  return [
    {
      name: name,
      path: '/' + nameLover,
      components: {
        default: NewTopics,
        navigation: ForumNavPanel
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
        navigation: ForumNavPanel
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
        navigation: ForumNavPanel
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
        default: ArticlesList,
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
        default: Blog,
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
