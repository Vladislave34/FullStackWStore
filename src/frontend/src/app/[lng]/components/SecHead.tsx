"use client"
import {FC, useState} from "react";
import {useRouter} from "next/navigation";

type SecHeadProps = {
    title: string
    seeAll: string
    lng: string
}

const SecHead : FC<SecHeadProps> = ({title, seeAll, lng}) => {
    const [hovered, setHovered] = useState(false);
    const router = useRouter();
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
                onClick={()=>router.push(`/${lng}/products/all?page=1`)}
            >
                {seeAll}
                <span >→</span>
            </div>
        </div>
    );
};

export default SecHead;