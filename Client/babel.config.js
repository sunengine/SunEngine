module.exports = {
  presets: [
    '@quasar/babel-preset-app',
    [
      "babel-preset-proposals",
      {
        "optionalChaining": true,
        "nullishCoalescingOperator": true,
        "exportDefaultFrom": true
      }
    ]
  ]
};
