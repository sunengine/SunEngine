import {consoleInit} from "services/consoleStyles";

export async function getAllCategories(context, data) {

  let requestData = {
    url: "/Categories/GetAllCategoriesAndAccesses"
  };

  if (data?.skipLock)
    requestData.skipLock = true;

  return await context.dispatch("request", requestData)
    .then(response => {
      console.info("%cGetAllCategories", consoleInit);
      context.commit('setCategories', response.data);
    });
}
