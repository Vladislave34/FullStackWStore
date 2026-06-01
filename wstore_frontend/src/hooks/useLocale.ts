"use client";

import { useRouter } from "next/navigation";
import { useTransition } from "react";
import { useLocale as useIntlLocale } from "next-intl";

export function useLocale() {
    const router = useRouter();
    const [isPending, startTransition] = useTransition();
    const locale = useIntlLocale(); // бере поточну локаль з next-intl

    const toggleLocale = () => {
        const newLocale = locale === "uk" ? "en" : "uk";
        document.cookie = `locale=${newLocale}; path=/; max-age=${60 * 60 * 24 * 365}`;
        startTransition(() => router.refresh());
    };

    return { locale, toggleLocale, isPending };
}