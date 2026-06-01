// types/translations.ts
import type en from '../messages/en.json';

export type TranslationRecord<T extends keyof typeof en> = {
    [K in keyof typeof en[T]]: string;
}
// окремий тип для пропсів
export type TranslationProps<T extends keyof typeof en> = {
    labels: TranslationRecord<T>;
};