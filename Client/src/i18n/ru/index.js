import account from "./account";
import activities from "./activities";
import admin from "./admin";
import articles from "./articles";
import auth from "./auth";
import blog from "./blog";
import comments from "./comments";
import components from "./components";
import errors from "./errors";
import forum from "./forum";
import global from "./global";
import material from "./material";
import pages from "./pages";
import personal from "./personal";
import profile from "./profile";

export default {
	...account,
	...activities,
	...admin,
	...articles,
	...auth,
	...blog,
	...comments,
	...components,
	Errors: errors,
	...forum,
	...global,
	...material,
	...pages,
	...personal,
	...profile
};
