<template>
    <v-dialog v-model="dialog" width="500" persistent scrollable>
        <v-container pa-0 grid-list-md>
            <v-card>
                <v-card-title class="primary white--text" style="padding: 5px 15px 5px 15px">
                    <h3>{{isUpdate ? 'Cập nhật nhân viên' : 'Thêm mới nhân viên'}}</h3>
                    <v-spacer></v-spacer>
                    <v-btn class="white--text ma-0" small @click.native="dialog = false" icon dark><v-icon>close</v-icon></v-btn>
                </v-card-title>

                <v-card-text>
                    <v-form scope="frmAddEdit">
                        <v-layout row wrap>
                            <v-flex xs6>
                                <v-text-field v-model="nhanVien.TenNhanVien"
                                              label="Tên nhân viên"
                                              type="text"
                                              :error-messages="errors.collect('Tên nhân viên', 'frmAddEdit')"
                                              v-validate="'required'"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Tên nhân viên"
                                              clearable></v-text-field>
                                <v-spacer></v-spacer>
                            </v-flex>

                            <v-flex xs6>
                                <v-text-field 
                                              v-model="nhanVien.SoDienThoai"
                                              label="Số điện thoại"
                                              type="number"
                                              :error-messages="errors.collect('Số điện thoại', 'frmAddEdit')"
                                              
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Số điện thoại"
                                              v-validate="{ required: true, max:10, min:10 }"
                                              clearable></v-text-field>
                            </v-flex>

                            <v-flex xs6>
                                <v-autocomplete v-model="nhanVien.BoPhanID"
                                                :items="dsBoPhan"
                                                :loading="dsBoPhanLoading"
                                                :search-input.sync="searchBoPhan"
                                                item-text="TenBoPhan"
                                                item-value="BoPhanID"
                                                label="Tên bộ phận"
                                                placeholder="Nhập tên bộ phận"
                                                :error-messages="errors.collect('BoPhanID', 'frmAddEdit')"
                                                v-validate="''"
                                                data-vv-scope="frmAddEdit"
                                                data-vv-name="BoPhanID"
                                                clearable></v-autocomplete>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field v-model="users.UserName"
                                              label="Tên đăng nhập"
                                              type="text"
                                              :error-messages="errors.collect('Tên đăng nhập', 'frmAddEdit')"
                                              v-validate="'required'"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Tên đăng nhập"
                                              :disabled="isUpdate"
                                              clearable></v-text-field>
                            </v-flex>
                            <v-flex xs6>
                                <v-text-field v-model="users.Password"
                                              label="Mật khẩu"
                                              :error-messages="errors.collect('Mật khẩu', 'frmAddEdit')"
                                              v-validate="{ required: true, min: 8 }"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Mật khẩu"
                                              v-show="!isUpdate"
                                              ></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-form>
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn class="primary" :disabled="loading" :loading="loading" @click.native="save">{{isUpdate?'Cập nhật':'Thêm mới'}}</v-btn>
                </v-card-actions>
            </v-card>
        </v-container>
    </v-dialog>
</template>

<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import NhanVienApi, { NhanVienApiSearchParams } from '@/apiResources/NhanVienApi';
    import { NhanVien } from '@/models/NhanVien';
    import { Users } from '@/models/Users';
    import { BoPhan } from '@/models/BoPhan';
    import BoPhanApi, { BoPhanApiSearchParams } from '@/apiResources/BoPhanApi';
    import { Validator } from 'vee-validate';
    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },

        components: {},
        data() {
            return {
                dialog: false,
                isUpdate: false,
                nhanVien: {
                    Users: {} as Users
                } as NhanVien,
                users: {} as Users,
                dsBoPhan: [] as BoPhan[],
                dsBoPhanLoading: false,
                searchBoPhan: '',
                searchParamsBoPhan: { includeEntities: true } as BoPhanApiSearchParams,
                loading: false,
                searchParamsNhanVien: {} as NhanVienApiSearchParams,
            }

        },
        created() {
            this.getDSBoPhan();
        },
        methods: {
            show(isUpdate: boolean, item: any) {
                this.dialog = true;
                this.isUpdate = isUpdate;
                this.nhanVien = item;
                if (isUpdate == true) {
                    this.getDataFromApi(item.NhanVienID);
                }
                else
                    this.users = {} as Users
            },
            getDataFromApi(id: number): void {
                NhanVienApi.detail(id).then(res => {
                    this.nhanVien = res;
                    this.users = res.Users as any;
                });
            },
            save(): void {
               
                this.$validator.validateAll('frmAddEdit').then((res) => {
                    if (res) {
                        this.nhanVien.Users = this.users;
                        if (this.isUpdate) {
                            this.loading = true;
                            NhanVienApi.update(this.nhanVien.NhanVienID, this.nhanVien).then(res => {
                                this.loading = false;
                                this.dialog = false;
                                this.$snotify.success('Cập nhật thành công!');
                                this.$emit("getLaiDanhSach");
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            NhanVienApi.insert(this.nhanVien).then(res => {
                                this.nhanVien = res;
                                this.isUpdate = true;
                                this.loading = false;
                                this.$snotify.success('Thêm mới thành công!');
                                this.dialog = false
                                this.$emit("getLaiDanhSach");
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },

            getDSBoPhan(): void {
                BoPhanApi.search(this.searchParamsBoPhan).then(res => {
                    this.dsBoPhan = res.Data;
                });
            },
        }
    });
</script>

