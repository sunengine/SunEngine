import Vue from 'vue';

import {consoleRequestStart, consoleRequestUrl} from 'sun';

export default function registerLayout(state, layout) {
  console.info(`%cRegister layout%c${layout.title}`, consoleRequestStart,consoleRequestUrl);
  Vue.set(state.layouts, layout.name.toLowerCase(), layout);
}





