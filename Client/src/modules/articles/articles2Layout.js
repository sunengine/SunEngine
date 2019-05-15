import {ArticlesPanel} from 'sun'
import {ArticlesMultiCatPage} from 'sun'
import {Categories2} from 'sun'
import {Material} from 'sun'
import {ArticlesPage} from 'sun'

export default {
  name: 'Articles2',
  title: 'Articles 2',
  categoryType: 'Articles',

  setCategoryRoute(category) {
    category.route = {
      name: `articles-${category.name}`,
      params: {}
    };

    for(const cat0 of category.subCategories) {
      for (const cat1 of cat0.subCategories) {
        cat1.route = {
          name: `articles-${category.name}-cat`,
          params: {
            categoryName: cat1.name
          }
        }
      }
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
          default: ArticlesMultiCatPage,
          navigation: ArticlesPanel
        },
        props: {
          default: {categoriesNames: nameLower, pageTitle: category.title},
          navigation: {categories: Categories2, categoryName: name}
        },
      },
      {
        name: `articles-${name}-cat`,
        path: `/${nameLower}/:categoryName`,
        components: {
          default: ArticlesPage,
          navigation: ArticlesPanel
        },
        props: {
          default: true,
          navigation: {categories: Categories2, categoryName: name}
        }
      },
      {
        name: `articles-${name}-cat-mat`,
        path: `/${nameLower}/:categoryName/:idOrName`,
        components: {
          default: Material,
          navigation: ArticlesPanel
        },
        props: {
          default: (route) => {
            return {categoryName: route.params.categoryName, idOrName: route.params.idOrName}
          },
          navigation: {categories: Categories2, categoryName: name}
        }
      }
    ]
  }
}
