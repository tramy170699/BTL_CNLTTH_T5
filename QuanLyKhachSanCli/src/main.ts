import '@babel/polyfill'
import Vue from 'vue';
import './plugins/vuetify'
import App from './App.vue';
import router from './router';
import store from './store';
import './registerServiceWorker';
import VeeValidate, { Validator } from "vee-validate";
import Snotify from 'vue-snotify';
import { SnotifyService } from 'vue-snotify/SnotifyService';
import DateTimePicker from '@/components/Commons/DateTimePicker';
import CommonComponents from '@/components/Commons';
import EventBus from '@/EventBus';
import * as moment from 'moment';
Vue.use(DateTimePicker);
Vue.use(CommonComponents);

require('moment/locale/vi');

Vue.use(require('vue-moment'), {
    moment
});
Vue.use(VeeValidate);
Validator.localize('vi');
Vue.use(Snotify);
Vue.use(EventBus);

declare module 'vue/types/vue' {
    interface Vue {
        $snotify: SnotifyService,
        $moment: any,
        $validator: Validator,
        $eventBus: Vue,
    }
}
Vue.config.productionTip = false;
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
