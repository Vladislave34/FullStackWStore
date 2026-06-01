"use client";

import { ThemeProvider } from "next-themes";

import { ReactNode } from "react";

import {DeviceProvider} from "@/providers/deviceProvider";
import {setupStore} from "@/store";
import {Provider} from "react-redux";
import {GoogleOAuthProvider} from "@react-oauth/google";
import {NextIntlClientProvider} from "next-intl";
const store = setupStore();
export default function MainProvider({
                                     children,
    locale, messages
                                 }: {
    children: ReactNode;
    locale: string;
    messages: Record<string, unknown>;

}) {

    return (
        <ThemeProvider attribute="class" defaultTheme="system" enableSystem >
            <DeviceProvider>
                <Provider store={store}>
                    <GoogleOAuthProvider clientId={process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID!}>
                {/*<NextIntlClientProvider locale={locale} messages={messages}>*/}
                {children}
                {/*</NextIntlClientProvider >*/}
                        </GoogleOAuthProvider>
                </Provider>
            </DeviceProvider>


        </ThemeProvider>
    );
}