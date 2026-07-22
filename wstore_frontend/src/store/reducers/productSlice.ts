import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import IUser from "@/models/auth/IUser";

type initialStateProps =  {
    currentPage: number;
    countForStore: number;
}

const initialState: initialStateProps = {
    currentPage: 1,
    countForStore: 0,
}

const productSlice = createSlice({
    name: "product",
    initialState,
    reducers: {
        setCurrentPage: (state, action: PayloadAction<number>) => {
            state.currentPage = action.payload;
        },
        setCountForStore: (state, action: PayloadAction<number>) => {
            state.countForStore = action.payload;
        }
    }
})
export default productSlice.reducer;
export const {setCurrentPage, setCountForStore}  = productSlice.actions;