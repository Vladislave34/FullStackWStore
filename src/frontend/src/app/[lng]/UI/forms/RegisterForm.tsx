
import * as Yup from "yup";


import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import {loginSuccess} from "@/store/reducers/authSlice";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";
import IRegisterModel from "@/models/auth/IRegisterModel";
import FormikFileInput from "@/app/[lng]/UI/forms/inputs/FormikFileInput";
import {useRegisterMutation} from "@/services/authService";

import {useT} from "next-i18next/client";
type RegisterTextValues = Omit<IRegisterModel, "image">;


const loginSchema = Yup.object({
    email: Yup.string()
        .email("Невірний формат email")
        .required("Email обов'язковий"),
    password: Yup.string()
        .min(6, "Мінімум 6 символів")
        .required("Пароль обов'язковий"),
});

const initialValues: IRegisterModel = {
    firstName: "",
    lastName: "",
    image: null,
    username: "",
    email: "",
    password: "",
};

const LoginForm = ({closeModal}:{closeModal: ()=>void}) => {
    const dispatch = useAppDispatch();
    const [register] = useRegisterMutation();
    const {t} = useT('register_form');
    const handleSubmit = async (values: IRegisterModel) => {
        try {
            const res = await register(values).unwrap();
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
            <FormikInput<RegisterTextValues>
                name="firstName"
                label={t('first_name')}
                type="text"
                placeholder={t('first_name')}
            />
            <FormikInput<RegisterTextValues>
                name="lastName"
                label={t('last_name')}
                type="text"
                placeholder={t('last_name')}
            />
            <FormikFileInput<IRegisterModel>
                name="image"
                label={t('avatar')}
                multiple={false}
            />
            <FormikInput<RegisterTextValues>
                name="username"
                label={t('username')}
                type="text"
                placeholder={t('username')}
            />
            <FormikInput<RegisterTextValues>
                name="email"
                label={t('email')}
                type="email"
                placeholder="your@email.com"
            />
            <FormikInput<RegisterTextValues>
                name="password"
                label={t('password')}
                type="password"
                placeholder="••••••••"
            />


        </UniversalForm>
);
};

export default LoginForm;