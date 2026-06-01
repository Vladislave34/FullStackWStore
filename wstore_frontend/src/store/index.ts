import {combineReducers} from "redux";
import {configureStore} from "@reduxjs/toolkit";

import {categoryApi} from "@/services/categoryService";
import {authApi} from "@/services/authService";
import authSlice from "@/store/reducers/authSlice";
import {storeApi} from "@/services/storeService";


const rootReducer = combineReducers({
    [categoryApi.reducerPath]: categoryApi.reducer,
    [authApi.reducerPath]: authApi.reducer,
    [storeApi.reducerPath]: storeApi.reducer,
    authSlice,
});

export const setupStore = () => {
    return configureStore({
        reducer: rootReducer,
        middleware: (getDefaultMiddleware) =>
            getDefaultMiddleware().concat(
                categoryApi.middleware,
                authApi.middleware,
                storeApi.middleware,
            ),
    });
};

export type RootState = ReturnType<typeof rootReducer>;
export type AppStore = ReturnType<typeof setupStore>;
export type AppDispatch = AppStore['dispatch'];