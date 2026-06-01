
import * as Yup from "yup";


import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/UI/forms/abstract/UniversalForm";
import {loginSuccess} from "@/store/reducers/authSlice";
import FormikInput from "@/UI/forms/inputs/FormikInput";
import {useLoginMutation} from "@/services/authService";

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
    const [login] = useLoginMutation();
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
            title="Вхід до акаунту"
            subtitle="Введіть дані для продовження"
            submitLabel="Увійти"
        >
            <FormikInput<LoginValues>
                name="email"
                label="Email"
                type="email"
                placeholder="your@email.com"
            />
            <FormikInput<LoginValues>
                name="password"
                label="Пароль"
                type="password"
                placeholder="••••••••"
            />

            <p className="text-[12px] text-[#888780] text-right -mt-2 cursor-pointer hover:text-[var(--text)] transition-colors">
                Забули пароль?
            </p>
        </UniversalForm>
    );
};

export default LoginForm;