let app1;

export default async ({app}) => {
  window.App = app;
  app1 = app;
}

export {app1 as app};
