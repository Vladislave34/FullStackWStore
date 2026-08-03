"use client";

import { ThemeProvider } from "next-themes";

import { ReactNode } from "react";

import {DeviceProvider} from "@/providers/deviceProvider";
import {setupStore} from "@/store";
import {Provider} from "react-redux";
import {GoogleOAuthProvider} from "@react-oauth/google";
// import {NextIntlClientProvider} from "next-intl";
// import {hasLocale} from "next-intl";
// import {routing} from "@/i18n/routing";
// import {notFound} from "next/navigation";
const store = setupStore();
export default  function MainProvider({
                                     children,

                                 }: {
    children: ReactNode;


}) {

    return (
        <ThemeProvider attribute="class" defaultTheme="system" enableSystem disableTransitionOnChange >
            <DeviceProvider>
                <Provider store={store}>
                    <GoogleOAuthProvider clientId={process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID!}>

                            {children}

                        </GoogleOAuthProvider>
                </Provider>
            </DeviceProvider>


        </ThemeProvider>
    );
}