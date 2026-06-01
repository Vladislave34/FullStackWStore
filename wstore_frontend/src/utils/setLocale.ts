'use server';

import { cookies } from 'next/headers';

export async function setLocale(locale: string) {
    const store = await cookies();
    store.set('locale', locale, {
        maxAge: 60 * 60 * 24 * 365, // 1 рік
        path: '/',
    });
}