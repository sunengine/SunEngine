import {QEditor, QInnerLoading, QSpinnerGears} from 'quasar';
import ValidateMixin from 'quasar/src/mixins/validate';

import {editorButtons} from 'sun'

export default {
  name: 'MyEditor',
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
      let rez = "";
      for (let file of this.filesNames) {
        let imagePath = this.$imagePath(file);
        rez += `<div><img class="text-img" src="${imagePath}" /></div>`
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

      const files = Array.from(filesSelected).filter(x => isImage(x.name));
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
        ...QEditor.options.computed.buttonDef.call(this),
        addImages: {icon: 'camera_enhance', tip: this.$tl('uploadImages'), handler: this.uploadImages},
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

    return h('div', {staticClass: 'relative-position'}, [editor, errorMessage, fileInput, loading]);
  }
}

/*
* <div class="q-field__bottom absolute-bottom row items-start relative-position">
*     <div class="q-field__messages col">
*         <div>Field is required</div>
*     </div>
* </div>
* */
