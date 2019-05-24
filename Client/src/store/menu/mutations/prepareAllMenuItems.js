
export default function prepareAllMenuItems(state, allMenuItems) {

  let menuItemsById = {};

  for(const menuItem of allMenuItems) {
    menuItemsById[menuItem.id.toString()] = menuItem;
  }

  for(let menuItem of allMenuItems) {
    if(menuItem.parentId) {
      const parent = menuItemsById[menuItem.parentId.toString()];
      if(!parent)
        continue;

      if(!parent.subMenuItems)
        parent.subMenuItems = [];

      parent.subMenuItems.push(menuItem);
      menuItem.parent = parent;
    }
  }

  state.namedMenuItems = {};

  for(const menuItemId in menuItemsById) {
    const menuItem = menuItemsById[menuItemId.toString()];

    if(menuItem.name) {
      state.namedMenuItems[menuItem.name.toLowerCase()] = menuItem;
    }
  }

  for(const menuItem of allMenuItems) {
    if(menuItem.routeName) {
      menuItem.to = {
        name: menuItem.routeName,
        params: menuItem.routeParamsJson
      }
    }
  }
}
