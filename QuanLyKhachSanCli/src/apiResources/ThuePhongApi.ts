import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { ThuePhong } from '@/models/ThuePhong'
export interface ThuePhongApiSearchParams extends Pagination {
    phongID?:number;
    hoaDonID?:number;
    bangGiaID?:number;
}
class ThuePhongApi extends BaseApi {
    search(searchParams: ThuePhongApiSearchParams): Promise<PaginatedResponse<ThuePhong>> {

        return new Promise<PaginatedResponse<ThuePhong>>((resolve: any, reject: any) => {
            HTTP.get('thuephong', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<ThuePhong> {
        return new Promise<ThuePhong>((resolve: any, reject: any) => {
            HTTP.get('thuephong/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, thuePhong: ThuePhong): Promise<ThuePhong> {
        return new Promise<ThuePhong>((resolve: any, reject: any) => {
            HTTP.put('thuephong/' + id, 
                thuePhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(thuePhong: ThuePhong): Promise<ThuePhong> {
        return new Promise<ThuePhong>((resolve: any, reject: any) => {
            HTTP.post('thuephong', 
                thuePhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<ThuePhong> {
        return new Promise<ThuePhong>((resolve: any, reject: any) => {
            HTTP.delete('thuephong/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new ThuePhongApi();
