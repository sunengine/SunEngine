<template>
  <div>

    <q-input ref="name" v-model="category.name" :label="$ta('name')" :rules="rules.name"/>

    <q-input ref="title" v-model="category.title" :label="$ta('title')" :rules="rules.title" />

    <q-input ref="description" v-model="category.description" autogrow type="textarea"
             :label="$ta('shortDescription')"/>


    <div class="q-mt-sm text-grey-6">{{$ta('header')}}</div>

    <MyEditor ref="header" style="margin-bottom: 12px;" v-model="category.header"/>

    <!--    <div :class="[{invisible: !(category.parentId.$invalid && !start)},'error']">
          {{$ta('selectParent')}}
        </div>-->
    <q-btn v-if="root" class="q-mt-md select-category" :label="parentCategoryTitle" no-caps outline
           icon="fas fa-folder">
      <q-menu>
        <div style="background-color: white;" class="q-pa-sm">
          <q-tree @update:selected="categorySelected" v-close-menu
                  default-expand-all
                  :selected.sync="category.parentId"
                  :nodes="where"
                  node-key="value">
            <div slot="header-normal" slot-scope="prop" class="row items-center">
              <div style="display: flex; align-items: center; align-content: center;">
                <q-icon name="fas fa-folder" size="16px" color="green-5" class="q-mr-sm"/>
                <span>{{ prop.node.label }}</span>
              </div>
            </div>
          </q-tree>
        </div>
      </q-menu>
    </q-btn>

    <div class="q-mt-lg">
      <q-select v-if="sectionTypes" :label="$ta('sectionType')" v-model="category.sectionTypeName"
                :options="sectionTypeOptions"/>
      <LoaderWait v-else/>
    </div>

    <div class="q-mt-lg">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isMaterialsContainer" :label="$ta('isMaterialsContainerCb')"/>
    </div>

    <div class="q-my-sm">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isCacheContent" label="Кэшировать содержимое"/>
    </div>

    <div class="q-my-sm">
      <q-checkbox toggle-indeterminate v-model="category.appendUrlToken" :label="$ta('appendUrlTokenCb')"/>
      <span class="text-amber-8 q-ml-md">
        {{$ta("appendUrlTokenInfo")}}
      </span>
    </div>
    <div>
      <q-checkbox :toggle-indeterminate="false" v-model="category.isHidden" :label="$ta('hideCb')"/>
    </div>
  </div>
</template>

<script>
  import MyEditor from "MyEditor";
  import MyTree from 'MyTree';
  import adminGetAllCategories from "services/adminGetAllCategories";
  import LoaderWait from "components/LoaderWait";

  const unset = "unset";

  //const allowedChars = helpers.regex('allowedChars', )

  function GoDeep(category) {

    if (!category)
      return;

    let children;
    if (category.subCategories) {
      children = [];
      for (let child of category.subCategories) {
        let one = GoDeep(child);
        if (one) children.push(one);
      }
    }

    return {
      label: category.title,
      value: category.id,
      category: category,
      children: children,
      selectable: true,
      header: 'normal'
    };
  }


  function createRules() {
    return {
      name: [
        value => !!value || this.$ta("validation.name.required"),
        value => value.length >= 2 || this.$ta("validation.name.minLength"),
        value => /^[a-zA-Z0-9-]*$/.test(value) || this.$ta("validation.name.allowedChars"),
      ],
      title: [
        value => !!value || this.$ta("validation.title.required"),
        value => value.length >= 3 || this.$ta("validation.title.minLength"),
      ],

    }
  }

  export default {
    name: "CategoryForm",
    components: {LoaderWait, MyEditor, MyTree},
    props: {
      category: {
        type: Object,
        required: true,
      },
    },
    data: function () {
      return {
        root: null,
        all: null,
        start: true,
        sectionTypes: null
      }
    },
    rules: null,
    computed: {
      sectionTypeOptions() {
        return [{label: "Без типа", value: unset}, ...this.sectionTypes?.map(x => {
          return {
            label: x.title,
            value: x.name
          }
        })];
      },
      parentCategoryTitle() {
        if (!this.category.parentId)
          return this.$ta("selectParent");
        return this.$ta("parent") + this?.all?.[this.category.parentId]?.title;
      }
      ,
      where() {
        return [GoDeep(this.root)];
      }
    },
    methods: {
      categorySelected(key) {
        if(!key)
        {
          let pid = this.category.parentId;
          this.$nextTick(() => {this.category.parentId = pid})
        }
      },
      validate() {
        this.$refs.name.validate();
        this.$refs.title.validate();
      },
      hasError() {
        return this.$refs.name.hasError || this.$refs.title.hasError;
      }
    },
    async created() {
      if (!this.category.sectionTypeName)
        this.category.sectionTypeName = unset;

      this.rules = createRules.call(this);

      await adminGetAllCategories().then(
        data => {
          this.root = data.root;
          this.all = data.all;
        }
      );

      await this.$store.dispatch("request",
        {
          url: "/Admin/AdminCategories/GetAllSectionTypes"
        })
        .then(response => {
            this.sectionTypes = response.data;
          }
        );
    }
  }
</script>

<style lang="stylus" scoped>


</style>
