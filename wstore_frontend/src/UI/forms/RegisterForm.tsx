
import * as Yup from "yup";


import {useAppDispatch} from "@/hooks/redux";
import UniversalForm from "@/UI/forms/abstract/UniversalForm";
import {loginSuccess} from "@/store/reducers/authSlice";
import FormikInput from "@/UI/forms/inputs/FormikInput";
import IRegisterModel from "@/models/auth/IRegisterModel";
import FormikFileInput from "@/UI/forms/inputs/FormikFileInput";
import {useRegisterMutation} from "@/services/authService";
import {useTranslations} from "next-intl";
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
    title="Реєстрація"
    subtitle="Введіть дані для продовження"
    submitLabel="Зареєструватися"
    >
            <FormikInput<RegisterTextValues>
                name="firstName"
                label="First Name"
                type="text"
                placeholder="First Name"
            />
            <FormikInput<RegisterTextValues>
                name="lastName"
                label="Last Name"
                type="text"
                placeholder="Last Name"
            />
            <FormikFileInput<IRegisterModel>
                name="image"
                label="Avatar"
                multiple={false}
            />
            <FormikInput<RegisterTextValues>
                name="username"
                label="Username"
                type="text"
                placeholder="Username"
            />
            <FormikInput<RegisterTextValues>
                name="email"
            label="Email"
            type="email"
            placeholder="your@email.com"
            />
            <FormikInput<RegisterTextValues>
                name="password"
            label="Пароль"
            type="password"
            placeholder="••••••••"
            />


        </UniversalForm>
);
};

export default LoginForm;