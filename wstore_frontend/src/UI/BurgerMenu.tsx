'use client'
import { useState } from "react";
import Image from "next/image";
import Icon from "@/UI/Icon";
import SearchBar from "@/UI/SearchBar";




interface BurgerMenuProps {
    showSearch?: boolean;

}

export default function BurgerMenu({ showSearch = false,  }: BurgerMenuProps) {
    const [open, setOpen] = useState(false);


    return (
        <div className="relative">

            {/* Burger toggle button */}
            <div
                onClick={() => setOpen(!open)}
                className="flex flex-col justify-between w-8 h-6 cursor-pointer"
            >
                {open ? (
                    <span
                        className="flex justify-center items-center text-xl leading-none"
                        style={{ color: "var(--text)" }}
                    >
                        ✕
                    </span>
                ) : (
                    new Array(3).fill(null).map((_, index) => (
                        <span
                            key={index}
                            className="block h-0.5 rounded"
                            style={{ background: "var(--text)" }}
                        />
                    ))
                )}
            </div>

            {/* Dropdown menu */}
            {open && (
                <div
                    style={{
                        background: "var(--surface)",
                        border: "1px solid var(--border)",
                    }}
                    className="absolute top-14 right-0 shadow-lg rounded-xl p-4 w-52 z-50"
                >
                    {/* Search — only on mobile (tablet has it in header) */}
                    {showSearch && (
                        <div className="mb-3">
                            <SearchBar />
                        </div>
                    )}

                    {/* Nav links */}
                    <ul
                        style={{ color: "var(--muted)" }}
                        className="flex flex-col gap-2 text-sm"
                    >
                        {['women', 'men', 'sale'].map((key) => (
                            <li
                                key={key}
                                className="hover:text-[var(--text)] cursor-pointer transition-colors py-1"
                            >
                                {key}
                            </li>
                        ))}
                    </ul>

                    {/* Divider */}
                    <div
                        style={{ background: "var(--border)" }}
                        className="my-3 h-px w-full"
                    />

                    {/* Cart + Profile */}
                    <div className="flex flex-row gap-3">
                        <Icon height={40} width={40}>
                            <Image src="/shopping-bags.png" alt="Cart" width={24} height={24} />
                        </Icon>
                        <Icon height={40} width={40}>
                            <Image src="/user.png" alt="Profile" width={24} height={24} />
                        </Icon>
                    </div>
                </div>
            )}
        </div>
    );
}