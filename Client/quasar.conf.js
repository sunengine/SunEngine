// Configuration for your app

var path = require('path');
const fs = require('fs');

var https = false;

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
        ? { // so on dev we'll have
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
    ssr: {
      pwa: false
    },
    pwa: {
      // workboxPluginMode: 'InjectManifest',
      // workboxOptions: {},
      manifest: {
        categoryName: 'SunEngine App',
        short_name: 'SunEngineApp',
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
    }
  }
}
