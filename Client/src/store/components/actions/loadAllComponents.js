import {consoleInit} from 'sun'

export default async function(context,  data) {

  let requestData = {
    url: '/Components/GetAllComponents'
  };

  if (data?.skipLock)
    requestData.skipLock = true;

  return await context.dispatch('request', requestData)
    .then(response => {
      console.info('%cLoadAllComponents', consoleInit, config.Log.InitExtended ? response.data : '');
      context.state.allComponents = {};
      for(const comp of response.data) {
        context.state.allComponents[comp.name.toLowerCase()] = comp;
      }
    });
}
