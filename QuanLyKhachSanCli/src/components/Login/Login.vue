<template>
    <v-img :aspect-ratio="16/7"
           height="100%"
           width="100%"
           src="https://360nhadat.com/wp-content/uploads/2018/01/Condotel-Phu-Quoc-5.jpg"
           gradient="to top right, rgba(100,115,201,.33), rgba(25,32,72,.7)">
        <v-container fluid fill-height style="padding: 0px">
            <v-layout align-center wrap justify-center class="login_page_wrapper" style="position:relative; top:-35px">
                <v-flex xs12 class="text-xs-center" style="margin-top: 60px">
                    <h1 class="white--text mt-2">
                        <span>
                            Đăng nhập hệ thống
                        </span>
                    </h1>
                </v-flex>
                <v-flex xs12 sm8 md4 style="position:relative;top:-30px;">
                    <v-card class="elevation-12">
                        <v-card-text>
                            <p v-if="msg" class="error--text text-xs-center mb-4">{{msg}}</p>
                            <v-form>
                                <v-text-field v-model="credentials.username" @keypress.enter="submit" prepend-icon="person" name="login" label="Tài khoản" type="text" required></v-text-field>
                                <v-text-field v-model="credentials.password" @keypress.enter="submit" prepend-icon="lock"
                                              :append-icon="show1 ? 'visibility_off' : 'visibility'"
                                              :type="show1 ? 'text' : 'password'" @click:append="show1 = !show1"
                                              name="password" label="Mật khẩu" id="password" required></v-text-field>
                            </v-form>
                            <v-btn block color="primary" :loading="loggingIn" :disabled="loggingIn" @click="submit">Đăng nhập</v-btn>
                           
                        </v-card-text>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-container>
    </v-img>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import auth from '@/auth';
    import EventBus from '@/EventBus';
    import store from '@/store/store';
import { debug } from 'util';

    export default Vue.extend({
        components: {  },
        data() {
            return {
                show1: false,
                credentials: {
                    username: '',
                    remember: false,
                    password: ''
                },
                loggingIn: false,
                msg: ''
            };
        },
        methods: {
            submit() {
                debugger

                this.loggingIn = true;
                auth.login(this.credentials, 'nhanvien')
                    .then((response: any) => {
                        this.loggingIn = false;
                        if (!store.state.user.AccessToken.IsAuthenticated) {
                            this.$snotify.error('Sai tên đăng nhập hoặc mật khẩu.');
                        }
                        else {
                        }
                    })
                    .catch((errorResponse: any) => {
                        this.loggingIn = false;
                        if (errorResponse !== 'Login failed!') {
                            this.msg = 'Có lỗi xảy ra.';
                        }
                        else {
                            this.msg = 'Tài khoản hoặc mật khẩu không chính xác.'
                        }
                        this.$snotify.error(this.msg);
                    });
            }
        }
    });
</script>
