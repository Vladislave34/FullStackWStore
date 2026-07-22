import type { Metadata } from 'next'
import { redirect } from 'next/navigation'
import {afterWrite} from "@popperjs/core";
import {getT} from "next-i18next/server";

export const metadata: Metadata = {
    title: 'WStore',
}


export default async function RootPage() {
    redirect(`/en?page=1`)
}
