import {ForumPanel, NewTopics, Thread} from 'sun'
import {Categories1} from 'sun'
import {Material} from 'sun'

export default {
  name: 'Forum1',
  title: 'Forum with 1 level threads',

  setCategoryRoute(category) {
    category.route = {
      name: `cat-${category.name}`,
      params:  {}
    };

    for(const cat of category.subCategories) {
      cat.route = {
        name: `cat-${category.name}-cat`,
        params:  {
          categoryName: cat.name
        }
      }
    }
  },

  getRoutes(category) {
    const name = category.name;
    const nameLower = name.toLowerCase();

    return [
      {
        name: `cat-${name}`,
        path: '/' + nameLower,
        components: {
          default: NewTopics,
          navigation: ForumPanel
        },
        props: {
          default: {categoryName: name},
          navigation: {categories: Categories1, categoryName: name}
        },
      },
      {
        name: `cat-${name}-cat`,
        path: `/${nameLower}/:categoryName`,
        components: {
          default: Thread,
          navigation: ForumPanel
        },
        props: {
          default: true,
          navigation: {categories: Categories1, categoryName: name}
        }
      },
      {
        name: `cat-${name}-cat-mat`,
        path: `/${nameLower}/:categoryName/:idOrName`,
        components: {
          default: Material,
          navigation: ForumPanel
        },
        props: {
          default: (route) => {
            return {categoryName: route.params.categoryName, idOrName: route.params.idOrName}
          },
          navigation: {categories: Categories1, categoryName: name}
        }
      }
    ]
  }
}
