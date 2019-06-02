import Vue from 'vue';


export default function(state, layout) {
  if (!state.layouts)
    state.layouts = {};
  Vue.set(state.layouts, layout.name.toLowerCase(), layout);
}





