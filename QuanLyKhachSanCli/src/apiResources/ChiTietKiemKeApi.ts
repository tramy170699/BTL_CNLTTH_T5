import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { ChiTietKiemKe } from '@/models/ChiTietKiemKe'
export interface ChiTietKiemKeApiSearchParams extends Pagination {
    kiemKeID?:number;
    vatDungPhongID?: number;
    phongID?: number;
}
class ChiTietKiemKeApi extends BaseApi {
    search(searchParams: ChiTietKiemKeApiSearchParams): Promise<PaginatedResponse<ChiTietKiemKe>> {

        return new Promise<PaginatedResponse<ChiTietKiemKe>>((resolve: any, reject: any) => {
            HTTP.get('chitietkiemke', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    getds(searchParams: ChiTietKiemKeApiSearchParams): Promise<PaginatedResponse<ChiTietKiemKe>> {

        return new Promise<PaginatedResponse<ChiTietKiemKe>>((resolve: any, reject: any) => {
            HTTP.get('chitietkiemke/getds', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<ChiTietKiemKe> {
        return new Promise<ChiTietKiemKe>((resolve: any, reject: any) => {
            HTTP.get('chitietkiemke/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, chiTietKiemKe: ChiTietKiemKe): Promise<ChiTietKiemKe> {
        return new Promise<ChiTietKiemKe>((resolve: any, reject: any) => {
            HTTP.put('chitietkiemke/' + id, 
                chiTietKiemKe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    updateDanhSach(lstchiTietKiemKe: ChiTietKiemKe[]): Promise<PaginatedResponse<ChiTietKiemKe>> {
        return new Promise<PaginatedResponse<ChiTietKiemKe>>((resolve: any, reject: any) => {
            HTTP.put('chitietkiemke/capnhatdanhsach',
                lstchiTietKiemKe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(chiTietKiemKe: ChiTietKiemKe): Promise<ChiTietKiemKe> {
        return new Promise<ChiTietKiemKe>((resolve: any, reject: any) => {
            HTTP.post('chitietkiemke', 
                chiTietKiemKe
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<ChiTietKiemKe> {
        return new Promise<ChiTietKiemKe>((resolve: any, reject: any) => {
            HTTP.delete('chitietkiemke/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new ChiTietKiemKeApi();
