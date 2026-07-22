import ICategory from "@/models/ICategory";
import IUser from "@/models/auth/IUser";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import IGenderModel from "@/models/gender/IGenderModel";


type initialStateProps = {
    category?: ICategory  | null
    gender?: IGenderModel | null
    hasSale?: boolean | null
    query: string

}

const initialState : initialStateProps = {
    category: null,
    gender: null,
    hasSale: null,
    query: "",
}

const filterSlice = createSlice({
    name: "search",
    initialState,
    reducers: {
        setCategory: (state, action: PayloadAction<ICategory  | undefined>) => {
            state.category = action.payload;
        },
        setGender: (state, action: PayloadAction<IGenderModel  | undefined>) => {
            state.gender = action.payload;
        },
        setHasSale: (state, action: PayloadAction<boolean | undefined>) => {
            state.hasSale = action.payload;
        },
        setQuery: (state, action: PayloadAction<string>) => {
            state.query = action.payload;
        },
        clearData: (state) => {
            state.query = "";
            state.category = null;
            state.hasSale = null;
            state.gender = null;
        }
    }
})
export default filterSlice.reducer;
export const {setCategory, setGender, setHasSale, setQuery, clearData} = filterSlice.actions;