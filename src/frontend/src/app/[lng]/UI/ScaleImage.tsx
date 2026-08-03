'use client'




import { useEffect, useState } from "react";
import Image from "next/image";

const ScaleImage = ({ image, alt = "Image" }: { image: string; alt?: string }) => {
    const [isScaled, setIsScaled] = useState(false);

    useEffect(() => {
        if (!isScaled) return;
        document.body.style.overflow = "hidden";
        const handleKeyDown = (e: KeyboardEvent) => {
            if (e.key === "Escape") setIsScaled(false);
        };
        window.addEventListener("keydown", handleKeyDown);
        return () => {
            document.body.style.overflow = "";
            window.removeEventListener("keydown", handleKeyDown);
        };
    }, [isScaled]);

    return (
        <div>
            <div className="relative w-12 h-12 shrink-0">
                <Image
                    src={image}
                    alt={alt}
                    fill
                    unoptimized
                    className="object-cover rounded-lg cursor-zoom-in"
                    onClick={() => setIsScaled(true)}
                />
            </div>

            {isScaled && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm"
                    onClick={() => setIsScaled(false)}
                >
                    <div
                        className="relative w-full max-w-3xl max-h-[85vh] bg-[var(--surface)] border border-[var(--border)] rounded-2xl shadow-xl p-6 animate-in fade-in zoom-in-95 duration-150"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <button
                            type="button"
                            aria-label="Close"
                            onClick={() => setIsScaled(false)}
                            className="absolute top-3 right-3 text-[var(--foreground)] opacity-70 hover:opacity-100"
                        >
                            ✕
                        </button>
                        <div className="relative w-full h-[70vh]">
                            <Image src={image} alt={alt} fill unoptimized className="object-contain" />
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ScaleImage;