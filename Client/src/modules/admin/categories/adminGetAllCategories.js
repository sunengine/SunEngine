import {store} from 'sun'

export default function adminGetAllCategories() {
  return store.dispatch('request',
    {
      url: '/Admin/CategoriesAdmin/GetAllCategories'
    })
    .then(response => {
        return {
          root: response.data,
          all: findAll(response.data, {})
        };
      }
    );

  function findAll(category, all) {
    all[category.id] = category;
    if (category.subCategories)
      for (let cat of category.subCategories)
        findAll(cat, all);
    return all;
  }
}


