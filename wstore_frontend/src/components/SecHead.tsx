"use client"
import {FC, useState} from "react";

type SecHeadProps = {
    title: string
    seeAll: string
}

const SecHead : FC<SecHeadProps> = ({title, seeAll}) => {
    const [hovered, setHovered] = useState(false);
    return (
        <div style={{
            color: "var(--text)"
        }}
            className="flex justify-between items-center text-base">
            <p>
                {title}
            </p>
            <div
                style={{
                    color: hovered ? "var(--text)" : "var(--muted)"
                }}
                className="flex flex-row gap-2 cursor-pointer transition-colors"
                onMouseEnter={() => setHovered(true)}
                onMouseLeave={() => setHovered(false)}
            >
                {seeAll}
                <span>→</span>
            </div>
        </div>
    );
};

export default SecHead;