import Vue from 'vue'

export default [
  ['bold', 'italic', 'strike', 'underline'],
  ['token', 'hr', 'link', 'addImages'],
  [
    {
      icon: Vue.prototype.$q.iconSet.editor.formatting,
      fixedLabel: true,
      list: 'no-icons',
      options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
    },
    {
      icon: Vue.prototype.$q.iconSet.editor.fontSize,
      fixedLabel: true,
      fixedIcon: true,
      list: 'no-icons',
      options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
    },
    'quote',
    'removeFormat',
  ],
  ['unordered', 'ordered'],
  ['undo', 'redo'],
  ['viewsource', 'fullscreen']
]
