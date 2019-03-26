export function GetWhereToMove(store) {
  const rez = GoDeep(store.state.categories.root);
  return [...rez.children];
}

export function GetWhereToAdd(store, categoriesNames) {
  if (categoriesNames.includes(","))
    return GetWhereToAddMultiCat(store, categoriesNames);
  else
    return GetWhereToAddOneCat(store, categoriesNames);
}

export function GetWhereToAddOneCat(store, categoryName) {
  let rez = GoDeep(store.getters.getCategory(categoryName));
  if (rez.selectable)
    rez = [rez];
  else
    rez = [...rez.children];

  return rez;
}

export function GetWhereToAddMultiCat(store, categoriesNames) {
  const categories = categoriesNames.split(",").map(x => x.trim());
  const nodes = [];
  for (let categoryName of categories) {
    const node = GoDeep(store.getters.getCategory(categoryName));
    if(node)
      nodes.push(node);
  }

  return nodes;
}


function GoDeep(category) {

  if (!category)
    return null;

  let children;
  if (category.subCategories) {
    children = [];
    for (let child of category.subCategories) {
      let one = GoDeep(child);
      if (one) children.push(one);
    }
  }

  if(children && children.length === 0)
    children = null;

  const ret = {
    label: category.title,
    value: category.name,
    category: category,
    children: children,
    selectable: false
  };

  if (category.categoryPersonalAccess?.materialWrite) {
    if (category.isMaterialsContainer) { // writable
      ret.icon = 'fas fa-folder';
      ret.iconColor = 'green-5';
      ret.selectable = true;
    } else if (children) { // disabled mode on FolderCategory
      ret.selectable = false;
    }

  } else if (!children) {
    return null;
  }
  return ret;
}

