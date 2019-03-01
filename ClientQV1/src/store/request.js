import axios from 'axios'
import {removeTokens, setTokens, setTokensString, parseJwt} from './token';
import Lock from 'js-lock';
import {store} from 'store';

const apiAxios = axios.create({baseURL: config.API, withCredentials: process.env.DEV});

apiAxios.interceptors.response.use(async rez => {
  return await checkTokens(rez)
}, async rez => {
  await checkTokens(rez.response);
  throw rez;
});




const lock = new Lock("request-lock");

export default async function request(context, data) {

  const url = data.url;
  const sendAsJson = data.sendAsJson ?? false;

  const headers = {};

  return lock.lock(
    () => {
      const tokens = store.state.auth.tokens;

      if (tokens && tokens.shortTokenExpiration < new Date(new Date().toUTCString())) {
        headers['LongToken1'] = tokens.longToken.token;
        return makeRequest();
      }
    }).then(x => {
    if (x)
      return x;
    else
      return makeRequest();
  });


  function makeRequest() {

    if (tokens)
      headers['Authorization'] = `Bearer ${tokens.shortToken}`;

    let body;

    if (data) {
      if ((typeof data === 'object')) {
        if (data instanceof FormData) {
          body = data;
        } else if (sendAsJson === false) {
          body = ConvertObjectToFormData(data);
        } else {
          headers['Content-Type'] = 'application/json';
          body = JSON.stringify(data);
        }
      } else {
        headers['Content-Type'] = 'application/x-www-form-urlencoded';
        body = data;
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
  if (rez.headers.tokens) {
    tokens = JSON.parse(rez.headers.tokens);
    const exps = parseJwt(tokens.shortToken)
    tokens.shortTokenExpiration = new Date(exps.exp * 1000);
    setTokens(tokens);
  } else if (rez.headers.tokensexpire) {
    tokens = null;
    removeTokens();
    await store.dispatch('doLogout');
  }
  return rez;
}
