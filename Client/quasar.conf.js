// Configuration for your app

const path = require('path');
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
      extendWebpack(cfg, { isServer, isClient } ) {
        cfg.resolve.modules.push(path.resolve('./src'));
        cfg.resolve.modules.push(path.resolve('./src/modules'));
        cfg.resolve.modules.push(path.resolve('./src/components'));
        cfg.resolve.modules.push(path.resolve('./src/classes'));
        if(ctx.dev) {
          cfg.plugins.push( new CopyWebpackPlugin([{from: 'configDev.js', to:'config.js'}]));
        } else {
          cfg.plugins.push( new CopyWebpackPlugin([{from: 'configProd.js',to:'config.js'}]));
        }
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
        'QChipsInput',
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
    ssr: {
      pwa: false
    },
    pwa: {
      // workboxPluginMode: 'InjectManifest',
      // workboxOptions: {},
      manifest: {
        // name: 'Quasar App',
        // short_name: 'Quasar-PWA',
        // description: 'Best PWA App in town!',
        display: 'standalone',
        orientation: 'portrait',
        background_color: '#ffffff',
        theme_color: '#027be3',
        icons: [
          {
            'src': 'statics/icons/icon-128x128.png',
            'sizes': '128x128',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-192x192.png',
            'sizes': '192x192',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-256x256.png',
            'sizes': '256x256',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-384x384.png',
            'sizes': '384x384',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-512x512.png',
            'sizes': '512x512',
            'type': 'image/png'
          }
        ]
      }
    },
  }
}
