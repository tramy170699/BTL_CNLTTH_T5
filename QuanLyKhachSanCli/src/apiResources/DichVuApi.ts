import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { DichVu } from '@/models/DichVu'
export interface DichVuApiSearchParams extends Pagination {
    tenDichVu?: string;
    giaBan?: number;
}
class DichVuApi extends BaseApi {
    search(searchParams: DichVuApiSearchParams): Promise<PaginatedResponse<DichVu>> {

        return new Promise<PaginatedResponse<DichVu>>((resolve: any, reject: any) => {
            HTTP.get('dichvu', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<DichVu> {
        return new Promise<DichVu>((resolve: any, reject: any) => {
            HTTP.get('dichvu/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, dichVu: DichVu): Promise<DichVu> {
        return new Promise<DichVu>((resolve: any, reject: any) => {
            HTTP.put('dichvu/' + id, 
                dichVu
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(dichVu: DichVu): Promise<DichVu> {
        return new Promise<DichVu>((resolve: any, reject: any) => {
            HTTP.post('dichvu', 
                dichVu
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<DichVu> {
        return new Promise<DichVu>((resolve: any, reject: any) => {
            HTTP.delete('dichvu/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new DichVuApi();
