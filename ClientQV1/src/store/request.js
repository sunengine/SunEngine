import axios from 'axios'
import {removeTokens, setTokens, setTokensString, parseJwt} from 'services/tokens';
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

    const tokens = store.state.auth.tokens;

    if (tokens)
      headers['Authorization'] = `Bearer ${tokens.shortToken}`;

    let body = data.data;

    if (body) {
      if ((typeof body === 'object')) {
        if (data instanceof FormData) {

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
  if (rez.headers.tokens) {
    const tokens = JSON.parse(rez.headers.tokens);
    const exps = parseJwt(tokens.shortToken)

    tokens.shortTokenExpiration = new Date(exps.exp * 1000);

    if(store.state.auth.isPermanentLogin)
      setTokens(tokens);

    store.state.auth.tokens = tokens;
  } else if (rez.headers.tokensexpire) {
    store.state.auth.tokens = null;
    removeTokens();
    await store.dispatch('doLogout');
  }
  return rez;
}
