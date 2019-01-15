
export function getCategory (state) {
  return function(name) {
    return state.all[name.toLowerCase()];
  }
}
