import {combineReducers} from "redux";
import {configureStore} from "@reduxjs/toolkit";

import {categoryApi} from "@/services/categoryService";
import {authApi} from "@/services/authService";
import authSlice from "@/store/reducers/authSlice";
import {storeApi} from "@/services/storeService";
import {productApi} from "@/services/productService";
import productSlice from "@/store/reducers/productSlice";
import searchSlice from "@/store/reducers/searchSlice";
import {colorApi} from "@/services/colorService";
import {sizeApi} from "@/services/sizeService";
import {productVariantApi} from "@/services/productVariantService";
import productVariantSlice from "@/store/reducers/productVariantSlice";
import {genderApi} from "@/services/genderService";
import {saleApi} from "@/services/saleService";
import {favouriteApi} from "@/services/favouriteService";
import {cartApi} from "@/services/cartService";
import {cartItemApi} from "@/services/cartItemService";
import cartItemSlice from "@/store/reducers/cartItemSlice";
import {paymentApi} from "@/services/paymentService";
import {addressApi} from "@/services/addressService";
import {orderApi} from "@/services/orderService";
import filterSlice from "@/store/reducers/filterSlice";
import {statisticsApi} from "@/services/statisticsService";


const rootReducer = combineReducers({
    [categoryApi.reducerPath]: categoryApi.reducer,
    [authApi.reducerPath]: authApi.reducer,
    [storeApi.reducerPath]: storeApi.reducer,
    [productApi.reducerPath]: productApi.reducer,
    [colorApi.reducerPath]: colorApi.reducer,
    [sizeApi.reducerPath]: sizeApi.reducer,
    [productVariantApi.reducerPath]: productVariantApi.reducer,
    [genderApi.reducerPath]: genderApi.reducer,
    [saleApi.reducerPath]: saleApi.reducer,
    [favouriteApi.reducerPath]: favouriteApi.reducer,
    [cartApi.reducerPath]: cartApi.reducer,
    [cartItemApi.reducerPath]: cartItemApi.reducer,
    [paymentApi.reducerPath]: paymentApi.reducer,
    [addressApi.reducerPath]: addressApi.reducer,
    [orderApi.reducerPath]: orderApi.reducer,
    [statisticsApi.reducerPath]: statisticsApi.reducer,
    authSlice,
    productSlice,
    searchSlice,
    productVariantSlice,
    cartItemSlice,
    filterSlice,
});

export const setupStore = () => {
    return configureStore({
        reducer: rootReducer,
        middleware: (getDefaultMiddleware) =>
            getDefaultMiddleware().concat(
                categoryApi.middleware,
                authApi.middleware,
                storeApi.middleware,
                productApi.middleware,
                colorApi.middleware,
                sizeApi.middleware,
                productVariantApi.middleware,
                genderApi.middleware,
                saleApi.middleware,
                favouriteApi.middleware,
                cartApi.middleware,
                cartItemApi.middleware,
                paymentApi.middleware,
                addressApi.middleware,
                orderApi.middleware,
                statisticsApi.middleware,
            ),
    });
};

export type RootState = ReturnType<typeof rootReducer>;
export type AppStore = ReturnType<typeof setupStore>;
export type AppDispatch = AppStore['dispatch'];