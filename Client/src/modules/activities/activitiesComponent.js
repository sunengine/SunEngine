import {ActivitiesPage} from 'sun'

export default {
  name: 'Activities',
  title: 'Activities',

  getServerTemplate() {
    return {
      materialsCategories: "Root",
      commentsCategories: "Root",
      number: 25
    }
  },

  getClientTemplate() {
    return {
      title: "Activities"
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
          default: ActivitiesPage,
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
