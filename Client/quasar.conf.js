// Configuration for your app

var path = require('path');


module.exports = function (ctx) {
  return {
    plugins: [
      "buildPath", "formatDate", "vuelidate", "ext", "imagePath"
    ],
    css: [
      'app.styl'
    ],
    extras: [
      ctx.theme.mat ? 'roboto-font' : null,
      'material-icons',
      'fontawesome'
    ],
    supportIE: false,
    build: {
      scopeHoisting: true,
      vueRouterMode: 'history',
      // vueCompiler: true,
      // gzip: true,
      // analyze: true,
      // extractCSS: false,
      extendWebpack(cfg) {
        cfg.resolve.modules.push(path.resolve('./src'));
        cfg.resolve.modules.push(path.resolve('./src/modules'));
        cfg.resolve.modules.push(path.resolve('./src/components'));
        cfg.resolve.modules.push(path.resolve('./src/classes'));
      },
      env: ctx.dev
        ? { // on dev
          API: JSON.stringify('http://localhost:5000'),
          UploadedImages: JSON.stringify('http://localhost:5000/UploadImages'),
          SiteName: JSON.stringify('SunEngine')
        }
        : { // and on build (production):
          API: JSON.stringify('http://localhost:8000/api'),
          UploadedImages: JSON.stringify('http://localhost:8000/UploadImages'),
          SiteName: JSON.stringify('SunEngine')
        }
    },
    devServer: {
      https: false,
      host: 'localhost',
      port: 5005,
      open: true // opens browser window automatically
    },
    framework: {
      components: [
        'QLayout',
        'QLayoutHeader',
        'QLayoutFooter',
        'QLayoutDrawer',
        'QPageContainer',
        'QPage',
        'QToolbar',
        'QToolbarTitle',
        'QBtn',
        'QIcon',
        'QList',
        'QListHeader',
        'QItem',
        'QItemMain',
        'QItemSide',
        'QItemTile',
        'QEditor',
        'QInput',
        'QField',
        'QAlert',
        'QPagination',
        'QCheckbox',
        'QTree',
        'QSpinner',
        'QSpinnerMat',
        'QPopover',
        'QChip',
      ],
      directives: [
        'Ripple',
        'CloseOverlay'
      ],
      plugins: [
        'Notify',
        'Dialog',
        'Meta'
      ],
      i18n: 'ru',
    },
    animations: [],
  }
}
