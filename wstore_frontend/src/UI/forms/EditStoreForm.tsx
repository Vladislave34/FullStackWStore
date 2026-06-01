'use client'
import { useRef, useState } from "react";
import { Formik, Form, Field as FormikField, ErrorMessage } from "formik";
import * as Yup from "yup";
import Image from "next/image";
import IEditStoreModel from "@/models/store/IEditStoreModel";

interface StoreItemModel {
    id: string;
    name: string;
    description: string;
    images: string[];
}

const storeSchema = Yup.object({
    name: Yup.string().required("Назва обов'язкова"),
    description: Yup.string().required("Опис обов'язковий"),
});

interface Props {
    store?: StoreItemModel;
    onSubmit: (values: IEditStoreModel) => Promise<void>;
    isLoading?: boolean;
}

const EditStoreForm = ({ store, onSubmit, isLoading }: Props) => {
    const fileInputRef = useRef<HTMLInputElement>(null);
    const [activeIndex, setActiveIndex] = useState(0);
    // Існуючі URL зображень (можна видаляти)
    const [existingImages, setExistingImages] = useState<string[]>(store?.images ?? []);

    const initialValues: IEditStoreModel = {
        id: store?.id,
        name: store?.name ?? "",
        description: store?.description ?? "",
        images: [],
    };

    const buildPreviewUrls = (newFiles: File[]) => [
        ...existingImages,
        ...newFiles.map(f => URL.createObjectURL(f)),
    ];

    const removeImage = (
        index: number,
        newFiles: File[],
        setFieldValue: (field: string, value: unknown) => void
    ) => {
        const existingCount = existingImages.length;

        if (index < existingCount) {
            // Видаляємо існуюче зображення
            const updated = existingImages.filter((_, i) => i !== index);
            setExistingImages(updated);
        } else {
            // Видаляємо новий файл
            const fileIndex = index - existingCount;
            const updated = newFiles.filter((_, i) => i !== fileIndex);
            setFieldValue("images", updated);
        }

        // Коригуємо activeIndex якщо треба
        setActiveIndex(prev => {
            const newTotal = existingImages.length + newFiles.length - 1;
            return prev >= newTotal ? Math.max(newTotal - 1, 0) : prev;
        });
    };

    return (
        <Formik
            initialValues={initialValues}
            validationSchema={storeSchema}
            onSubmit={(values) => onSubmit({ ...values })}
        >
            {({ values, setFieldValue, isSubmitting, submitCount }) => {
                const previewUrls = buildPreviewUrls(values.images);
                const total = previewUrls.length;
                const safeIndex = Math.min(activeIndex, Math.max(total - 1, 0));

                const prev = () => setActiveIndex(i => (i - 1 + total) % total);
                const next = () => setActiveIndex(i => (i + 1) % total);

                return (
                    <div className="w-full p-6">
                        <Form className="h-full">
                            <div className="flex gap-6 items-stretch h-full">

                                {/* ── Ліворуч: зображення ── */}
                                <div className="bg-[var(--card)] border border-[var(--border)] rounded-xl p-6 w-[55%]">
                                    <p className="m-0 mb-4 text-[13px] font-semibold text-[var(--text)] uppercase tracking-wider">
                                        Зображення магазину
                                    </p>

                                    {total > 0 ? (
                                        <div className="relative w-full aspect-video rounded-lg overflow-hidden bg-[var(--bg)] mb-3">
                                            <Image
                                                src={previewUrls[safeIndex]}
                                                alt={`store image ${safeIndex + 1}`}
                                                fill
                                                className="object-cover"
                                                unoptimized
                                            />

                                            {/* Кнопка видалення поточного фото */}
                                            <button
                                                type="button"
                                                onClick={() => removeImage(safeIndex, values.images, setFieldValue)}
                                                className="absolute top-2 left-2 w-7 h-7 rounded-full bg-black/50 hover:bg-red-500/80 text-white flex items-center justify-center transition-colors"
                                                title="Видалити фото"
                                            >
                                                <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                                                    <path d="M18 6 6 18M6 6l12 12"/>
                                                </svg>
                                            </button>

                                            {total > 1 && (
                                                <>
                                                    <button
                                                        type="button"
                                                        onClick={prev}
                                                        className="absolute left-2 top-1/2 -translate-y-1/2 w-8 h-8 rounded-full bg-black/40 hover:bg-black/60 text-white flex items-center justify-center transition-colors"
                                                    >
                                                        ‹
                                                    </button>
                                                    <button
                                                        type="button"
                                                        onClick={next}
                                                        className="absolute right-2 top-1/2 -translate-y-1/2 w-8 h-8 rounded-full bg-black/40 hover:bg-black/60 text-white flex items-center justify-center transition-colors"
                                                    >
                                                        ›
                                                    </button>

                                                    <div className="absolute bottom-2 left-1/2 -translate-x-1/2 flex gap-1.5">
                                                        {previewUrls.map((_, i) => (
                                                            <button
                                                                key={i}
                                                                type="button"
                                                                onClick={() => setActiveIndex(i)}
                                                                className={`h-1.5 rounded-full transition-all ${
                                                                    i === safeIndex ? "bg-white w-3" : "bg-white/50 w-1.5"
                                                                }`}
                                                            />
                                                        ))}
                                                    </div>
                                                </>
                                            )}

                                            <span className="absolute top-2 right-2 text-[11px] text-white bg-black/40 px-2 py-0.5 rounded-full">
                                                {safeIndex + 1} / {total}
                                            </span>
                                        </div>
                                    ) : (
                                        <div
                                            onClick={() => fileInputRef.current?.click()}
                                            className="w-full aspect-video rounded-lg border-2 border-dashed border-[var(--border)] flex flex-col items-center justify-center gap-2 cursor-pointer hover:border-blue-500/40 hover:bg-blue-500/5 transition-colors mb-3"
                                        >
                                            <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="var(--muted)" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
                                                <rect x="3" y="3" width="18" height="18" rx="2"/>
                                                <circle cx="8.5" cy="8.5" r="1.5"/>
                                                <path d="m21 15-5-5L5 21"/>
                                            </svg>
                                            <span className="text-[13px] text-[var(--muted)]">Натисніть щоб додати фото</span>
                                        </div>
                                    )}

                                    {total > 0 && (
                                        <div className="flex gap-2 flex-wrap">
                                            {previewUrls.map((url, i) => (
                                                <div key={i} className="relative group">
                                                    <button
                                                        type="button"
                                                        onClick={() => setActiveIndex(i)}
                                                        className={`relative w-16 h-16 rounded-lg overflow-hidden border-2 transition-all ${
                                                            i === safeIndex
                                                                ? "border-blue-500"
                                                                : "border-[var(--border)] opacity-60 hover:opacity-100"
                                                        }`}
                                                    >
                                                        <Image src={url} alt="" fill className="object-cover" unoptimized />
                                                    </button>

                                                    {/* Кнопка видалення на мініатюрі */}
                                                    <button
                                                        type="button"
                                                        onClick={() => removeImage(i, values.images, setFieldValue)}
                                                        className="absolute -top-1.5 -right-1.5 w-5 h-5 rounded-full bg-red-500 text-white items-center justify-center hidden group-hover:flex transition-all shadow-sm z-10"
                                                    >
                                                        <svg width="9" height="9" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round">
                                                            <path d="M18 6 6 18M6 6l12 12"/>
                                                        </svg>
                                                    </button>
                                                </div>
                                            ))}

                                            <button
                                                type="button"
                                                onClick={() => fileInputRef.current?.click()}
                                                className="w-16 h-16 rounded-lg border-2 border-dashed border-[var(--border)] flex items-center justify-center text-[var(--muted)] text-xl hover:border-blue-500/40 hover:bg-blue-500/5 transition-colors"
                                            >
                                                +
                                            </button>
                                        </div>
                                    )}

                                    <input
                                        ref={fileInputRef}
                                        type="file"
                                        accept="image/*"
                                        multiple
                                        className="hidden"
                                        onChange={e => {
                                            const files = Array.from(e.target.files ?? []);
                                            if (!files.length) return;
                                            const updated = [...values.images, ...files];
                                            setFieldValue("images", updated);
                                            setActiveIndex(existingImages.length + updated.length - 1);
                                            e.target.value = "";
                                        }}
                                    />
                                </div>

                                {/* ── Праворуч: інфо + кнопка ── */}
                                <div className="flex flex-col gap-4 w-[45%]">
                                    <div className="bg-[var(--card)] border border-[var(--border)] rounded-xl p-6 flex-1">
                                        <p className="m-0 mb-4 text-[13px] font-semibold text-[var(--text)] uppercase tracking-wider">
                                            Інформація про магазин
                                        </p>
                                        <div className="flex flex-col gap-4 h-full">
                                            <InputField name="name" label="Назва" placeholder="Назва магазину" />

                                            <div className="flex flex-col gap-1 flex-1">
                                                <label className="text-[12px] font-medium text-[var(--muted)] uppercase tracking-wide">
                                                    Опис
                                                </label>
                                                <FormikField
                                                    as="textarea"
                                                    name="description"
                                                    placeholder="Розкажіть про ваш магазин..."
                                                    className="px-3 py-2 rounded-lg text-[14px] text-[var(--text)] bg-[var(--bg)] border border-[var(--border)] outline-none transition-colors resize-none placeholder:text-[var(--muted)] focus:border-[var(--accent-mid)] w-full flex min-h-[300px]"
                                                />
                                                <ErrorMessage
                                                    name="description"
                                                    component="p"
                                                    className="text-[11px] text-[var(--sale)] m-0 mt-0.5"
                                                />
                                            </div>
                                        </div>
                                    </div>

                                    <div className="flex items-center gap-3">
                                        <button
                                            type="submit"
                                            disabled={isLoading || isSubmitting}
                                            className="px-6 py-2.5 rounded-lg text-[14px] bg-[var(--tag)] font-semibold transition-opacity cursor-pointer text-[var(--tag-text)] border border-[var(--border)] disabled:opacity-50 disabled:cursor-not-allowed hover:bg-blue-500/10 hover:border-blue-500/20 hover:text-blue-500 hover:shadow-sm"
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
                                </div>

                            </div>
                        </Form>
                    </div>
                );
            }}
        </Formik>
    );
};

const InputField = ({ name, label, placeholder }: { name: string; label: string; placeholder?: string }) => (
    <div className="flex flex-col gap-1">
        <label className="text-[12px] font-medium text-[var(--muted)] uppercase tracking-wide">
            {label}
        </label>
        <FormikField
            name={name}
            placeholder={placeholder}
            className="px-3 py-2 rounded-lg text-[14px] text-[var(--text)] bg-[var(--bg)] border border-[var(--border)] outline-none transition-colors placeholder:text-[var(--muted)] focus:border-[var(--accent-mid)]"
        />
        <ErrorMessage name={name} component="p" className="text-[11px] text-[var(--sale)] m-0 mt-0.5" />
    </div>
);

export default EditStoreForm;