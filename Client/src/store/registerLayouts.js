import articlesLayout from 'articles/articlesLayout'
import { consoleInit } from "defination";
import registerLayoutsSite from "site/registerLayoutsSite"

export default function(store) {
  console.info("%cRegister layouts: articlesLayout", consoleInit);
  store.commit("registerLayout", articlesLayout);
  registerLayoutsSite(store);
}
