export default function getLayout (state) {
  return function(name) {
    if(!name)
      return null;
    return state.layouts[name.toLowerCase()];
  }
}
