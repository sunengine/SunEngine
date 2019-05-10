import {articlesLayout} from 'sun'
import {consoleInit} from 'sun'
import {registerLayoutsSite} from 'sun'


export default function (store) {
  console.info("%cRegister layouts: articlesLayout", consoleInit);
  store.commit("registerLayout", articlesLayout);
  registerLayoutsSite(store);
}
