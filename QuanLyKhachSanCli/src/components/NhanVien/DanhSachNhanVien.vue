<template>
    <v-card width="100%" style="padding: 20px">
        <v-layout row wrap>
            <v-flex xs12>
                <h3>Danh khách nhân viên</h3>
            </v-flex>
            <v-flex xs12>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-layout nowrap>
                            <v-flex xs4>
                                <v-text-field label="Tìm kiếm"
                                              append-icon="search" ma-0 pa-0 hide-details
                                              placeholder="Nhập tên nhân viên, số điện thoại..."
                                              v-model="searchParamsNhanVien.query"
                                              @input="getDataFromApi(searchParamsNhanVien)"></v-text-field>
                            </v-flex>
                            <v-flex xs4>
                                <v-autocomplete v-model="searchParamsNhanVien.boPhanID"
                                                :items="dsBoPhan"
                                                item-text="TenBoPhan"
                                                item-value="BoPhanID"
                                                label="Bộ phận"
                                                placeholder="Chọn bộ phận"
                                                @input="getDataFromApi(searchParamsNhanVien)"
                                                clearable></v-autocomplete>
                            </v-flex>
                            <v-spacer></v-spacer>
                            <v-btn small color="primary" style="margin-top: auto" @click="showModalThemSua(false, {})">+ Thêm mới</v-btn>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsNhanVien"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsNhanVien"
                                      :total-items="searchParamsNhanVien.totalItems"
                                      :loading="loadingTable"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td>{{ props.index + 1}}</td>
                                <td>{{ props.item.TenNhanVien }}</td>
                                <td>{{ props.item.SoDienThoai }}</td>
                                <td>{{ props.item.Users? props.item.Users.UserName : '' }}</td>
                                <td>{{ props.item.BoPhan ? props.item.BoPhan.TenBoPhan : "" }}</td>
                                <td class="text-xs-center" style="width:80px;">
                                    <v-btn flat icon small @click="showModalThemSua(true, props.item)" class="ma-0">
                                        <v-icon small>edit</v-icon>
                                    </v-btn>
                                    <v-btn flat color="red" icon small class="ma-0" @click="confirmDelete(props.item)">
                                        <v-icon small>delete</v-icon>
                                    </v-btn>
                                </td>
                                </td>
                            </template>
                        </v-data-table>
                    </v-flex>
                </v-layout>
            </v-flex>
            <v-dialog v-model="dialogConfirmDelete" max-width="290">
                <v-card>
                    <v-card-title class="headline">Xác nhận xóa</v-card-title>
                    <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                        <v-btn color="red darken-1" @click.native="deleteNhanVien" flat>Xác Nhận</v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <them-sua-nhan-vien ref="themSuaNhanVien" @getLaiDanhSach="getDataFromApi(searchParamsNhanVien)"></them-sua-nhan-vien>

        </v-layout>
    </v-card>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import NhanVienApi, { NhanVienApiSearchParams } from '@/apiResources/NhanVienApi';
    import { NhanVien } from '@/models/NhanVien';
    import BoPhanApi, { BoPhanApiSearchParams } from '@/apiResources/BoPhanApi';
    import { BoPhan } from '@/models/BoPhan';
    import ThemSuaNhanVien from './ThemSuaNhanVien.vue';

    export default Vue.extend({
        components: {
            ThemSuaNhanVien
        },
        data() {
            return {
                dsBoPhan: [] as BoPhan[],
                dsNhanVien: [] as NhanVien[],
                tableHeader: [
                    { text: 'STT', value: 'TenNhanVien', align: 'center', sortable: true },
                    { text: 'Tên nhân viên', value: 'TenNhanVien', align: 'center', sortable: true },
                    { text: 'Số điện thoại', value: 'SoDienThoai', align: 'center', sortable: true },
                    { text: 'Tên đăng nhập', value: 'TenDangNhap', align: 'center', sortable: true },
                    { text: 'Tên bộ phận', value: 'BoPhanID', align: 'center', sortable: true },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsNhanVien: { includeEntities: true, rowsPerPage: 10 } as NhanVienApiSearchParams,
                searchParamsBoPhan: { includeEntities: true, rowsPerPage: 0 } as BoPhanApiSearchParams,
                loadingTable: false,
                selectedNhanVien: {} as NhanVien,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDSBoPhan();
            this.getDataFromApi(this.searchParamsNhanVien);
        },
        methods: {
            getDataFromApi(searchParamsNhanVien: NhanVienApiSearchParams): void {
                this.loadingTable = true;
                NhanVienApi.search(searchParamsNhanVien).then(res => {
                    this.dsNhanVien = res.Data;
                    this.searchParamsNhanVien.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            showModalThemSua(isUpdate: boolean, item: any) { // refs gọi từ cha -=> con 
                (this.$refs.themSuaNhanVien as any).show(isUpdate, item); // gọi đến hàm show bên modal con
            },
            confirmDelete(nhanVien: NhanVien): void {
                this.selectedNhanVien = nhanVien;
                this.dialogConfirmDelete = true;
            },
            deleteNhanVien(): void {
                NhanVienApi.delete(this.selectedNhanVien.NhanVienID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsNhanVien);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
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

