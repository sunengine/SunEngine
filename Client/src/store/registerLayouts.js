import {articlesLayout} from 'sun'
import {blogLayout} from 'sun'
import {forum1Layout} from 'sun'
import {forum2Layout} from 'sun'
import {registerLayoutsSite} from 'sun'


export default function (store) {
  store.commit("registerLayout", articlesLayout);
  store.commit("registerLayout", blogLayout);
  store.commit("registerLayout", forum1Layout);
  store.commit("registerLayout", forum2Layout);

  registerLayoutsSite(store);
}
