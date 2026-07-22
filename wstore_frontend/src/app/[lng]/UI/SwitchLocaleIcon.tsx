// 'use client';
//
// import Icon from '@/UI/Icon';
// import { usePathname, useRouter } from 'next/navigation';
// import i18nConfig from '../../i18n.config';
//
// const SwitchLocaleIcon = () => {
//     const router = useRouter();
//     const pathname = usePathname();
//
//     const segments = pathname.split('/');
//
//     const currentLocale = i18nConfig.supportedLngs.includes(segments[1])
//         ? segments[1]
//         : i18nConfig.fallbackLng;
//
//     const changeLang = () => {
//         const newLocale = currentLocale === 'en' ? 'uk' : 'en';
//
//         const hasLocale = i18nConfig.supportedLngs.includes(segments[1]);
//
//         const newPath = hasLocale
//             ? `/${newLocale}/${segments.slice(2).join('/')}`
//             : `/${newLocale}${pathname}`;
//
//         router.push(newPath);
//     };
//
//     return (
//         <div onClick={changeLang} className="cursor-pointer">
//             <Icon height={40} width={40}>
//                 {currentLocale.toUpperCase()}
//             </Icon>
//         </div>
//     );
// };
//
// export default SwitchLocaleIcon;

// 'use client';
//
// import { usePathname, useRouter } from 'next/navigation';
// import i18nConfig from "../../../../i18n.config";
//
//
// export default function LanguageSwitcher() {
//     const router = useRouter();
//     const pathname = usePathname();
//
//     const changeLang = (lng: string) => {
//         const segments = pathname.split('/');
//         const hasLocale = i18nConfig.supportedLngs.includes(segments[1]);
//
//         const newPath = hasLocale
//             ? `/${lng}/${segments.slice(2).join('/')}`
//             : `/${lng}${pathname}`;
//
//         router.push(newPath);
//     };
//
//     return (
//         <div className="flex gap-2">
//             <button onClick={() => changeLang('en')}>EN</button>
//             <button onClick={() => changeLang('uk')}>UA</button>
//         </div>
//     );
// }
'use client';

import {useRouter, usePathname, useSearchParams} from 'next/navigation';
import i18nConfig from "../../../../i18n.config";


export default function LanguageSwitcher() {
    const router = useRouter();
    const pathname = usePathname();
    const searchParams = useSearchParams();

    const currentLang = pathname.split('/')[1];

    const changeLang = (lng: string) => {
        const segments = pathname.split('/');
        const hasLocale = i18nConfig.supportedLngs.includes(segments[1]);

        const newPath = hasLocale
            ? `/${lng}/${segments.slice(2).join('/')}`
            : `/${lng}${pathname}`;

        const query = searchParams.toString();
        const newUrl = query ? `${newPath}?${query}` : newPath;

        router.push(newUrl);
    };

    return (
        <div
            className="
                flex items-center
                rounded-xl
                border
                p-1
                gap-1
                bg-[var(--surface)]
                border-[var(--border)]
            "
        >
            {[
                { code: 'en', label: 'EN' },
                { code: 'uk', label: 'UA' },
            ].map((lang) => (
                <button
                    key={lang.code}
                    onClick={() => changeLang(lang.code)}
                    className={`
                        px-3 py-1.5
                        text-sm font-medium
                        rounded-lg
                        transition-all duration-200
                        hover:cursor-pointer
                        ${
                        currentLang === lang.code
                            ? 'bg-[var(--btn)] text-white'
                            : 'text-[var(--text)] hover:bg-[var(--accent-soft)]'
                    }
                    `}
                >
                    {lang.label}
                </button>
            ))}
        </div>
    );
}