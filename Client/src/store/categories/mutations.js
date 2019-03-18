
export function setCategories(state, root) {

  state.root = root;
  state.all = {};
  buildStructureRecursive(root);

  function buildStructureRecursive(category, sectionRoot = null) {

    if (!category) {
      return;
    }

    // Add to all
    state.all[category.name] = category;

    // Make section types
    if(category.sectionType) {
      sectionRoot = category;
      category.sectionRoot = category;
    }
    else if(sectionRoot) {
      category.sectionType = sectionRoot.sectionType;
      category.sectionRoot = sectionRoot;
    }

    if (!category.subCategories) {
      return;
    }

    for (let subCategory of category.subCategories) {

      // Make parents
      subCategory.parent = category;

      buildStructureRecursive(subCategory, sectionRoot);
    }
  }
}





