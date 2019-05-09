import auth from 'router/auth'
import account from 'router/account'
import misc from 'router/misc'
import personal from 'router/personal';
import admin from 'router/admin';
import ssr from 'router/ssr';
import site from './routesSite';

export const routes = [...auth,...account,...misc,...personal,...admin/*,...site,...ssr*/];


export const initRouter = function(router) {

};
