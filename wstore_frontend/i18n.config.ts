import type { I18nConfig } from 'next-i18next/proxy'

const i18nConfig: I18nConfig = {
    supportedLngs: ['en', 'uk'],
    fallbackLng: 'en',
    defaultNS: 'common',
    ns: [
        'nav', 'hero', 'guest_menu',
        'authorize_menu', 'profile', 'create_store',
        'create_store_form', 'formik_file_input', 'login_form',
        'register_form', 'edit_profile_form', 'edit_store_form',
        'store_products', 'add_product_form', 'product_card_for_store',
        'create_variant_form', 'sec_head', 'order_status'
    ],
    hideDefaultLocale: true,
    ...(process.env.NODE_ENV === 'production'
        ? {
            resourceLoader: (language, namespace) =>
                import(`./app/i18n/locales/${language}/${namespace}.json`)
        }
        : {})
}

export default i18nConfig