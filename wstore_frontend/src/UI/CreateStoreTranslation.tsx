import CreateStoreArea from "@/UI/CreateStoreArea";
import {useAppDispatch} from "@/hooks/redux";
import {useTranslations} from "next-intl";
import {TranslationRecord} from "@/types/translations";


const CreateStoreTranslation = () => {
    const t = useTranslations("storecreateform");
    const createStoreForm: TranslationRecord<'storecreateform'> = {
        title: t('title'),
        subtitle: t('subtitle'),
        name_label: t('name_label'),
        name_placeholder: t('name_placeholder'),
        description_label: t('description_label'),
        description_placeholder: t('description_placeholder'),
        avatar_label: t('avatar_label'),
        upload_text: t('upload_text'),
        submit: t('submit'),
    };
    return (
        <>
            <CreateStoreArea transform={createStoreForm}  />
        </>
    );
};

export default CreateStoreTranslation;