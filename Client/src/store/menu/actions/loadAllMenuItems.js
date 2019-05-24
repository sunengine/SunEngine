import { consoleInit } from 'sun'

export default async function loadAllMenuItems(context, data) {

  if(context.state.menu?.namedMenuItems)
    return;

  let requestData = {
    url: '/Menu/GetAllMenuItems'
  };

  if (data?.skipLock)
    requestData.skipLock = true;

  return await context.dispatch('request', requestData)
    .then(response => {
      console.info('%cLoadAllMenuItems', consoleInit, config.Log.InitExtended ? response.data : '' );
      context.commit('prepareAllMenuItems', response.data);
    });
}
