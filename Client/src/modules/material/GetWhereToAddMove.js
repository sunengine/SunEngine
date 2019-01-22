export function GetWhereToMove(store) {
  const rez = GoDeep(store.state.categories.root);
  return [...rez.children];
}

export function GetWhereToAdd(store, categoryName) {
  const rez = GoDeep(store.getters.getCategory(categoryName));
  if(rez.selectable)
    return [rez];
  else
    return [...rez.children];
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

  let ret;

  if (category?.categoryPersonalAccess?.MaterialWrite) {
    if (category.isMaterialsContainer) { // writable
      ret = {
        label: category.title,
        value: category.name,
        category: category,
        children: children,
        selectable: true,
        header: 'normal'
      };
    } else if(children){ // disabled mode on FolderCategory
      ret = {
        label: category.title,
        value: category.name,
        category: category,
        children: children,
        selectable: false,
        header: 'root'
      };
    }
  }

  if(!ret && children)
    ret = {children: children}

  return ret;
}

