'use client'
import * as Yup from "yup";


import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import {loginSuccess} from "@/store/reducers/authSlice";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";
import {useLoginMutation} from "@/services/authService";
import {useT} from "next-i18next/client";
import {useState} from "react";
import EmailInput from "@/app/[lng]/UI/EmailInput";

type LoginValues =  {
    email: string;
    password: string;
}

const loginSchema = Yup.object({
    email: Yup.string()
        .email("Невірний формат email")
        .required("Email обов'язковий"),
    password: Yup.string()
        .min(6, "Мінімум 6 символів")
        .required("Пароль обов'язковий"),
});

const initialValues: LoginValues = {
    email: "",
    password: "",
};

const LoginForm = ({closeModal}:{closeModal: ()=>void}) => {
    const dispatch = useAppDispatch();
    const [isOpenInput, setIsOpenInput] = useState(false);
    const [login] = useLoginMutation();
    const {t} = useT('login_form')
    const handleSubmit = async (values: LoginValues) => {
        try {
            const res = await login(values).unwrap();
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
            <FormikInput<LoginValues>
                name="email"
                label={t('email')}
                type="email"
                placeholder="your@email.com"
            />
            <FormikInput<LoginValues>
                name="password"
                label={t('password')}
                type="password"
                placeholder="••••••••"
            />

            <p
                className="
                text-[12px] text-[#888780] text-right
                -mt-2 cursor-pointer hover:text-[var(--text)]
                transition-colors"
                onClick={()=>setIsOpenInput(prev => !prev)}
            >
                {t('forgot_password')}
            </p>
            <div className={`${isOpenInput ? "" : "hidden"}`}>
                <EmailInput />
            </div>
        </UniversalForm>
    );
};

export default LoginForm;