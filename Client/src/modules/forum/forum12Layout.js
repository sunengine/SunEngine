export default {

}


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
