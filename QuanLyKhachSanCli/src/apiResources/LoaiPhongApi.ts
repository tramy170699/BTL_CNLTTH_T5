import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { LoaiPhong } from '@/models/LoaiPhong'
export interface LoaiPhongApiSearchParams extends Pagination {
    query?: String;
}
class LoaiPhongApi extends BaseApi {
    search(searchParams: LoaiPhongApiSearchParams): Promise<PaginatedResponse<LoaiPhong>> {

        return new Promise<PaginatedResponse<LoaiPhong>>((resolve: any, reject: any) => {
            HTTP.get('loaiphong', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<LoaiPhong> {
        return new Promise<LoaiPhong>((resolve: any, reject: any) => {
            HTTP.get('loaiphong/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, loaiPhong: LoaiPhong): Promise<LoaiPhong> {
        return new Promise<LoaiPhong>((resolve: any, reject: any) => {
            HTTP.put('loaiphong/' + id, 
                loaiPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(loaiPhong: LoaiPhong): Promise<LoaiPhong> {
        return new Promise<LoaiPhong>((resolve: any, reject: any) => {
            HTTP.post('loaiphong', 
                loaiPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<LoaiPhong> {
        return new Promise<LoaiPhong>((resolve: any, reject: any) => {
            HTTP.delete('loaiphong/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new LoaiPhongApi();
