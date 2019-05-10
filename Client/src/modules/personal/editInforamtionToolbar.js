export default [
  ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
  ['token', 'hr', 'link', 'addImages'],
  [
    {
      icon: $q.iconSet.editor.formatting,
      fixedLabel: true,
      list: 'no-icons',
      options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
    },
    {
      icon: $q.iconSet.editor.fontSize,
      fixedLabel: true,
      fixedIcon: true,
      list: 'no-icons',
      options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
    },
    'removeFormat'
  ],
  ['quote', 'unordered', 'ordered', 'outdent', 'indent',

    {
      icon: $q.iconSet.editor.align,
      fixedLabel: true,
      options: ['left', 'center', 'right', 'justify']
    }
  ],
  ['undo', 'redo', 'fullscreen'],
]
