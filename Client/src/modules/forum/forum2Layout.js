import {ForumPanel, NewTopics, Thread} from 'sun'
import {Categories2} from 'sun'
import {Material} from 'sun'

export default {
  name: 'Forum2',
  title: 'Forum 2',
  categoryType: 'Forum',
  getRoutes(category) {
    const name = category.name;
    const nameLower = name.toLowerCase();

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
          navigation: {categories: Categories2, categoryName: nameLower}
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
          navigation: {categories: Categories2, categoryName: nameLower}
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
          navigation: {categories: Categories2, categoryName: nameLower}
        }
      }
    ]
  }
}
