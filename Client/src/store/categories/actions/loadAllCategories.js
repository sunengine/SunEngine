import {consoleInit} from 'sun'

export default async function(context, data) {

  let requestData = {
    url: '/Categories/GetAllCategoriesAndAccesses'
  };

  if (data?.skipLock)
    requestData.skipLock = true;

  return await context.dispatch('request', requestData)
    .then(response => {
      console.info('%cLoadAllCategories', consoleInit, config.Log.InitExtended ? response.data : '');
      context.commit('prepareAllCategories', response.data);
    });
}
