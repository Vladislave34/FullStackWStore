'use client'
import { useRef } from "react";
import { Formik, Form, Field as FormikField, ErrorMessage } from "formik";
import * as Yup from "yup";
import {useAppDispatch, useAppSelector} from "@/hooks/redux";
import { useEditProfileMutation } from "@/services/authService";
import IEditProfileModel from "@/models/auth/IEditProfileModel";
import {loginSuccess} from "@/store/reducers/authSlice";
import Image from "next/image";
import {CgProfile} from "react-icons/cg";


const editSchema = Yup.object({
    firstName: Yup.string(),
    lastName:  Yup.string(),
    username:  Yup.string(),
});

const EditProfileForm = () => {
    const user = useAppSelector(state => state.authSlice.user);
    const dispatch = useAppDispatch();
    const [editProfile, { isLoading }] = useEditProfileMutation();
    const fileInputRef = useRef<HTMLInputElement>(null);

    const initialValues: IEditProfileModel = {
        firstName: user?.firstName ?? "",
        lastName:  user?.lastName  ?? "",
        username:  user?.username  ?? "",
        image:     null,
    };

    const handleSubmit = async (values: IEditProfileModel) => {
        try {

            const res =  await editProfile(values).unwrap();
            localStorage.setItem("accessToken", res.accessToken);
            localStorage.setItem("refreshToken", res.refreshToken);
            dispatch(loginSuccess(res));
        } catch (err) {
            console.error(err);
        }
    };

    const initials = `${user?.firstName?.[0] ?? ""}${user?.lastName?.[0] ?? ""}`.toUpperCase();

    return (
        <Formik
            initialValues={initialValues}
            validationSchema={editSchema}
            onSubmit={handleSubmit}
        >
            {({ values, setFieldValue, isSubmitting, submitCount }) => (
                <div className="max-w-2xl mx-auto py-8 px-4">


                    <div className="bg-[var(--card)] border border-[var(--border)] rounded-xl p-5 mb-4 flex items-center gap-4">
                        <div className="relative shrink-0">
                            {values.image ? (
                                <Image
                                    src={URL.createObjectURL(values.image as File)}
                                    alt="avatar"
                                    width={72}
                                    height={72}
                                    className="rounded-full object-cover border-2 border-[var(--border)]"
                                />
                                // <CgProfile size={72} color="var(--accent-mid)" />
                            ) : user?.image ? (
                                <Image
                                    src={user.image}
                                    alt="avatar"
                                    width={72}
                                    height={72}
                                    className="rounded-full object-cover h-24 w-24 border-2 border-[var(--border)]"
                                    priority unoptimized
                                />
                            ) : (
                                <div className="w-[72px] h-[72px] rounded-full bg-[var(--accent-soft)] flex items-center justify-center text-xl font-semibold text-[var(--accent)]">
                                    {initials}
                                </div>
                            )}
                            <button
                                type="button"
                                onClick={() => fileInputRef.current?.click()}
                                className="absolute bottom-0 right-0 w-6 h-6 rounded-full bg-[var(--surface)] border border-[var(--border)] flex items-center justify-center cursor-pointer hover:bg-[var(--accent-soft)] transition-colors"
                                title="Змінити фото"
                            >
                                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="var(--muted)" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                    <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/>
                                    <circle cx="12" cy="13" r="4"/>
                                </svg>
                            </button>
                            <input
                                ref={fileInputRef}
                                type="file"
                                accept="image/*"
                                className="hidden"
                                onChange={e => {
                                    const file = e.target.files?.[0] ?? null;
                                    setFieldValue("image", file);
                                }}
                            />
                        </div>

                        <div className="flex-1">
                            <p className="m-0 font-semibold text-[15px] text-[var(--text)]">
                                {user?.firstName} {user?.lastName}
                            </p>
                            <p className="m-0 text-[13px] text-[var(--muted)]">{user?.email}</p>
                        </div>
                    </div>

                    <Form>

                        <div className="bg-[var(--card)] border border-[var(--border)] rounded-xl p-5 mb-4">
                            <p className="m-0 mb-4 text-[13px] font-semibold text-[var(--text)] uppercase tracking-wider">
                                Особисті дані
                            </p>
                            <div className="grid grid-cols-2 gap-4">
                                <InputField name="firstName" label="Ім'я"     placeholder="Ім'я" />
                                <InputField name="username"  label="Нікнейм"  placeholder="username" />
                                <InputField name="lastName"  label="Прізвище" placeholder="Прізвище" />
                            </div>
                        </div>


                        <div className="bg-[var(--card)] border border-[var(--border)] rounded-xl p-5 mb-4">
                            <p className="m-0 mb-4 text-[13px] font-semibold text-[var(--text)] uppercase tracking-wider">
                                Email адреса
                            </p>
                            <div className="flex items-center gap-3">
                                <div className="w-9 h-9 rounded-full bg-[var(--accent-soft)] flex items-center justify-center shrink-0">
                                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="var(--accent-mid)" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                        <rect x="2" y="4" width="20" height="16" rx="2"/>
                                        <path d="m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7"/>
                                    </svg>
                                </div>
                                <div>
                                    <p className="m-0 text-[14px] text-[var(--text)]">{user?.email}</p>
                                    <p className="m-0 text-[12px] text-[var(--muted)]">Основна пошта</p>
                                </div>
                            </div>
                        </div>

                        {/* Save */}
                        <div className="flex items-center gap-3">
                            <button
                                type="submit"
                                disabled={isLoading || isSubmitting}
                                className="px-6 py-2.5  rounded-lg text-[14px] bg-[var(--tag)]
                                font-semibold transition-opacity cursor-pointer text-[var(--tag-text)]
                                border border-[var(--border)] disabled:opacity-50
                                 disabled:cursor-not-allowed hover:bg-blue-500/10 hover:border-blue-500/20 hover:text-blue-500 hover:shadow-sm"
                                // style={{ background: "var(--tag)", color: "var(--tag-text)" }}
                            >
                                {isLoading || isSubmitting ? "Збереження..." : "Зберегти зміни"}
                            </button>

                            {submitCount > 0 && !isSubmitting && !isLoading && (
                                <span className="text-[13px] text-[var(--price)] flex items-center gap-1">
                                    <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                                        <path d="M20 6 9 17l-5-5"/>
                                    </svg>
                                    Збережено
                                </span>
                            )}
                        </div>
                    </Form>
                </div>
            )}
        </Formik>
    );
};

const InputField = ({
                        name, label, placeholder,
                    }: {
    name: string;
    label: string;
    placeholder?: string;
}) => (
    <div className="flex flex-col gap-1">
        <label className="text-[12px] font-medium text-[var(--muted)] uppercase tracking-wide">
            {label}
        </label>
        <FormikField
            name={name}
            placeholder={placeholder}
            className="px-3 py-2 rounded-lg text-[14px] text-[var(--text)] bg-[var(--bg)] border border-[var(--border)] outline-none transition-colors placeholder:text-[var(--muted)] focus:border-[var(--accent-mid)]"
        />
        <ErrorMessage
            name={name}
            component="p"
            className="text-[11px] text-[var(--sale)] m-0 mt-0.5"
        />
    </div>
);

export default EditProfileForm;
