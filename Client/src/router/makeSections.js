import {Material} from 'sun'
import {Thread} from 'sun'
import {NewTopics} from 'sun'
import {ArticlesPage} from 'sun'
import {ForumPanel} from 'sun'
import {BlogPage} from 'sun'
import {Categories1} from 'sun'


export function makeForumSection(name, categoriesPanel) {
  let nameLower = name.toLowerCase();
  return [
    {
      name: `forum-${name}`,
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
      name: `forum-${name}-cat`,
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
      name: `forum-${name}-cat-mat`,
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
      name: `articles-${name}`,
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
      name: `articles-${name}-mat`,
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
      name: `articles-${name}`,
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
      name: `articles-${name}-mat`,
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
      name: `blog-${name}`,
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
      name:  `blog-${name}-mat`,
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

