export default {
  Global: {
    dialog: {
      cancel: "Cancel",
      yes: "Yes",
      ok: "Ok"
    },
    btn: {
      save: "Save",
      cancel: "Cancel"
    },
    validation: {
      emailSig: "Incorrect email",
    },
    errorNotify: "Error",
    submitting: "Sending..."
  },
  LoginRegisterMenu: {
    enter: "Sign In",
    register: "Sign Up"
  },
  App: {
    loading: "Loading...",
    canNotConnectApi: "Can`t connect to API."
  },
  Captcha: {
    newMessageBtn: "Generate new captcha",
    waitMessage: "To generate a new token, you need to wait a bit, try after a while",
    enterToken: "Enter text from image",
    required: "@:Captcha.enterToken",
  },
  UserMenu: {
    profile: "Profile",
    yourAccount: "Account",
    adminPanel: "Admin Panel",
    exit: "Sign Out",
    logoutNotify: "You did logout",
  },
  Login: {
    title: "Sign In",
    nameOrEmail: "Login or email",
    password: "Password",
    entering: "Sign In...",
    enterBtn: "Sign In",
    forgotPassword: "Forget password?",
    doNotRemember: "Do not remember",
    enterSuccess: "You logged in",
    validation: {
      nameOrEmail: {
        required: "Enter login or email"
      },
      password: {
        required: "Enter password"
      },
    }
  },
  Register: {
    title: "Sign Up",
    userName: "Login",
    email: "Email",
    password: "Password",
    password2: "Repeat password",
    registerBtn: "@:Register.title",
    registering: "Sing Up...",
    emailSent: "An email has been sent to the address you supplied. Important! Please check your email for a message confirming your registration.",
    validation: {
      userName: {
        required: "Enter login",
        minLength: "Login must be at least 3 letters long",
        maxLength: `Login must be at most not ${config.DbColumnSizes.Users_UserName} characters`
      },
      email: {
        required: "Enter email",
        emailSig: "@:Global.validation.emailSig",
        maxLength: `Email must be at most not ${config.DbColumnSizes.Users_Email} characters`
      },
      password: {
        required: "Enter password",
        minLength: `Password must be at least ${config.PasswordValidation.MinLength} characters`,
        minDifferentChars: `Password must contain at least ${config.PasswordValidation.MinDifferentChars} different characters`
      },
      password2: {
        equals: "Passwords must match"
      }
    }
  },
  RegisterEmailResult: {
    title: "Confirm Email",
    success: "Your email was confirmed.",
    error: "Something went wrong.",
    enter: "Sign In"
  },
  CreateEditComment: {
    required: "Enter message",
    htmlTextSizeOrHasImage: "Minimum text length - 5",
    spamProtectionMessage: "You can't send messages that often. Please, wait."
  },
  CreateMaterial: {
    title: "Add material",
    sendBtn: "Post",
    successNotify: "Material successfully added"
  },
  EditMaterial: {
    title: "Edit material",
    saveBtn: "Save",
    successNotify: "Material successfully saved",
  },
  MaterialForm: {
    name: "Name (eng)",
    title: "Header",
    description: "Short description",
    tags: "Tags",
    selectCategory: "Select category",
    category: "Category: {0}",
    validation: {
      name: {
        allowedChars: "Name must be contain only english letters, numbers and character '-'",
        numberNotAllowed: "Material name can`t be number",
        minLength: "Minimal name length - 3",
        maxLength: `Maximal name length - ${config.DbColumnSizes.Materials_Name}`,
      },
      title: {
        required: "Enter header",
        minLength: "Minimal header length - 3",
        maxLength: `Maximal header length - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Enter text",
        htmlTextSizeOrHasImage: "Minimal text length - 5",
      },
      description: {
        maxLength: "Maximal length " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Selected category"
      }
    }
  },
  CreateEditMaterial: {
    titleField: "Header",
    addTitle: "Add material",
    editTitle: "Edit text: {0}",
    description: "Short description",
    tags: "Tags",
    sendBtn: "Send",
    selectCategory: "Select category",
    category: "Category: {0}",
    successNotify: "Material successfully added",
    spamProtectionNotify: "You can't create material that often. Please, wait.",
    validation: {
      title: {
        required: "Enter header",
        minLength: "Minimal header length - 3",
        maxLength: `Maximal header length - ${config.DbColumnSizes.Materials_Title}`,
      },
      text: {
        required: "Enter text",
        htmlTextSizeOrHasImage: "Minimal text length - 5",
      },
      description: {
        maxLength: "Maximal length " + config.DbColumnSizes.Materials_Description
      },
      category: {
        required: "Selected category"
      }
    }
  },
  MyEditor: {
    uploadImages: "Upload image"
  },
  Material: {
    category: "category:",
    edit: "Edit",
    tags: "Tags:"
  },
  ReadComment: {
    edit: "edit",
    deleteDialogMessage: "Remove message?"
  },
  Post: {
    commentsCount: "comments",
    readMore: "Read more"
  },
  BlogPage: {
    newPostBtn: "New post"
  },
  SettingsMenu: {
    goToProfile: "View profile",
    changeEmail: "Change email",
    changePassword: "Change password",
    changeLink: "Change link",
    changeName: "Change login",
    changeYourInformation: "Change your information",
    changePhoto: "Change avatar",
    banedUsersList: "Banned users"
  },
  SettingsPage: {
    title: "Account"
  },
  SettingsPanel: {
    title: "@:SettingsPage.title"
  },
  ChangeName: {
    title: "Change login",
    successNotify: "Login changed",
    nameValidationInfo: "Login can contain letters, numbers, space and '-' character, and must be at least 3 characters long.",
    saveBtn: "@:Global.btn.save",
    name: "Login",
    password: "Password",
    validation: {
      password: {
        required: "Enter password"
      },
      name: {
        required: "Enter login",
        minLength: "Login must be at least 3 characters",
        allowedChars: "Only allowed characters are allowed.",
        nameInDb: "Login already used"
      }
    }
  },
  ChangeLink: {
    title: "Edit user Link",
    successNotify: "Link edited",
    link: "Link",
    linkValidationInfo: "Link must consist of at least 3 characters 'a-z', 'A-Z', '-', '0-9'. And contain at least one letter.",
    saveBtn: "@:Global.btn.save",
    validation: {
      minLength: "Link must be at least 3 characters",
      allowedChars: "Allowed only english letters and numbers",
      numberNotAllow: "Link must be contain letters",
      linkInDb: "This link already used"
    }
  },


  ForumPanel: {
    newTopics: "New topics",
    sections: "Sections"
  },
  Topic: {
    lastFrom: "Last from"
  },

  Thread: {
    newTopicBtn: "New topic",
    topic: "Topic",
    last: "Last"
  },
  NewTopics: {
    titleStart: "New topics",
    newTopicBtn: "@:Thread.newTopicBtn",
    topic: "@:Thread.topic",
    last: "@:Thread.last"
  },
  Profile: {
    roles: "Groups",
    sendPrivateMessageBtn: "Private message",
    banBtn: "Ban",
    unBanBtn: "Unban",
    banNotify: "User {0} can`t now write you",
    unBanNotify: "User {0} can now write you"
  },
  SendPrivateMessage: {
    title: "Private message",
    titleStart: "Write private message",
    sendBtn: "Send",
    successNotify: "Message is successfully sent to the user {0}",
    sendErrorNotify: "Message sending failed. Server error.",
    sendSpamProtectionNotify: "You can't send private messages that often. Please, wait.",
  },
  MyBanList: {
    title: "Banned users"
  },
  LoadPhoto: {
    title: "Change user`s avatar",
    resetBtn: "Reset user`s avatar",
    uploadNewPhotoBtn: "Upload avatar",
    avatarDeletedSuccessNotify: "Avatar was deleted",
    avatarChangedSuccessNotify: "Avatar was changed"
  },
  EditInformation: {
    title: "Edit information",
    label: "Information about you on your profile page.",
    successNotify: "Information successfully saved",
    save: "@:Global.btn.save"
  },

  Error404: {
    title: "Error 404",
    info: "Sorry, page not found...",
    goBackBtn: "Return back"
  },
};
