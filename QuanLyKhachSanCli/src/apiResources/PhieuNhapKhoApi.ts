import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { PhieuNhapKho } from '@/models/PhieuNhapKho'
export interface PhieuNhapKhoApiSearchParams extends Pagination {
    phieuNhapID?: number;
    ngayBD?: Date;
    ngayKT?: Date;
}
class PhieuNhapKhoApi extends BaseApi {
    search(searchParams: PhieuNhapKhoApiSearchParams): Promise<PaginatedResponse<PhieuNhapKho>> {

        return new Promise<PaginatedResponse<PhieuNhapKho>>((resolve: any, reject: any) => {
            HTTP.get('phieunhapkho', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<PhieuNhapKho> {
        return new Promise<PhieuNhapKho>((resolve: any, reject: any) => {
            HTTP.get('phieunhapkho/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, phieuNhapKho: PhieuNhapKho): Promise<PhieuNhapKho> {
        return new Promise<PhieuNhapKho>((resolve: any, reject: any) => {
            HTTP.put('phieunhapkho/' + id, 
                phieuNhapKho
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(phieuNhapKho: PhieuNhapKho): Promise<PhieuNhapKho> {
        return new Promise<PhieuNhapKho>((resolve: any, reject: any) => {
            HTTP.post('phieunhapkho', 
                phieuNhapKho
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<PhieuNhapKho> {
        return new Promise<PhieuNhapKho>((resolve: any, reject: any) => {
            HTTP.delete('phieunhapkho/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new PhieuNhapKhoApi();
