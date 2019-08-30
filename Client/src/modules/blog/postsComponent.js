import {BlogMultiCatPage} from 'sun'

export default {
  name: 'Posts',
  title: 'Posts',

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
