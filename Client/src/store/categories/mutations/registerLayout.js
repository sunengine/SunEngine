import Vue from 'vue';


export default function registerLayout(state, layout) {
  if (!state.layouts)
    state.layouts = {};
  Vue.set(state.layouts, layout.name.toLowerCase(), layout);
}





