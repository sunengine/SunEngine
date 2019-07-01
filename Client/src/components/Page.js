
export default {
  data() {
    return {
      title: "_",
    }
  },
  meta() {
    return {
      title: this.title,
      titleTemplate: title => title === "_" ? config.SiteName :  `${title} - ${config.SiteName}`
    }
  }
}
