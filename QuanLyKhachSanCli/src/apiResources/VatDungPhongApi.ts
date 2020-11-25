import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { VatDungPhong } from '@/models/VatDungPhong'
export interface VatDungPhongApiSearchParams extends Pagination {
    vatDungID?:number;
    phongID?:number;
}
class VatDungPhongApi extends BaseApi {
    search(searchParams: VatDungPhongApiSearchParams): Promise<PaginatedResponse<VatDungPhong>> {

        return new Promise<PaginatedResponse<VatDungPhong>>((resolve: any, reject: any) => {
            HTTP.get('vatdungphong', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<VatDungPhong> {
        return new Promise<VatDungPhong>((resolve: any, reject: any) => {
            HTTP.get('vatdungphong/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, vatDungPhong: VatDungPhong): Promise<VatDungPhong> {
        return new Promise<VatDungPhong>((resolve: any, reject: any) => {
            HTTP.put('vatdungphong/' + id, 
                vatDungPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(vatDungPhong: VatDungPhong): Promise<VatDungPhong> {
        return new Promise<VatDungPhong>((resolve: any, reject: any) => {
            HTTP.post('vatdungphong', 
                vatDungPhong
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<VatDungPhong> {
        return new Promise<VatDungPhong>((resolve: any, reject: any) => {
            HTTP.delete('vatdungphong/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new VatDungPhongApi();
