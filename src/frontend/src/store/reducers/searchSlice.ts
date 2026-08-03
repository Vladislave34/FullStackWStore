import ICategory from "@/models/ICategory";
import IUser from "@/models/auth/IUser";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";


type initialStateProps = {
    category?: ICategory  | null
    query: string
}

const initialState : initialStateProps = {
    category: null,
    query: "",
}

const searchSlice = createSlice({
    name: "search",
    initialState,
    reducers: {
        setCategoryForSearch: (state, action: PayloadAction<ICategory  | undefined>) => {
            state.category = action.payload;
        },
        setQuery: (state, action: PayloadAction<string>) => {
            state.query = action.payload;
        },
        clearData: (state) => {
            state.query = "";
            state.category = null;
        }
    }
})
export default searchSlice.reducer;
export const {setCategoryForSearch, clearData, setQuery} = searchSlice.actions;