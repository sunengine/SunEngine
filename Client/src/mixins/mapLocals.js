import Vue from 'vue'



export default function (name) {

  //const name = this.$options.name;

  const v = require("Vue");
  debugger;

  const messages = v.prototype.$i18n.messages[ v.prototype.$i18n.locale][name];
  const rez = {};


  for (const [key, value] of messages.entries()) {

    rez[key] = function () {
      return value;
    }
  }

  debugger;

  return rez;
}
