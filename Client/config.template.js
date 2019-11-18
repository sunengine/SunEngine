const config = {

  API: 'http://localhost:5000',
  SiteUrl: "http://localhost:5005",
  UploadedImages: 'http://localhost:5000/UploadImages',
  Skin: 'http://localhost:5000/CurrentSkin/styles.css',
  //Skin: 'http://localhost:5005/statics/CurrentSkin/styles.css', // use to skin dev and place skin in statics/CurrentSkin

  OpenExternalLinksAtNewTab: true,

  VueDevTools: true,        // Do not use on production
  VueAppInWindow: true,     // Do not use on production

  Log: {
    InitExtended: true,     // Do not use on production
    Requests: true,         // Do not use on production
    MoveTo: true,           // Do not use on production
  }
};
