import {getRequestConfig} from 'next-intl/server';
import {cookies, headers} from "next/headers";

export default getRequestConfig(async () => {

    const store = await cookies();

    // 1. Спочатку перевіряємо збережену мову в куці
    let locale = store.get('locale')?.value;


    if (!locale) {
        const headersList = await headers();
        const acceptLanguage = headersList.get('accept-language') || '';

        // Accept-Language  "uk-UA,uk;q=0.9,en;q=0.8"
        const preferred = acceptLanguage
            .split(',')[0]
            .split('-')[0]
            .toLowerCase()
            .trim();
        console.log(preferred);
        const supportedLocales = ['en', 'uk'];
        locale = supportedLocales.includes(preferred) ? preferred : 'en';
    }

    return {
        locale,
        messages: (await import(`@/messages/${locale}.json`)).default
    };
});