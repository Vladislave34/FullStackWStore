'use client'

import Image from "next/image";
import { useEffect, useRef, useState } from "react";

const images = [
    "/_3acab0d4-9150-429e-93df-2f4888a33c42.jpeg",
    "/_30e15f1d-e709-4c45-986f-f535ec82942c.jpeg",
    "/_980d4e4b-4068-4bbd-bde0-00c31728f117.jpeg"
];

const Carousel = () => {
    const [current, setCurrent] = useState(0);
    const [direction, setDirection] = useState<"left" | "right">("right");
    const prevRef = useRef(0);

    useEffect(() => {
        const interval = setInterval(() => {
            prevRef.current = current;
            setDirection("right");

            setCurrent(prev =>
                prev === images.length - 1 ? 0 : prev + 1
            );
        }, 4000);

        return () => clearInterval(interval);
    }, [current]);

    const goTo = (index: number) => {
        if (index === current) return;

        prevRef.current = current;
        setDirection(index > current ? "right" : "left");
        setCurrent(index);
    };

    return (
        <div className="relative w-full h-[351px] overflow-hidden">
            {images.map((src, index) => {
                let className = "translate-x-full opacity-0";

                // active slide
                if (index === current) {
                    className = "translate-x-0 opacity-100 z-20";
                }

                // previous slide animation
                else if (index === prevRef.current) {
                    className =
                        direction === "right"
                            ? "-translate-x-full opacity-0 z-10"
                            : "translate-x-full opacity-0 z-10";
                }

                return (
                    <div
                        key={index}
                        className={`
                            absolute inset-0
                            transition-all duration-700 ease-in-out
                            ${className}
                        `}
                    >
                        <Image
                            src={src}
                            alt={`slide-${index}`}
                            fill
                            className="object-cover"
                            priority={index === 0}
                        />
                    </div>
                );
            })}


        </div>
    );
};

export default Carousel;