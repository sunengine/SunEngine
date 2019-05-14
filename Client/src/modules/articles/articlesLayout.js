import {ArticlesPage} from 'sun'
import {Material} from 'sun'

export default {
  name: 'Articles',
  title: 'Articles',
  categoryType: 'Articles',

  setCategoryRoute(category) {
    category.route = {
      name: `articles-${category.name}`,
      params:  {}
    }
  },

  getRoutes(category) {
    const name = category.name;
    const nameLower = name.toLowerCase();

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
    ]
  }
}
