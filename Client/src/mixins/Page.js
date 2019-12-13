export default {
  data() {
    return {
      title: " ",
    }
  },
  methods: {
    setTitle(title) {
      this.title = title;
    }
  },
  meta() {
    return {
      title: this.title,
      titleTemplate: title => title === " " ? config.Global.SiteName : `${title} - ${config.Global.SiteName}`
    }
  }
}
