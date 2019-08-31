import {BlogMultiCatPage} from 'sun'

export default {
  name: 'Posts',
  title: 'Posts',

  getServerTemplate() {
    return {
      categoriesNames: "Root",
      previewSize: 800,
      pageSize: 12
    }
  },

  getClientTemplate() {
    return {
      title: "Posts",
      subTitle: null,
      header: null,
      categoriesNames: "Root",
      rolesCanAdd: null,
      addButtonLabel: null
    }
  },

  getRoutes(component) {
    const name = component.name;
    const nameLower = name.toLowerCase();

    return [
      {
        name: `comp-${name}`,
        path: '/' + nameLower,
        components: {
          default: BlogMultiCatPage,
          navigation: null
        },
        props: {
          default: {
            componentName: nameLower
          }
        },
        meta: {
          component: component
        }
      }
    ]
  }
}
