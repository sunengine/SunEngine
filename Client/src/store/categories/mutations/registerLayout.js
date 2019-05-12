import Vue from 'vue';

export default function registerLayout(state, layout) {
  Vue.set(state.layouts, layout.name.toLowerCase(), layout);
}





