import Category from "Category";


export function setCategories(state, root) {
  state.root = root;

  let all = {};

  deep(all, root);
  state.all = all;
}


function deep(all, category) {

  if (!category) {
    return;
  }

  Object.setPrototypeOf(category,Category.prototype);


  all[category.name] = category;
  if (!category.subCategories) {
    return;
  }
  for (let subCategory of category.subCategories) {
    subCategory.parent = category;

    deep(all, subCategory);
  }
}


