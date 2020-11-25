import { NhanVien } from "./NhanVien";


export interface Users {
    PasswordSalt: string;
    Active: boolean;
    UserId: number;
    UserName: string;
    Password: string;
    CreatedTime: Date;
    LastLogin: Date;
    Lang: string;
    NhanVien?: NhanVien[];
}
