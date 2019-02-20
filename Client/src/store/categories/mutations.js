
export function setCategories(state, root) {
  state.root = root;

  let all = {};

  function deep(category, sectionRoot = null) {

    if (!category) {
      return;
    }

    all[category.name] = category;

    if(category.sectionType) {
      sectionRoot = category;
    }
    category.sectionRoot = sectionRoot;

    if (!category.subCategories) {
      return;
    }
    for (let subCategory of category.subCategories) {
      subCategory.parent = category;

      deep(subCategory, sectionRoot);
    }
  }

  deep(root);
  state.all = all;
}





