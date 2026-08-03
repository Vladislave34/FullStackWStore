import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import { jwtDecode } from "jwt-decode";
import type IUser from "../../models/auth/IUser.ts";
import type IJwtResponse from "@/models/auth/IJwtResponse";

interface AuthState {
    user: IUser | null;
    accessToken: string | null;
    refreshToken: string | null;
}

const getUserFromToken = (token: string): IUser | null => {
    try {
        if (!token) return null;
        return jwtDecode<IUser>(token) ?? null;
    } catch (err) {
        console.error("Invalid token:", err);
        return null;
    }
};

export const isTokenExpired = (token: string): boolean => {
    try {
        const { exp } = jwtDecode<{ exp: number }>(token);
        return exp * 1000 < Date.now();
    } catch {
        return true;
    }
};


const getInitialState = (): AuthState => {
    if (typeof window === "undefined") {
        return { user: null, accessToken: null, refreshToken: null };
    }

    const accessToken = localStorage.getItem("accessToken");
    const refreshToken = localStorage.getItem("refreshToken");

    if (!accessToken || isTokenExpired(accessToken)) {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        return { user: null, accessToken: null, refreshToken: null };
    }

    return {
        user: getUserFromToken(accessToken),
        accessToken,
        refreshToken,
    };
};

const authSlice = createSlice({
    name: "auth",
    initialState: getInitialState,
    reducers: {
        loginSuccess: (state, action: PayloadAction<IJwtResponse>) => {
            const { accessToken, refreshToken } = action.payload;
            const user = getUserFromToken(accessToken);
            if (user) {
                state.user = user;
                state.accessToken = accessToken;
                state.refreshToken = refreshToken;
                localStorage.setItem("accessToken", accessToken);
                localStorage.setItem("refreshToken", refreshToken);
            }
        },
        refreshSuccess: (state, action: PayloadAction<IJwtResponse>) => {
            const { accessToken, refreshToken } = action.payload;
            const user = getUserFromToken(accessToken);
            if (user) {
                state.user = user;
                state.accessToken = accessToken;
                state.refreshToken = refreshToken;
                localStorage.setItem("accessToken", accessToken);
                localStorage.setItem("refreshToken", refreshToken);
            }
        },
        logout: (state) => {
            state.user = null;
            state.accessToken = null;
            state.refreshToken = null;
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
        },
    },
});

export const { loginSuccess, refreshSuccess, logout } = authSlice.actions;
export default authSlice.reducer;