import {QTree} from "quasar-framework/src/components/tree";

export default {
  name: "MyTree",
  extends: QTree,
  computed: {
    meta () {
      const meta = {}

      const travel = (node, parent) => {
        const tickStrategy = node.tickStrategy || (parent ? parent.tickStrategy : this.tickStrategy)
        const
          key = node[this.nodeKey],
          isParent = node.children && node.children.length > 0,
          isLeaf = !isParent,
          selectable = !node.disabled && this.hasSelection && node.selectable !== false,
          expandable = !node.disabled && node.expandable !== false,
          hasTicking = tickStrategy !== 'none',
          strictTicking = tickStrategy === 'strict',
          leafFilteredTicking = tickStrategy === 'leaf-filtered',
          leafTicking = tickStrategy === 'leaf' || tickStrategy === 'leaf-filtered'

        let tickable = !node.disabled && node.tickable !== false
        if (leafTicking && tickable && parent && !parent.tickable) {
          tickable = false
        }

        let lazy = node.lazy
        if (lazy && this.lazy[key]) {
          lazy = this.lazy[key]
        }

        const m = {
          key,
          parent,
          isParent,
          isLeaf,
          lazy,
          disabled: node.disabled,
          link: selectable, // || (expandable && (isParent || lazy === true)),
          children: [],
          matchesFilter: this.filter ? this.filterMethod(node, this.filter) : true,

          selected: key === this.selected && selectable,
          selectable,
          expanded: isParent ? this.innerExpanded.includes(key) : false,
          expandable,
          noTick: node.noTick || (!strictTicking && lazy && lazy !== 'loaded'),
          tickable,
          tickStrategy,
          hasTicking,
          strictTicking,
          leafFilteredTicking,
          leafTicking,
          ticked: strictTicking
            ? this.innerTicked.includes(key)
            : (isLeaf ? this.innerTicked.includes(key) : false)
        }

        meta[key] = m

        if (isParent) {
          m.children = node.children.map(n => travel(n, m))

          if (this.filter) {
            if (!m.matchesFilter) {
              m.matchesFilter = m.children.some(n => n.matchesFilter)
            }
            if (
              m.matchesFilter &&
              !m.noTick &&
              !m.disabled &&
              m.tickable &&
              leafFilteredTicking &&
              m.children.every(n => !n.matchesFilter || n.noTick || !n.tickable)
            ) {
              m.tickable = false
            }
          }

          if (m.matchesFilter) {
            if (!m.noTick && !strictTicking && m.children.every(n => n.noTick)) {
              m.noTick = true
            }

            if (leafTicking) {
              m.ticked = false
              m.indeterminate = m.children.some(node => node.indeterminate)

              if (!m.indeterminate) {
                const sel = m.children
                  .reduce((acc, meta) => meta.ticked ? acc + 1 : acc, 0)

                if (sel === m.children.length) {
                  m.ticked = true
                }
                else if (sel > 0) {
                  m.indeterminate = true
                }
              }
            }
          }
        }

        return m
      }

      this.nodes.forEach(node => travel(node, null))
      return meta
    }
  }
}
