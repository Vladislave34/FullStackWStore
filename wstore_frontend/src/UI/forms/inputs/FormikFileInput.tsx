'use client'
import { useState } from "react";
import { useFormikContext } from "formik";

interface FormikFileInputProps<T> {
    name: keyof T;
    label: string;
    multiple?: boolean;
}

function FormikFileInput<T>({ name, label, multiple = false }: FormikFileInputProps<T>) {
    const { setFieldValue, errors, touched } = useFormikContext<T>();
    const [files, setFiles] = useState<File[]>([]);
    const [previews, setPreviews] = useState<string[]>([]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const incoming = Array.from(e.currentTarget.files ?? []);
        if (!incoming.length) return;

        if (multiple) {
            const merged = [...files, ...incoming];
            setFiles(merged);
            setPreviews(merged.map(f => URL.createObjectURL(f)));
            setFieldValue(name as string, merged);
        } else {
            setFiles([incoming[0]]);
            setPreviews([URL.createObjectURL(incoming[0])]);
            setFieldValue(name as string, incoming[0]);
        }
    };

    const removeImage = (index: number) => {
        const newFiles = files.filter((_, i) => i !== index);
        const newPreviews = previews.filter((_, i) => i !== index);
        setFiles(newFiles);
        setPreviews(newPreviews);
        setFieldValue(name as string, multiple ? newFiles : null);
    };

    return (
        <div className="flex flex-col">
            <label className="text-[13px] text-[#5F5E5A] mb-[6px]">{label}</label>
            <label className="
                mt-[2px]
                flex flex-col items-center justify-center gap-2
                rounded-xl
                border border-dashed border-[var(--border)]
                p-5
                cursor-pointer
                hover:border-[#888780]
                hover:bg-[var(--hover-bg)]
                transition-colors
            ">
                <div className="
                    w-11 h-11 rounded-full
                    bg-[var(--hover-bg)]
                    flex items-center justify-center
                    text-[#5F5E5A] text-xl
                ">
                    ↑
                </div>
                <span className="text-[13px] text-[#888780]">
                    {multiple ? "Завантажити фото (можна декілька)" : "Завантажити фото"}
                </span>
                <input
                    type="file"
                    accept="image/*"
                    multiple={multiple}
                    className="hidden"
                    onChange={handleChange}
                />
            </label>

            {previews.length > 0 && (
                <div className="flex flex-wrap gap-2 mt-3">
                    {previews.map((src, i) => (
                        <div key={i} className="relative">
                            <img
                                src={src}
                                alt={`preview-${i}`}
                                className="w-16 h-16 rounded-xl object-cover border border-[var(--border)]"
                            />
                            <button
                                type="button"
                                onClick={() => removeImage(i)}
                                className="
                                    absolute -top-1.5 -right-1.5
                                    w-5 h-5 rounded-full
                                    bg-[#A32D2D] text-white
                                    text-xs flex items-center justify-center
                                    hover:bg-red-700 transition-colors
                                "
                            >
                                ×
                            </button>
                        </div>
                    ))}
                </div>
            )}

            {touched[name] && errors[name] && (
                <span className="text-[#A32D2D] text-[12px] mt-[5px]">
                    {errors[name] as string}
                </span>
            )}
        </div>
    );
}

export default FormikFileInput;