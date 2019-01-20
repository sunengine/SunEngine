// Configuration for your app

var path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin')


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
        if(ctx.dev) {
          cfg.plugins.push( new CopyWebpackPlugin([{from: 'configDev.js', to:'config.js'}]));
        } else {
          cfg.plugins.push( new CopyWebpackPlugin([{from: 'configProd.js',to:'config.js'}]));
        }
      },
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
        'QSpinnerGears',
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
