import "../globals.css";
import { ReactNode } from "react";
import { Inter } from "next/font/google";
import Header from "@/app/[lng]/components/Header";
import Provider from "@/providers/provider";

import type {Metadata} from "next";
import Footer from "@/app/[lng]/components/Footer";
// import Script from "next/script";
// import {getLocale} from "@/utils/getLocale";
// import {getMessages} from "next-intl/server";
// import {hasLocale, NextIntlClientProvider} from "next-intl";
// import {routing} from "@/i18n/routing";
// import {notFound} from "next/navigation";
import { initServerI18next, getT, getResources, generateI18nStaticParams } from 'next-i18next/server'
import { I18nProvider } from 'next-i18next/client'
import i18nConfig from "../../../i18n.config";
import {headers} from "next/headers";



export const metadata: Metadata = {
    title: "WStore",
    description: "Магазин одягу",
}


initServerI18next(i18nConfig)
export async function generateStaticParams() {
    return generateI18nStaticParams()
}
export default async function LngLayout({
                                             children, params
                                         }: {
    children: ReactNode; params: Promise<{ lng: string }> ;
}) {
    const { lng } = await params
    const { i18n } = await getT();
    const headersList = await headers();
    const pathname = headersList.get('x-invoke-path') ||
        headersList.get('x-pathname') || '';

    if (process.env.NODE_ENV === 'development') {
        await i18n.reloadResources(i18nConfig.supportedLngs, i18nConfig.ns)
    }

    const resources = getResources(i18n)
    return (

        <I18nProvider fallbackLng={i18nConfig.fallbackLng} language={lng} resources={resources}>
            <Provider>
                <Header params={params}  />
                <main className="w-full ">
                    {children}
                </main>
                <Footer />
            </Provider>
        </I18nProvider>



    );
}