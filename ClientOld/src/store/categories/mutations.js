
export function setCategories(state, root) {

  state.root = root;
  state.all = {};
  buildStructureRecursive(root);

  function buildStructureRecursive(category, sectionType = null) {

    if (!category) {
      return;
    }

    // Add to all
    state.all[category.name] = category;

    // Make section types
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

      // Make parents
      subCategory.parent = category;

      buildStructureRecursive(subCategory, sectionType);
    }
  }
}





