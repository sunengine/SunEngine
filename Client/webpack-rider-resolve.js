// This file is for Rider IDE to understand js paths.

'use strict'

const path = require('path')

module.exports = {
  resolve: {
    modules: [
      path.resolve(__dirname, './src'),
      path.resolve(__dirname, './src/modules'),
      path.resolve(__dirname, './src/components'),
      path.resolve(__dirname, './src/services')
    ]
  }
}
