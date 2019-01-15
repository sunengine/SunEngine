import axios from 'axios'

export default async function request(url, data, sendAsJson = false, token = null /* or it will be send as FormData */) {

  var headers = {};

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  headers['Accept'] = 'application/json';

  var body;

  if (data) {
    if ((typeof data === 'object')) {
      if (data instanceof FormData) {
        body = data;
      }
      else if (sendAsJson === false) {
        body = ConvertObjectToFormData(data);
      }
      else {
        headers['Content-Type'] = 'application/json';
        body = JSON.stringify(data);
      }
    }
    else {
      headers['Content-Type'] = 'application/x-www-form-urlencoded';
      body = data;
    }
  }

  return await axios({
    method: 'post',
    url: url,
    baseURL: process.env.API,
    data: body,
    headers: headers
  });
}

function ConvertObjectToFormData(obj) {
  var formData = new FormData();

  for (var key in obj) {
    formData.append(key, obj[key]);
  }

  return formData;
}
