'use client'
import {useEffect, useRef, useState} from "react";
import {productVariantApi} from "@/services/productVariantService";
import {usePathname} from "next/navigation";
import IProductVariantModel from "@/models/product/variant/IProductVariantModel";

const BackgroundCarousel = ({data} : {data: IProductVariantModel}) => {

    const images = data?.images ?? [];

    const slides = images.length > 0 ? [...images, images[0]] : [];

    const [currentIndex, setCurrentIndex] = useState(0);
    const [withTransition, setWithTransition] = useState(true);

    useEffect(() => {
        if (images.length <= 1) return;

        setCurrentIndex(0);
        setWithTransition(true);

        const interval = setInterval(() => {
            setCurrentIndex(prev => prev + 1);
        }, 3000);

        return () => clearInterval(interval);
    }, [images.length]);

    // коли доїхали до клону (останній елемент slides) — після завершення
    // анімації миттєво (без transition) телепортуємось на справжній перший слайд
    useEffect(() => {
        if (currentIndex === slides.length - 1) {
            const timeout = setTimeout(() => {
                setWithTransition(false);
                setCurrentIndex(0);
            }, 1200); // має співпадати з duration transition нижче
            return () => clearTimeout(timeout);
        } else {
            setWithTransition(true);
        }
    }, [currentIndex, slides.length]);

    return (
        <div className="relative w-full h-screen overflow-hidden">
            {slides.map((src, index) => (
                <div
                    key={index}
                    className={`absolute inset-0 bg-cover bg-center ${
                        withTransition ? "transition-transform duration-[1200ms] ease-in-out" : ""
                    }`}
                    style={{
                        backgroundImage: `url(${src})`,
                        transform: `translateX(${(index - currentIndex) * 100}%)`,
                    }}
                />
            ))}
            <div className="absolute inset-0 bg-black/40" />
        </div>
    );
};

export default BackgroundCarousel;