
export function setCategories(state, root) {
  state.root = root;

  let all = {};

  function deep(category, sectionType = null) {

    if (!category) {
      return;
    }

    all[category.name] = category;

    if(category.sectionType) {
      sectionType = category.sectionType;
    }
    else {
      category.sectionType = sectionType;
    }

    if (!category.subCategories) {
      return;
    }
    for (let subCategory of category.subCategories) {
      subCategory.parent = category;

      deep(subCategory, sectionType);
    }
  }

  deep(root);
  state.all = all;
}





