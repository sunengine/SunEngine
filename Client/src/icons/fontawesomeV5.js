import {Glob} from "glob";

const global = {
    delete: "fas fa-trash-alt",
    save: "far fa-save",
    cancel: "fas fa-times",
    refresh: "fas fa-sync-alt",
    goTo: "fas fa-arrow-right",
    search: "fas fa-search",
    eye: "far fa-eye",
    eyeSlash: "far fa-eye-slash",
    upload: "fas fa-cloud-upload-alt",
    add: "fas fa-plus",
    clock: "far fa-clock",
    user: "far fa-user",
    users: "fas fa-user",
    send: "fas fa-arrow-circle-right",
    comment: "far fa-comment",
    category: "fas fa-folder",
    key: "fas fa-key",
    envelope: "fas fa-envelope"
};

export default {
    name: "fontawesome-v5",
    sunName: "FontAwesome",
    global: global,
    CategoriesAdmin: {
        addCategoryBtn: global.add
    },
    CategoryForm: {
        icons: "fas fa-icons",
        category: global.category,
        layout: "fas fa-table"
    },
    CategoryItem: {
        up: "fas fa-chevron-up",
        down: "fas fa-chevron-down",
        eyeSlash: global.eyeSlash,
        edit: "fas fa-wrench",
        plus: global.add,
        goTo: global.goTo,
        material: "far fa-file-alt"
    },
    CreateCategory: {
        create: global.save,
        cancel: global.cancel
    },
    EditCategory: {
        save: global.save,
        cancel: global.cancel,
        delete: global.delete
    },
    ComponentForm: {
        component: "fas fa-cube"
    },
    ComponentsAdmin: {
        add: global.add,
        component: "fas fa-cube",
        edit: "fas fa-wrench",
        goTo: global.goTo
    },
    CreateComponent: {
        add: global.save,
        cancel: global.cancel
    },
    EditComponent: {
        save: global.save,
        cancel: global.cancel,
        delete: global.delete
    },
    CreateMenuItem: {
        create: global.add,
        cancel: global.cancel
    },
    EditMenuItem: {
        save: global.save,
        cancel: global.cancel,
        delete: global.delete
    },
    MenuAdminItem: {
        up: "fa fa-angle-up", //"la la-arrow-up", // "la la-caret-up", "la la-angle-up",
        down: "fa fa-angle-down", //"la la-arrow-up" // "la la-caret-down", "la la-angle-down"
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        blank: "far fa-file",
        edit: "fas fa-wrench",
        add: global.add,
        goTo: global.goTo,
        delete: global.delete
    },
    MenuItemForm: {
        link: "fas fa-link",
        css: "fab fa-css3-alt",
        icons: "fas fa-icons",
        search: global.search
    },
    MenuItemsAdmin: {
        add: global.add
    },
    ProfileRoles: {
        plus: "fas fa-plus",
        minus: "fas fa-minus"
    },
    RolesPage: {
        users: "fas fa-user-friends"
    },
    RolesPermissions: {
        save: global.save,
        reset: global.refresh
    },
    RoleUsers: {
        user: "fas fa-user",
        search: global.search
    },
    CustomCssAdmin: {
        save: global.save,
        reset: global.refresh,
        clean: "fas fa-snowplow"
    },
    MainSkinsAdmin: {
        upload: global.upload,
        preview: global.search,
        current: "fas fa-check",
        set: "fas fa-play",
        info: "fas fa-info",
        delete: global.delete
    },
    PartialSkinsAdmin: {
        upload: global.upload,
        delete: global.delete
    },
    SkinsAdmin: {
        main: "fas fa-user-astronaut",
        partial: "fas fa-puzzle-piece",
        customCss: "fab fa-css3-alt"
    },
    AdminMenu: {
        AdminInformation: "fas fa-info-circle",
        ConfigurationAdmin: "fas fa-sliders-h",
        MenuItemsAdmin: "fas fa-bars",
        CategoriesAdmin: "fas fa-folder",
        ComponentsAdmin: "fas fa-shapes",
        SkinsAdmin: "fas fa-palette",
        RolesPage: "fas fa-user-friends",
        RolesPermissions: "fas fa-user-shield",
        DeletedElements: "fas fa-trash",
        resetCache: global.refresh,
        systemTools: "fas fa-tools",
        ImagesCleaner: "fas fa-broom",
        CypherSecrets: global.key
    },
    ConfigurationAdmin: {
        search: global.search,
        question: "far fa-question-circle",
        save: global.save,
        reset: global.refresh,
        cancel: global.cancel
    },
    CypherSecrets: {
        key: global.key
    },
    DeletedElements: {
        trashBtn: global.delete
    },
    ImagesCleaner: {
        clean: global.delete,
        refresh: global.refresh
    },
    Captcha: {
        hand: "fas fa-hand-point-right",
        refresh: global.refresh
    },
    LoaderWait: {
        exclamation: "fas fa-exclamation-triangle"
    },
    SunEditor: {
        addImages: "fas fa-image"
    },
    ChangeEmail: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key,
        envelope: global.envelope,
        save: global.save
    },
    ChangeEmailResult: {
        success: "fas fa-check-circle",
        failed: "fas fa-exclamation-circle"
    },
    ChangePassword: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key,
        save: global.save
    },
    ResetPassword: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        envelope: global.envelope,
        key: global.key,
        save: global.save
    },
    ResetPasswordFailed: {
        failed: "fas fa-exclamation-circle"
    },
    ResetPasswordSetNew: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key
    },
    Activity: {
        comment: global.comment,
        material: "far fa-file-alt",
        publishDate: global.clock,
        category: global.category
    },
    Article: {
        delete: global.delete,
        publishDate: global.clock,
        comments: global.comment,
        user: global.user
    },
    ArticlesMultiCatPage: {
        add: global.add
    },
    ArticlesPage: {
        add: global.add
    },
    Login: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key,
        user: global.users,
        question: "far fa-question-circle"
    },
    LoginRegisterMenu: {
        signIn: "fas fa-sign-in-alt",
        register: "fas fa-user-plus"
    },
    Register: {
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key,
        user: global.users,
        envelope: global.envelope
    },
    RegisterEmailResult: {
        success: "fas fa-check-circle",
        failed: "fas fa-exclamation-circle"
    },
    UserMenu: {
        profile: "fas fa-user-circle",
        personal: "fas fa-address-card",
        admin: "fas fa-cog",
        signOut: "fas fa-sign-out-alt"
    },
    BlogMultiCatPage: {
        add: global.add
    },
    BlogPage: {
        add: global.add
    },
    Post: {
        deleted: global.delete,
        hidden: global.eyeSlash,
        readMore: "far fa-file-alt",
        comment: global.comment
    },
    Comment: {
        delete: global.delete,
        publishDate: global.clock
    },
    CreateComment: {
        send: global.send
    },
    EditComment: {
        save: global.save,
        cancel: global.cancel
    },
    Thread: {
        add: global.add
    },
    Topic: {
        deleted: global.delete,
        hidden: global.eyeSlash,
        comment: global.comment,
        category: global.category,
        publishDate: global.clock
    },
    Material: {
        delete: global.delete,
        restore: "fas fa-trash-restore",
        visits: global.eye,
        publishDate: global.clock
    },
    CreateMaterial: {
        save: global.save,
        cancel: global.cancel
    },
    EditMaterial: {
        save: global.save,
        cancel: global.cancel,
        delete: global.delete
    },
    MaterialForm: {
        tags: "fas fa-tags",
        category: global.category
    },
    Profile: {
        envelope: global.envelope,
        menu: "fas fa-ellipsis-v",
        roles: "fas fa-cog",
        ban: "fas fa-ban",
        unBan: "far fa-circle"
    },
    SendPrivateMessage: {
        send: global.send,
        cancel: global.cancel
    },
    Layout: {
        mainMenu: "la la-bars",
        secondMenu: "fa fa-clipboard-list",
        heart: "fas fa-heart",
        user: global.users
    },
    ChangeLink: {
        link: "fas fa-link",
        save: global.save
    },
    ChangeName: {
        save: global.save,
        eye: global.eye,
        eyeSlash: global.eyeSlash,
        key: global.key,
        user: global.users
    },
    EditInformation: {
        save: global.save
    },
    LoadPhoto: {
        upload: global.upload, // "far fa-user-circle"
        delete: global.delete
    },
    Sessions: {
        signOut: "fas fa-sign-out-alt",
        machine: "fas fa-desktop",
        clock: global.clock
    },
    SettingsMenu: {
        profile: "fas fa-address-card",
        user: global.users,
        key: global.key,
        envelope: global.envelope,
        link: "fas fa-link",
        photo: "fas fa-portrait",
        information: "fas fa-info-circle",
        sessions: "fas fa-ticket-alt",
        ban: "fas fa-ban"
    }
};
