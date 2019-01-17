import {QEditor, QInnerLoading, QSpinnerGears} from "quasar";


export default {
  name: "MyEditor",
  extends: QEditor,
  data: function () {
    return {
      filesNumber: 0,
      filesNames: [],
      filesLoading: false
    }
  },
  methods: {
    oneFileDone() {
      this.filesNumber--;
      if (this.filesNumber <= 0) {
        this.allUploaded();
      }
    },

    allUploaded() {
      this.filesNames = this.filesNames.filter(x => x);
      this.runCmd('insertHTML', this.getImagesHtml(), true);
      this.filesNames = [];
      this.filesNumber = 0;
      this.filesLoading = false;
    },

    getImagesHtml() {
      let rez = "";
      for (let file of this.filesNames) {
        let imagePath = this.$imagePath(file);
        rez += `<div><img class="text-img" src="${imagePath}" /></div>`
      }
      return rez;
    },

    handleFiles() {
      debugger;

      const filesSelected = this.$refs.file.files;

      if (!filesSelected.length)
        return;

      function isImage(name) {
        return /(.gif|.jpg|.jpeg|.png|.svg)$/i.test(name);
      }

      const files = Array.from(filesSelected).filter(x => isImage(x.name));
      if (!files.length)
        return;

      this.filesNumber = files.length;
      this.filesLoading = true;

      for (let i = 0; i < files.length; i++) {

        let formData = new FormData();
        formData.append('file', files[i]);



        this.$store.dispatch("request",
          {
            url: "/Images/UploadImage",
            data: formData
          })
          .then(response => {
            this.filesNames[i] = response.data.fileName;
            this.oneFileDone();
          })
          .catch(x => {
            console.log("error", x);
            this.oneFileDone();
          });
      }
    },

    uploadImages() {
      this.$refs.file.click();
    }
  },

  computed: {

    buttonDef() {
      return {
        ...QEditor.computed.buttonDef.call(this),
        addImages: {icon: 'camera_enhance', tip: 'Добавить изображения', handler: this.uploadImages}
      };
    }
  },

  render(h) {
    let fileInput = h("input", {
      ref: "file",
      attrs: {type: "file", accept: "image/*", multiple: true},
      style: {display: "none"},
      on: {change: this.handleFiles}
    });
    let editor = QEditor.render.call(this, h);
    let loading = h(QInnerLoading, {props: {visible: this.filesLoading}},
      [h(QSpinnerGears, {props: {size: "60px"}, class: "text-grey-8"})]
    );
    return h('div', {class: "relative-position"}, [editor, fileInput, loading]);
  }
}


