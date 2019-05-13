import Vue from 'vue';

import {consoleInit} from 'sun';

export default function registerLayout(state, layout) {
  console.info(`%cRegister layout: ${layout.title}`, consoleInit);
  Vue.set(state.layouts, layout.name.toLowerCase(), layout);
}





