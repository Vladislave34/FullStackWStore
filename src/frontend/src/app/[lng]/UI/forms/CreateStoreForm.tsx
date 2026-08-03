import * as Yup from "yup";
import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";

import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";

import FormikFileInput from "@/app/[lng]/UI/forms/inputs/FormikFileInput";

import ICreateStore from "@/models/store/ICreateStore";

import {storeApi} from "@/services/storeService";
import {loginSuccess} from "@/store/reducers/authSlice";
import {useT} from "next-i18next/client";



type CreateStoreTextValues = Omit<ICreateStore, "images">;


const loginSchema = Yup.object({
    name: Yup.string().required(),
    description: Yup.string().required(),

});

const initialValues: ICreateStore = {
    name: "",
    description: "",
    images: null,
};

const CreateStoreForm = ({closeModal}:{closeModal: ()=>void}) => {
    const dispatch = useAppDispatch();
    const {t} = useT('create_store_form')
    const [createStore] = storeApi.useCreateStoreMutation();

    const handleSubmit = async (values: ICreateStore) => {
        try {

            const res = await createStore(values).unwrap();
            localStorage.setItem("accessToken", res.accessToken);
            localStorage.setItem("refreshToken", res.refreshToken);
            dispatch(loginSuccess(res));
            closeModal();
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <UniversalForm
            initialValues={initialValues}
            validationSchema={loginSchema}
            onSubmit={handleSubmit}
            title={t('title')}
            subtitle={t('subtitle')}
            submitLabel={t('submit')}
        >
            <FormikInput<CreateStoreTextValues>
                name="name"
                label={t("name")}
                type="text"
                placeholder={t("name")}
            />
            <FormikInput<CreateStoreTextValues>
                name="description"
                label={t("description")}
                type="text"
                placeholder={t("description")}
            />
            <FormikFileInput<ICreateStore>
                name="images"
                label={t("avatar")}
                multiple={true}
            />


        </UniversalForm>
    );
};

export default CreateStoreForm;