<template>
    <v-navigation-drawer persistent :mini-variant="miniVariant"
                         width="250"
                         :clipped="$vuetify.breakpoint.lgAndUp"
                         id="appDrawer" style="position:fixed; top:0; left:0; overflow-y:auto;"
                         :value="showLeftSideBar" @input="toggleSidebar"
                         enable-resize-watcher fixed app>
        <v-list id="leftSideBar">
            <v-list dense expand>
                <template v-if="menus.length > 0" v-for="(item, i) in menus">
                    <!--group with subitems-->
                    <v-list-group v-if="item.items && item.show" :key="item.title" :prepend-icon="item.icon" no-action="no-action">
                        <v-list-tile slot="activator" ripple="ripple" v-if="item.title && item.show">
                            <v-list-tile-content>
                                <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                            </v-list-tile-content>
                        </v-list-tile>
                        <template v-for="(subItem, i) in item.items">
                            <!--sub group-->
                            <v-list-group v-if="subItem.items && subItem.show" :key="subItem.name" sub-group="sub-group">
                                <v-list-tile slot="activator" ripple="ripple" v-if="subItem.title && subItem.show">
                                    <v-list-tile-content>
                                        <v-list-tile-title>{{ subItem.title }}</v-list-tile-title>
                                    </v-list-tile-content>
                                </v-list-tile>
                            </v-list-group>
                            <!--child item-->
                            <v-list-tile v-else-if="subItem.show" :key="i" :to="subItem.link" ripple="ripple">
                                <v-list-tile-content>
                                    <v-list-tile-title><span>{{ subItem.title }}</span></v-list-tile-title>
                                </v-list-tile-content>
                                <v-list-tile-action v-if="subItem.action1">
                                    <div id="ex4">
                                        <span class="p1 fa-stack fa-2x has-badge" :data-count="tongSoKhachDatPhong">
                                        </span>
                                    </div>
                                    <!--<v-icon :class="[subItem.actionClass || 'success--text']">{{ subItem.action }}</v-icon>-->
                                </v-list-tile-action>
                                <v-list-tile-action v-if="subItem.action2">
                                    <div id="ex4">
                                        <span class="p1 fa-stack fa-2x has-badge" :data-count="tongSoKhachDatDichVu">
                                        </span>
                                    </div>
                                    <!--<v-icon :class="[subItem.actionClass || 'success--text']">{{ subItem.action }}</v-icon>-->
                                </v-list-tile-action>
                            </v-list-tile>
                        </template>
                    </v-list-group>
                    <!--top-level link-->
                    <v-list-tile v-else-if="item.show" :to="item.link" ripple="ripple" rel="noopener" :key="item.link">
                        <v-list-tile-action v-if="item.icon && item.show">
                            <v-icon>{{ item.icon }}</v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content v-if="item.title && item.show">
                            <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                        </v-list-tile-content>
                        <!-- <v-circle class="white--text pa-0 chip--x-small" v-if="item.badge" :color="item.color || 'primary'" disabled="disabled">{{ item.badge }}</v-circle> -->
                        <v-list-tile-action v-if="item.subAction && item.show">
                            <v-icon class="success--text">{{ item.subAction }}</v-icon>
                        </v-list-tile-action>
                    </v-list-tile>
                </template>
            </v-list>
        </v-list>
    </v-navigation-drawer>
</template>

<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import { HTTP } from '@/HTTPServices.ts';
    import * as MUTATION_TYPES from '../../store/MUTATION_TYPES'
    import store from '../../store/store'
    import DatPhongApi, { DatPhongApiSearchParams } from '@/apiResources/DatPhongApi';
    import { DatPhong } from '@/models/DatPhong';
    export default Vue.extend({
        name: 'LeftSideBar',
        components: {

        },
        data() {
            return {
                miniVariant: false,
                clipped: false,
                scrollSettings: {
                    maxScrollbarLength: 160
                },
                menus: [] as any,
                tongSoKhachDatPhong: 0 as any,
                tongSoKhachDatDichVu: 0 as any,
                lstNhomMxh: [] as any,
                boPhanID: store.state.user.Profile.NhanVien.BoPhanID
            }
        },
        created() {
            this.getMenu();
            this.$eventBus.$on('getSoKhachDatPhong', this.getSoKhachDatPhong);
            this.$eventBus.$on('getSoKhachDatDichVu', this.getSoKhachDatDichVu);
        },
        computed: {
            showLeftSideBar(): boolean {
                return store.state.app.showLeftSideBar
            },
        },
        methods: {
            toggleSidebar(val: any) {
                if (val !== store.state.app.showLeftSideBar) {
                    store.commit(MUTATION_TYPES.TOOGLE_LEFT_SIDE_BAR)
                }
            },
            getSoKhachDatPhong() {
                var searchParamsDatPhong = {} as DatPhongApiSearchParams;
                searchParamsDatPhong.laDatPhong = true;
                searchParamsDatPhong.TrangThai = 0;
                DatPhongApi.search(searchParamsDatPhong).then(res => {
                    this.tongSoKhachDatPhong = res.Data.length
                })
            },
            getSoKhachDatDichVu() {
                var searchParamsDatPhong = {} as DatPhongApiSearchParams;
                searchParamsDatPhong.laDatPhong = false;
                searchParamsDatPhong.TrangThai = 0;
                DatPhongApi.search(searchParamsDatPhong).then(res => {
                    this.tongSoKhachDatDichVu = res.Data.length
                })
            },
            getMenu() {
                this.menus = [{
                    title: 'Trang chủ',
                    icon: 'home',
                    link: '/',
                    show: true,
                    items: [
                        { title: 'Đặt phòng', link: '/khachdatphong', show: true },
                        { title: 'Đặt dịch vụ', link: '/khachdatdichvu', show: true },
                    ]
                },              
                {
                    title: 'Quản lý danh mục',
                    icon: 'reorder',
                    link: '/',
                    show: (this.boPhanID == 1002|| this.boPhanID==6) ? true: false,
                    items: [
                         { title: 'Danh sách nhân viên   ', link: '/nhanvien', show: (this.boPhanID == 1002|| this.boPhanID==6) ? true: false, },
                            ]
                },
                
                ]
            }
        }
    })
</script>

<style>
    #ex4 .p1[data-count]:after {
        position: absolute;
        right: 5%;
        top: 28%;
        content: attr(data-count);
        font-size: 12px;
        padding: .2em;
        border-radius: 50%;
        line-height: 1em;
        color: white;
        background: rgba(255,0,0,.85);
        text-align: center;
        min-width: 1.5em;
    }


    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon,
    #leftSideBar .v-list__tile__action {
        min-width: 26px;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon,
    #leftSideBar .v-list__tile__action {
        padding: 0 5px;
    }

    #leftSideBar .v-list__group__items .v-list__tile__title {
        padding: 0 10px 0 0;
    }

    #leftSideBar .v-list__tile {
        padding: 0;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__append-icon {
        padding: 0 5px;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__append-icon,
    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon {
        padding: 0 5px !important;
    }

    #header-logo .v-toolbar__content {
        padding: 0 !important;
    }

    .v-list--dense {
        padding-top: 0;
        padding-bottom: 4px;
        margin-top: -5px;
    }
</style>