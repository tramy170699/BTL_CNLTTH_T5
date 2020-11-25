import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { VatDung } from '@/models/VatDung'
export interface VatDungApiSearchParams extends Pagination {
}
class VatDungApi extends BaseApi {
    search(searchParams: VatDungApiSearchParams): Promise<PaginatedResponse<VatDung>> {

        return new Promise<PaginatedResponse<VatDung>>((resolve: any, reject: any) => {
            HTTP.get('vatdung', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<VatDung> {
        return new Promise<VatDung>((resolve: any, reject: any) => {
            HTTP.get('vatdung/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, vatDung: VatDung): Promise<VatDung> {
        return new Promise<VatDung>((resolve: any, reject: any) => {
            HTTP.put('vatdung/' + id, 
                vatDung
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(vatDung: VatDung): Promise<VatDung> {
        return new Promise<VatDung>((resolve: any, reject: any) => {
            HTTP.post('vatdung', 
                vatDung
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<VatDung> {
        return new Promise<VatDung>((resolve: any, reject: any) => {
            HTTP.delete('vatdung/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new VatDungApi();
