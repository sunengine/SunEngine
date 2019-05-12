export default function prepareAllCategories(state, root) {

  state.root = root;
  state.all = {};

  buildStructureRecursive(root);

  detectCanSomeChildrenWriteMaterial(root);

  setSettingsFromJson();

  function buildStructureRecursive(category, sectionRoot = null) {

    if (!category) {
      return;
    }

    // Add to all
    state.all[category.name.toLowerCase()] = category;

    // Make section types
    if (category.sectionType) {
      sectionRoot = category;
      //category.sectionRoot = category;
    } else if (sectionRoot) {
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

  function detectCanSomeChildrenWriteMaterial(category) {
    if (!category) {
      return;
    }

    let has = false;
    if (category.subCategories) {
      for (const cat of category.subCategories) {
        if (detectCanSomeChildrenWriteMaterial(cat))
          has = true;
      }
    }
    if (category.categoryPersonalAccess?.materialWrite)
      has = true;

    category.canSomeChildrenWriteMaterial = has;

    return has;
  }

  function setSettingsFromJson() {
    for (const cat in state.all) {
      if (cat.settingJson) {
        cat.settings = eval(cat.settingsJson);
      }
    }
  }
}
