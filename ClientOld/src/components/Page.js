
export default {
  data () {
    return {
      title: null,
    }
  },
  meta () {
    return {
      title: this.title
    }
  },
  methods: {
    setTitle(text) {
      this.title = text + " - " + config.SiteName;
    }
  }
}
