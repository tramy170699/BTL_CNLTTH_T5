import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { BoPhan } from '@/models/BoPhan'
export interface BoPhanApiSearchParams extends Pagination {
}
class BoPhanApi extends BaseApi {
    search(searchParams: BoPhanApiSearchParams): Promise<PaginatedResponse<BoPhan>> {

        return new Promise<PaginatedResponse<BoPhan>>((resolve: any, reject: any) => {
            HTTP.get('bophan', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<BoPhan> {
        return new Promise<BoPhan>((resolve: any, reject: any) => {
            HTTP.get('bophan/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, boPhan: BoPhan): Promise<BoPhan> {
        return new Promise<BoPhan>((resolve: any, reject: any) => {
            HTTP.put('bophan/' + id, 
                boPhan
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(boPhan: BoPhan): Promise<BoPhan> {
        return new Promise<BoPhan>((resolve: any, reject: any) => {
            HTTP.post('bophan', 
                boPhan
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<BoPhan> {
        return new Promise<BoPhan>((resolve: any, reject: any) => {
            HTTP.delete('bophan/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new BoPhanApi();
