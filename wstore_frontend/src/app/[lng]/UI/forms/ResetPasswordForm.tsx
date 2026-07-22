'use client'
import * as Yup from "yup";
import FormikNumberInput from "@/app/[lng]/UI/forms/inputs/FormikNumberInput";
import IResetPasswordModel from "@/models/auth/IResetPasswordModel";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import IAddProduct from "@/models/product/IAddProduct";
import FormikInput from "@/app/[lng]/UI/forms/inputs/FormikInput";
import {usePathname, useSearchParams} from "next/navigation";
import {authApi} from "@/services/authService";


const schema = Yup.object({
    newPassword: Yup.string().required(),
    confirmNewPassword: Yup.string()
        .required()
        .oneOf(
            [Yup.ref("newPassword")],
            "Паролі не співпадають"
        )
})

const ResetPasswordForm = () => {
    const searchParams = useSearchParams();
    const email = searchParams.get("email") ?? '';
    const token = searchParams.get("token") ?? '';
    const [resetPassword] = authApi.useResetPasswordMutation();
    const initialValues : IResetPasswordModel = {
        email: email,
        token: token,
        newPassword: '',
        confirmNewPassword: '',
    }
    return (
        <UniversalForm
            validationSchema={schema}
            initialValues={initialValues}
            onSubmit={ async (values) =>
                await resetPassword(values).unwrap()
            }
            title={'Reset password'}
            subtitle={'Reset password'}
            submitLabel={'Reset'}
        >
            <FormikInput<IAddProduct>
                name="newPassword"
                label={"New password"}
                type="password"
                placeholder={"New password"}
            />
            <FormikInput<IAddProduct>
                name="confirmNewPassword"
                label={"Confirm New Password"}
                type="password"
                placeholder={"New password"}
            />
        </UniversalForm>
    );
};

export default ResetPasswordForm;