import Vue from "vue";
import { consoleInit } from "sun";

Vue.config.devtools = config.Dev.VueDevTools;
if (Vue.config.devtools) console.log(`%cVueDevTools enabled`, consoleInit);
