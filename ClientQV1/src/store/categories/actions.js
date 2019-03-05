import {consoleInitStyle} from "services/consoleStyles";

export async function getAllCategories(context, data) {
  //console.log("GetAllCategories 0");

  let requestData = {
    url: "/Categories/GetAllCategoriesAndAccesses"
  };

  if (data?.skipLock)
    requestData.skipLock = true;

  await context.dispatch("request", requestData)
    .then(response => {
      console.info("%cGetAllCategories", consoleInitStyle);
      context.commit('setCategories', response.data);
    }).catch(error => {
      console.log("error", error);
    });
}
