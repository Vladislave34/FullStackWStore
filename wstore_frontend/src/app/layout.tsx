import "./globals.css";
import { ReactNode } from "react";
import { Inter } from "next/font/google";
import Header from "@/components/Header";
import Provider from "@/providers/provider";

import type {Metadata} from "next";
import Footer from "@/components/Footer";
import Script from "next/script";
import {getLocale} from "@/utils/getLocale";
import {getMessages} from "next-intl/server";


export const metadata: Metadata = {
    title: "WStore",
    description: "Магазин одягу",
}

const inter = Inter({
    subsets: ["latin", "cyrillic"],
    weight: ["400", "500"],
});

export default async function RootLayout({
                                       children,
                                   }: {
    children: ReactNode;
}) {
    const locale = await getLocale();
    const messages = await getMessages();
    return (
        <html lang="en" suppressHydrationWarning>
        <body
            style={{
                background: "var(--bg)",
            }}
            className={inter.className}>

        <Provider locale={locale!} messages={messages}  >
            <Header />
            <main className="w-full ">
                {children}
            </main>
            <Footer />
        </Provider>

        </body>

        </html>
    );
}