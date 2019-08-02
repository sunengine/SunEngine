import {QEditor, QInnerLoading, QSpinnerGears} from 'quasar';
import ValidateMixin from 'quasar/src/mixins/validate';

import {editorButtons} from 'sun'

export default {
  name: 'SunEditor',
  extends: QEditor,
  mixins: [ValidateMixin],
  data: function () {
    return {
      filesNumber: 0,
      filesNames: [],
      filesLoading: false,
      cursorX: null,
      cursorY: null
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
      this.$refs.file.value = '';
    },

    getImagesHtml() {
      let rez = '';
      for (let file of this.filesNames) {
        let imagePath = this.$imagePath(file);
        rez += `<div><img src="${imagePath}" /></div>`
      }
      return rez;
    },

    handleFiles() {
      const filesSelected = this.$refs.file.files;

      if (!filesSelected.length)
        return;

      function isImage(name) {
        return /(.gif|.jpg|.jpeg|.png|.svg)$/i.test(name);
      }

      const files = [];
      const filesRejected = [];

      Array.from(filesSelected).forEach(x => {
        if (isImage(x.name))
          files.push(x);
        else
          filesRejected.push(x.name)
      });

      if (filesRejected.length)
        console.error(`Files: ${filesRejected.map(x => `"${x}"`).join(", ")} has wrong extensions, allowed only gif,jpg,jpeg,png,svg extensions.`);

      if (!files.length)
        return;

      this.filesNumber = files.length;
      this.filesLoading = true;

      for (let i = 0; i < files.length; i++) {

        const formData = new FormData();
        formData.append('file', files[i]);

        this.$store.dispatch('request',
          {
            url: '/UploadImages/UploadImage',
            data: formData
          })
          .then(response => {
            this.filesNames[i] = response.data.fileName;
            this.oneFileDone();
          })
          .catch(error => {
            console.log("error", error);
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
        ...QEditor.options.computed.buttonDef.call(this),
        addImages: {icon: 'fas fa-image', tip: this.$tl('uploadImages'), handler: this.uploadImages},
        ...editorButtons
      };
    }
  },

  render(h) {
    const fileInput = h('input', {
      ref: 'file',
      attrs: {type: 'file', accept: 'image/*', multiple: true},
      style: {display: 'none'},
      on: {change: this.handleFiles}
    });
    const editor = QEditor.options.render.call(this, h);
    const loading = h(QInnerLoading, {props: {showing: this.filesLoading}},
      [h(QSpinnerGears, {props: {size: '60px'}, class: 'text-grey-8'})]
    );

    const error = h('div', {
      staticClass: 'error',
      key: 'q--slot-error'
    }, this.computedErrorMessage);

    const errorTransition = h('transition', {
      staticClass: '',
      props: {
        name: 'q-transition--field-message',
      }
    }, [error]);

    const errorMessage = this.hasError && errorTransition;

    return h('div', {staticClass: 'relative-position sun-editor'}, [editor, errorMessage, fileInput, loading]);
  }
}
