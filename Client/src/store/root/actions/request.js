import axios from 'axios'
import Lock from 'js-lock'

import {store as store} from 'sun'
import {router} from 'sun'
import {app} from 'sun'
import {removeTokens, setTokens, getTokens} from 'sun'
import {routeCheckAccess} from 'sun'
import {consoleTokens, consoleUserLogout, consoleRequestStart, consoleRequestUrl} from 'sun'


const lock = new Lock("request-lock");


const apiAxios = axios.create({baseURL: config.API, withCredentials: process.env.DEV});


apiAxios.interceptors.response.use(async rez => {
  await checkTokens(rez);
  return rez;
}, async rez => {
  console.error(rez.response);
  await checkTokens(rez.response);
  throw rez;
});


export default async function request(context, data) {

  const url = data.url;

  if (config.Log.Requests)
    console.log(`%cRequest%c${url}`, consoleRequestStart, consoleRequestUrl, data);

  const sendAsJson = data.sendAsJson ?? false;

  const headers = {};

  if (data.skipLock) {
    if (checkLocalTokensExpire()) {

      headers['LongToken1'] = getTokens().longToken;
    }
    return makeRequest();
  }

  return lock.lock(() => {

      if (checkLocalTokensExpire()) {

        headers['LongToken1'] = getTokens().longToken;
        return makeRequest();
      }
    }
  ).then(x => {
    if (x)
      return x;
    else
      return makeRequest();
  });

  function checkLocalTokensExpire() {

    const tokens = getTokens()?.tokens;

    if (!tokens)
      return false;

    const nowDate = new Date(new Date().toUTCString()).getTime();
    const exp = tokens.shortTokenExpiration.getTime();

    console.log("nowDate - exp", nowDate, exp);

    const rez = exp < nowDate;

    if (rez)
      console.log('%cTokens expire', consoleTokens);

    return rez;
  }

  function makeRequest() {

    const tokens = getTokens();

    if (tokens)
      headers['Authorization'] = `Bearer ${tokens.shortToken}`;

    let body = data.data;

    if (body) {
      if ((typeof body === 'object')) {
        if (body instanceof FormData) {

        } else if (sendAsJson === false) {
          body = ConvertObjectToFormData(body);
        } else {
          headers['Content-Type'] = 'application/json';
          body = JSON.stringify(body);
        }
      } else {
        headers['Content-Type'] = 'application/x-www-form-urlencoded';
      }
    }

    return apiAxios.post(url, body,
      {
        headers: headers,
      });

  }
}


function ConvertObjectToFormData(obj) {
  const formData = new FormData();

  for (const key in obj) {
    if (obj.hasOwnProperty(key)) {
      formData.append(key, obj[key]);
    }
  }

  return formData;
}

async function checkTokens(rez) {
  const tokensHeader = rez.headers.tokens;

  if (tokensHeader) {

    if (tokensHeader === 'expire') {

      removeTokens();
      console.info('%cLogout', consoleUserLogout);

      store.commit('clearAllUserRelatedData');
      await store.dispatch('loadAllCategories', {skipLock: true});
      await store.dispatch('loadAllMenuItems', {skipLock: true});
      await store.dispatch('setAllRoutes');

      routeCheckAccess(router.currentRoute);
      router.push(router.currentRoute);
      app.rerender();
      return;

    } else {

      const newTokens = JSON.parse(tokensHeader);

      newTokens.shortTokenExpiration = new Date(newTokens.shortTokenExpiration);

      store.state.auth.longToken = newTokens.longToken;

      setTokens(newTokens);

      console.info('%cTokens refreshed', consoleTokens);

    }
  }

  return rez;
}
