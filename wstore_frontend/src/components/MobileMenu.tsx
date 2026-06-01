"use client";

import { useState } from "react";
import Image from "next/image";
import Icon from "@/UI/Icon";
import SwitchThemeIcon from "@/UI/SwitchThemeIcon";
import SwitchLocaleIcon from "@/UI/SwitchLocaleIcon";

interface NavLink {
    key: string;
    label: string;
}

export default function MobileMenu({ links }: { links: NavLink[] }) {
    const [open, setOpen] = useState(false);

    return (
        <>
            {/* Burger button */}
            <button
                onClick={() => setOpen((prev) => !prev)}
                aria-label="Меню"
                style={{ color: "var(--text)" }}
                className="flex flex-col justify-center items-center w-10 h-10 gap-[5px]"
            >
                <span
                    className={`block h-[2px] w-6 bg-current transition-transform duration-300 ${
                        open ? "translate-y-[7px] rotate-45" : ""
                    }`}
                />
                <span
                    className={`block h-[2px] w-6 bg-current transition-opacity duration-300 ${
                        open ? "opacity-0" : ""
                    }`}
                />
                <span
                    className={`block h-[2px] w-6 bg-current transition-transform duration-300 ${
                        open ? "-translate-y-[7px] -rotate-45" : ""
                    }`}
                />
            </button>

            {/* Drawer */}
            {open && (
                <div
                    style={{
                        background: "var(--surface)",
                        borderTop: "1px solid var(--border)",
                    }}
                    className="absolute top-[75px] left-0 w-full z-50 flex flex-col px-6 py-4 gap-4 shadow-md"
                >
                    {/* Nav links */}
                    {links.map((link) => (
                        <span
                            key={link.key}
                            style={{ color: "var(--muted)" }}
                            className="text-base hover:text-[var(--text)] hover:cursor-pointer transition-colors border-b border-[var(--border)] pb-3"
                            onClick={() => setOpen(false)}
                        >
                            {link.label}
                        </span>
                    ))}

                    {/* Icons row */}
                    <div className="flex items-center gap-4 pt-1">
                        <SwitchLocaleIcon />
                        <SwitchThemeIcon />
                        <Icon height={40} width={40}>
                            <Image src="/shopping-bags.png" alt="Cart" width={24} height={24} />
                        </Icon>
                        <Icon height={40} width={40}>
                            <Image src="/user.png" alt="Profile" width={24} height={24} />
                        </Icon>
                    </div>
                </div>
            )}
        </>
    );
}