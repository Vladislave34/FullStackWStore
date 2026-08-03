import {createSlice, PayloadAction} from "@reduxjs/toolkit";


type cartItemProps = {
    cartItemIds: string[];
}
const initialState : cartItemProps = {
    cartItemIds: [],
}

const cartItemSlice = createSlice({
    name: 'cartItem',
    initialState,
    reducers: {
        addToOrder: (state, action: PayloadAction<string>) => {
            state.cartItemIds.push(action.payload);
        },
        deleteFromOrder: (state, action: PayloadAction<string>) => {
            state.cartItemIds = state.cartItemIds.filter(x=>x !== action.payload);
        },
        clearOrderState(state) {
            state.cartItemIds = [];
        }
    }
})

export const {addToOrder, deleteFromOrder, clearOrderState} = cartItemSlice.actions;
export default cartItemSlice.reducer;