"use client"
import { type ReactNode, useEffect } from "react";
import { createPortal } from "react-dom";

interface ModalProps {
    isOpen: boolean;
    closeModal: () => void;
    children: ReactNode;
    title?: string;
    size?: "sm" | "md" | "lg";
}

const sizeMap = {
    sm: "max-w-[360px]",
    md: "max-w-[480px]",
    lg: "max-w-[640px]",
};

const Modal = ({ isOpen, closeModal, children, title, size = "md" }: ModalProps) => {
    useEffect(() => {
        if (!isOpen) return;
        const onKey = (e: KeyboardEvent) => e.key === "Escape" && closeModal();
        document.addEventListener("keydown", onKey);
        document.body.style.overflow = "hidden";
        return () => {
            document.removeEventListener("keydown", onKey);
            document.body.style.overflow = "";
        };
    }, [isOpen, closeModal]);

    if (!isOpen) return null;

    return createPortal(
        <div
            className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm"
            onClick={closeModal}
        >
            <div
                className={`
                    relative w-full ${sizeMap[size]}
                    bg-[var(--surface)]
                    border border-[var(--border)]
                    rounded-2xl
                    shadow-xl
                    p-6
                    animate-in fade-in zoom-in-95 duration-150
                `}
                onClick={(e) => e.stopPropagation()}
            >
                {/* Header */}
                {title && (
                    <div className="flex items-center justify-between mb-5">
                        <h2 className="text-[18px] font-medium text-[var(--logo-text)]">
                            {title}
                        </h2>
                        <button
                            onClick={closeModal}
                            className="
                                w-8 h-8 flex items-center justify-center
                                rounded-lg text-[#888780]
                                hover:bg-[var(--hover-bg)]
                                hover:text-[var(--text)]
                                transition-colors text-lg
                            "
                            aria-label="Закрити"
                        >
                            ✕
                        </button>
                    </div>
                )}

                {/* Close без title */}
                {!title && (
                    <button
                        onClick={closeModal}
                        className="
                            absolute top-4 right-4
                            w-8 h-8 flex items-center justify-center
                            rounded-lg text-[#888780]
                            hover:bg-[var(--hover-bg)]
                            hover:text-[var(--text)]
                            transition-colors text-lg
                        "
                        aria-label="Закрити"
                    >
                        ✕
                    </button>
                )}

                {children}
            </div>
        </div>,
        document.body
    );
};

export default Modal;