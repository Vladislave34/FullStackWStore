import {createSlice, PayloadAction} from "@reduxjs/toolkit";

type initialStateProps =  {
    currentPage: number;
    productVariantId: string;
};

const initialState: initialStateProps = {
    currentPage: 1,
    productVariantId: ''
}
const productVariantSlice = createSlice({
    name: "productVariant",
    initialState,
    reducers: {
        setProductVariantId: (state, action: PayloadAction<string>) => {
            state.productVariantId = action.payload;
        }
    }
});
export default productVariantSlice.reducer;
export const { setProductVariantId } = productVariantSlice.actions;