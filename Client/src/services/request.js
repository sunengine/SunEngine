import axios from 'axios'
import {removeToken, setToken, setTokenString} from './token';


const apiAxios = axios.create({baseURL: config.API, withCredentials: process.env.DEV});

export var tokens = null;

export function setSessionTokens(tok) {
  tokens = tok;
}

/*apiAxios.interceptors.request.use(x=>{
  console.log("interceptor",x);
  return x;
});*/

var awaiter = null;


export default async function request(url, data, sendAsJson = false) {

  const headers = {};

  debugger;

  if (tokens && tokens.shortTokenExpiration < new Date()) {
    headers['LongToken1'] = tokens.longToken.token;

    /*await apiAxios.post("Auth/Refresh").then(data => {
      tokens = data.tokens;
    });*/

    awaiter = makeRequest();
    await awaiter;
    let rez = awaiter;
    awaiter = null;
    return rez;
  }

  if (awaiter)
    await awaiter;

  return makeRequest();

  async function makeRequest() {

    debugger;

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
      }).then(rez => {
      if (rez.headers.tokens) {
        tokens = JSON.parse(rez.headers.tokens);
        setTokenString(rez.headers.tokens);
      } else if (rez.headers.tokensexpire) {
        tokens = null;
        removeToken();
      }
      return rez;
    });


  }
}

function ConvertObjectToFormData(obj) {
  const formData = new FormData();

  for (const key in obj) {
    formData.append(key, obj[key]);
  }

  return formData;
}
