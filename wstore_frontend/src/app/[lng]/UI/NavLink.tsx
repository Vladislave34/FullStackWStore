'use client'

import Link from "next/link";
import { ReactNode } from "react";
import { useSearchParams } from "next/navigation";

const GENDER_IDS: Record<"women" | "men", string> = {
    women: "019f563d-770b-7be3-b6ce-605b9ea8965c", // заміни на реальні id/slug з бекенду
    men: "019f563d-7709-7a54-af0d-1f775ae5ce77",
};

const NavLink = ({
                     lng,
                     children,
                     choice,
                     gender,
                 }: {
    lng: string;
    children: ReactNode;
    choice: "genderId" | "hasSale";
    gender?: "women" | "men";
}) => {
    const searchParams = useSearchParams();

    const hasSale = searchParams.get("hasSale") === "true" ? true : null;
    const currentGenderId = searchParams.get("genderId");

    let href: string;
    let isActive: boolean | null;

    if (choice === "hasSale") {
        href = `/${lng}/products/all?hasSale=true&page=1`;
        isActive = hasSale;
    } else {
        const genderId = gender ? GENDER_IDS[gender] : "";
        href = `/${lng}/products/all?genderId=${genderId}&page=1`;
        isActive = currentGenderId === genderId;
    }

    return (
        <Link
            href={href}
            className={`
                hover:text-[var(--text)] hover:cursor-pointer
                transition-colors hover:border-b-2
                ${isActive ? "border-b-2 text-[var(--text)]" : ""}
            `}
        >
            {children}
        </Link>
    );
};

export default NavLink;