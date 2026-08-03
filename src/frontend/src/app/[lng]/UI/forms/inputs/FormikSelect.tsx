'use client'
import { useFormikContext } from "formik";

interface Option {
    value: string | number;
    label: string;
}

interface FormikSelectProps<T> {
    name: keyof T;
    label: string;
    options: Option[];
    placeholder?: string;
}

function FormikSelect<T extends object>({
                                            name,
                                            label,
                                            options,
                                            placeholder,
                                        }: FormikSelectProps<T>) {
    const { values, errors, touched, handleChange, handleBlur } =
        useFormikContext<T>();

    return (
        <div className="relative">
            <select
                name={name as string}
                value={values[name] as string | number}
                onChange={handleChange}
                onBlur={handleBlur}
                className="
            bg-[var(--search)]
            border border-[var(--border)]
            rounded-[10px]
            px-4 py-[10px]
            pr-10
            text-sm text-[var(--placeholder)]
            outline-none
            transition-colors
            focus:border-[#888780]
            w-full
            appearance-none
            cursor-pointer
        "
            >
                {placeholder && (
                    <option value="" disabled>
                        {placeholder}
                    </option>
                )}
                {options.map((option) => (
                    <option key={option.value} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </select>

            {/* Кастомна стрілка */}
            <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-4">
                <svg
                    className="w-4 h-4 text-[var(--placeholder)]"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                </svg>
            </div>
        </div>
    );
}

export default FormikSelect;