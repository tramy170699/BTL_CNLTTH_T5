import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { NhanVien } from '@/models/NhanVien'
export interface NhanVienApiSearchParams extends Pagination {
    boPhanID?: number;
    query?: String;
}
class NhanVienApi extends BaseApi {
    search(searchParams: NhanVienApiSearchParams): Promise<PaginatedResponse<NhanVien>> {

        return new Promise<PaginatedResponse<NhanVien>>((resolve: any, reject: any) => {
            HTTP.get('nhanvien', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<NhanVien> {
        return new Promise<NhanVien>((resolve: any, reject: any) => {
            HTTP.get('nhanvien/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, nhanVien: NhanVien): Promise<NhanVien> {
        return new Promise<NhanVien>((resolve: any, reject: any) => {
            HTTP.put('nhanvien/' + id, 
                nhanVien
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(nhanVien: NhanVien): Promise<NhanVien> {
        return new Promise<NhanVien>((resolve: any, reject: any) => {
            HTTP.post('nhanvien', 
                nhanVien
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<NhanVien> {
        return new Promise<NhanVien>((resolve: any, reject: any) => {
            HTTP.delete('nhanvien/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new NhanVienApi();
