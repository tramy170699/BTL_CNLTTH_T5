
<template>
    <v-app>
        <left-side-bar v-if="isLoggedIn"></left-side-bar>
        <v-toolbar v-if="isLoggedIn" color="primary" height="50px" dark app :clipped-left="$vuetify.breakpoint.lgAndUp" fixed>
            <v-toolbar-title style="width: 100%" class="ml-0 pl-3">
                <v-toolbar-side-icon  @click.stop="show"></v-toolbar-side-icon>
                <span class="hidden-sm-and-down">Quản lý khách sạn</span>
            </v-toolbar-title>
            <v-spacer></v-spacer>
            <v-menu offset-y>
                <v-btn icon large slot="activator" dark>
                    <v-avatar size="42px">
                        <img src="data:image/jpeg;base64,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" />
                    </v-avatar>
                </v-btn>
                <v-list>
                    <v-list-tile to="">
                        <v-list-tile-title>
                            {{user?user.Username:''}}
                        </v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile to="">
                        <v-list-tile-title>
                            Thông tin tài khoản
                        </v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile @click="logout()">
                        <v-list-tile-title>
                            Đăng xuất
                        </v-list-tile-title>
                    </v-list-tile>
                </v-list>
            </v-menu>
        </v-toolbar>
        <v-content>
            <v-container fluid fill-height grid-list-md style="padding: 10px">
                <v-layout justify-center>
                    <router-view></router-view>
                </v-layout>
                <vue-snotify></vue-snotify>
            </v-container>
        </v-content>
    </v-app>
</template>

<script lang="ts">
    import "vue-snotify/styles/material.css";
    import "vue-snotify/styles/material.css";
    import { Vue } from 'vue-property-decorator';
    import auth from '@/auth';
    import store from '@/store/store';
    import LeftSideBar from '@/components/Layout/LeftSideBar.vue';
    export default Vue.extend({
        name: 'App',
        components: { LeftSideBar },
        data() {
            return {
                clipped: false,
                drawer: true,
                fixed: true,
                items: [
                    {
                        icon: 'list',
                        title: 'BangGia',
                        link: '/banggia',
                    },
                    {
                        icon: 'list',
                        title: 'BoPhan',
                        link: '/bophan',
                    },
                    {
                        icon: 'list',
                        title: 'ChiTietKiemKe',
                        link: '/chitietkiemke',
                    },
                    {
                        icon: 'list',
                        title: 'ChiTietPhieuNhap',
                        link: '/chitietphieunhap',
                    },
                    {
                        icon: 'list',
                        title: 'DatDichVu',
                        link: '/datdichvu',
                    },
                    {
                        icon: 'list',
                        title: 'DatPhong',
                        link: '/datphong',
                    },
                    {
                        icon: 'list',
                        title: 'DichVu',
                        link: '/dichvu',
                    },
                    {
                        icon: 'list',
                        title: 'HoaDon',
                        link: '/hoadon',
                    },
                    {
                        icon: 'list',
                        title: 'KiemKe',
                        link: '/kiemke',
                    },
                    {
                        icon: 'list',
                        title: 'KhachHang',
                        link: '/khachhang',
                    },
                    {
                        icon: 'list',
                        title: 'LoaiPhong',
                        link: '/loaiphong',
                    },
                    {
                        icon: 'list',
                        title: 'NhanVien',
                        link: '/nhanvien',
                    },
                    {
                        icon: 'list',
                        title: 'PhieuNhapKho',
                        link: '/phieunhapkho',
                    },
                    {
                        icon: 'list',
                        title: 'Phong',
                        link: '/phong',
                    },
                    {
                        icon: 'list',
                        title: 'sysdiagrams',
                        link: '/sysdiagrams',
                    },
                    {
                        icon: 'list',
                        title: 'ThuePhong',
                        link: '/thuephong',
                    },
                    {
                        icon: 'list',
                        title: 'VatDung',
                        link: '/vatdung',
                    },
                    {
                        icon: 'list',
                        title: 'VatDungPhong',
                        link: '/vatdungphong',
                    },
                ],
                miniVariant: false,
                right: true,
                rightDrawer: false,
                title: 'Title',
                user: {} as any
            };
        },
        computed: {
            isLoggedIn(): boolean {
                this.user = store.state.user.Profile
                return store.state.user
                    && store.state.user.AccessToken
                    && store.state.user.AccessToken.IsAuthenticated;
            }
        },
        created() {
             this.$eventBus.$emit('getSoKhachDatPhong');
             this.$eventBus.$emit('getSoKhachDatDichVu');
        },
        methods: {
            show() {
                store.state.app.showLeftSideBar = !store.state.app.showLeftSideBar
            },
            logout() {
                auth.logout();
            },
        }
    });
</script>
<style>

    #ex5 .p1[data-count]:after {
        position: absolute;
        right: 61%;
        top: -15%;
        content: attr(data-count);
        font-size: 14px;
        padding: .2em;
        border-radius: 50%;
        line-height: 1em;
        color: white;
        background: rgba(255,0,0,.85);
        text-align: center;
        min-width: 1.5em;
    }

    .v-toolbar__content {
        padding-left: 0px
    }

    #appDrawer {
        overflow: hidden;
    }

        #appDrawer .v-list__tile__action, #appDrawer .v-list__group__header .v-list__group__header__prepend-icon {
            min-width: 35px;
        }

        #appDrawer .v-list__group__header .v-list__group__header__prepend-icon {
            padding-right: 0;
        }

        #appDrawer .v-list__group__items--no-action .v-list__tile {
            padding-left: 35px;
        }

    .drawer-menu--scroll {
        height: calc(100vh - 100px);
        overflow: auto;
    }

    #headermenu .v-text-field.v-text-field--solo .v-input__control {
        min-height: 35px;
    }
    /* #headermenu .v-toolbar__content {
        background-image: url('/static/img/banner1.png'); background-size: 100% 100%;
    } */
    .fade-enter-active, .fade-leave-active {
        transition: opacity .5s;
    }

    .fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
        opacity: 0;
    }
</style>