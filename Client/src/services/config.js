var config;

if(window.config != null)
  config = window.config;
else
  config = process.env;

export default config;
