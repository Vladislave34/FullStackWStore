import * as Yup from "yup";
import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/UI/forms/abstract/UniversalForm";

import FormikInput from "@/UI/forms/inputs/FormikInput";
import IRegisterModel from "@/models/auth/IRegisterModel";
import FormikFileInput from "@/UI/forms/inputs/FormikFileInput";

import ICreateStore from "@/models/store/ICreateStore";

import {storeApi} from "@/services/storeService";
import {loginSuccess} from "@/store/reducers/authSlice";
import {useTranslations} from "next-intl";
import {TranslationProps, TranslationRecord} from "@/types/translations";


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

const CreateStoreForm = ({closeModal, trans}:{closeModal: ()=>void, trans?: TranslationRecord<"storecreateform">}) => {
    const dispatch = useAppDispatch();

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
            title="Створення Магазину"
            subtitle="Введіть дані для продовження"
            submitLabel="Створити"
        >
            <FormikInput<CreateStoreTextValues>
                name="name"
                label="Name"
                type="text"
                placeholder="Name"
            />
            <FormikInput<CreateStoreTextValues>
                name="description"
                label="Description"
                type="text"
                placeholder="Description"
            />
            <FormikFileInput<ICreateStore>
                name="images"
                label="Avatar"
                multiple={true}
            />


        </UniversalForm>
    );
};

export default CreateStoreForm;