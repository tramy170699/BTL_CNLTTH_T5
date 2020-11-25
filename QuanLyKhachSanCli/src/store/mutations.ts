
import {
    TOOGLE_LEFT_SIDE_BAR,
    TOOGLE_RIGHT_SIDE_BAR
} from './MUTATION_TYPES'

export default {
    
    updateTitle (state:any, text:any) {
        state.app.title = text
    },
    UPDATE_APP(state: any, app: any) {
        state.app = app
    },
    UPDATE_AUTH(state: any, auth: any) {
        state.auth = auth
    },

    UPDATE_USER(state: any, user: any) {
        state.user = user
       
    },

    [TOOGLE_LEFT_SIDE_BAR](state: any) {
        state.app.showLeftSideBar = !state.app.showLeftSideBar
    },

    [TOOGLE_RIGHT_SIDE_BAR](state: any) {
        state.app.showRightSideBar = !state.app.showRightSideBar
    },


    CLEAR_ALL_DATA(state: any) {
        state.user = {
            AccessToken: {
                IsAuthenticated: false,
                Token: null,
                UserName: null,
                RefreshToken: null,
                EffectiveTime: null,
                ExpiresTime: null
            },
            Profile: {
                Username: null,
                Userid: null,
                NhanVien: null
            }
        }
    }
}
