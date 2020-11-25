import Vue from 'vue';
import Router, { Route } from 'vue-router';

import DanhSachNhanVien from './components/NhanVien/DanhSachNhanVien.vue';


import Login from './components/Login/Login.vue';
import store from './store/store';
import { HTTP } from './HTTPServices';



Vue.use(Router);
export default new Router({
    routes: [
        {
            path: '/',
            name: 'login',
            component: Login,
        }, 
        {
            path: '/nhanvien',
            name: 'nhanVien',
            component: DanhSachNhanVien,
            beforeEnter: guardRoute
        },
        
              
    ],
});


function guardRoute(to: Route, from: Route, next: any): void {
    const isAuthenticated = store.state.user && store.state.user.AccessToken ? store.state.user.AccessToken.IsAuthenticated : false;
    if (!isAuthenticated) {
        next({
            path: '/login',
            query: {
                redirect: to.fullPath
            }
        });
    } else {
        HTTP.get('/auth/validate-token/')
            .then(response => {
                next();
            })
            .catch(e => {
                store.commit('CLEAR_ALL_DATA');
                next({
                    path: '/login',
                    query: {
                        redirect: to.fullPath
                    }
                });
            });
    }
}