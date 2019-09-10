import {request} from 'sun'

export default async function (context, data) {
  return await request(data.url, data.data, data.sendAsJson, data.skipLock);
}
