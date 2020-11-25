import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { DatPhong } from '@/models/DatPhong'
export interface DatPhongApiSearchParams extends Pagination {
    nhanVienID?:number;
    HoTen?: string;
    NgayBD?: Date;
    NgayKT?: Date;
    TrangThai?: number;
    laDatPhong?: boolean;
}
class DatPhongApi extends BaseApi {
    search(searchParams: DatPhongApiSearchParams): Promise<PaginatedResponse<DatPhong>> {

        return new Promise<PaginatedResponse<DatPhong>>((resolve: any, reject: any) => {
            HTTP.get('datphong', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<DatPhong> {
        return new Promise<DatPhong>((resolve: any, reject: any) => {
            HTTP.get('datphong/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, datPhong: DatPhong): Promise<DatPhong> {
        return new Promise<DatPhong>((resolve: any, reject: any) => {
            HTTP.put('datphong/' + id, 
                datPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(datPhong: DatPhong): Promise<DatPhong> {
        return new Promise<DatPhong>((resolve: any, reject: any) => {
            HTTP.post('datphong', 
                datPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<DatPhong> {
        return new Promise<DatPhong>((resolve: any, reject: any) => {
            HTTP.delete('datphong/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new DatPhongApi();
