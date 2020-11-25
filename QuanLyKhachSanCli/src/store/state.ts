import { NhanVien } from "../models/NhanVien";

export const STORAGE_KEY = 'insight-client-system';
let Cookies = require('js-cookie');

let syncedData = {
    app: {
        drawer: true,
        showLeftSideBar: true,
        showRightSideBar: false,
        searchText: '',
        searchTimestamp: null,
        title: ''
    },
    user: {
        AccessToken: {
            IsAuthenticated: false,
            Token: null,
            UserName: null,
            RefreshToken: null,
            EffectiveTime: null,
            ExpiresTime: null
        },
        Profile: {
            Userid: null,
            Username: null,
            NhanVien: {} as NhanVien
        }
    }
}


const notSyncedData = {
    app: {
        showLeftSideBar: true,
        showRightSideBar: false,
        searchText: '',
        searchTimestamp: null,
        title: ''
    }
}
// Sync with local storage.
let userLogin = Cookies.get(STORAGE_KEY)
if (userLogin) {
    var data = JSON.parse(userLogin)
    if (data === undefined) {
        data = ''
    } else {
        syncedData = data
    }
}

// Merge data and export it.
export const state = Object.assign(syncedData, notSyncedData)
