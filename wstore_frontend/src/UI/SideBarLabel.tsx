import React, {FC, ReactNode, useState} from 'react';
import Link from "next/link";
import {usePathname} from "next/navigation";

type SubMenuItem = {
    href: string;
    label: string;
}

type NavBarLabelProps = {
    handleClick?: () => void;
    children: ReactNode;
    href: string;
    subItems?: SubMenuItem[];
}

const SideBarLabel: FC<NavBarLabelProps> = ({href, children, subItems}) => {
    const path = usePathname();
    const [open, setOpen] = useState(false);
    const hasSubItems = subItems && subItems.length > 0;

    const baseClass = `bg-[var(--tag)] border-[0.5px] border-solid border-[var(--border)] text-[var(--tag-text)]
        rounded-xl text-xs md:text-sm lg:text-base py-2 px-4
        flex  items-center whitespace-nowrap
        font-semibold cursor-pointer w-[85%]
         transition-all`;

    const activeClass = "bg-blue-500/10 border-blue-500/20 text-blue-500 shadow-sm  ";

    if (hasSubItems) {
        return (
            <div className="w-[85%] flex flex-col gap-1">
                <button
                    onClick={() => setOpen(prev => !prev)}
                    className={`${baseClass} w-full justify-between ${path.startsWith(href) ? activeClass : ""}`}
                >
                    <span>{children}</span>
                    <span className={`ml-2 text-xs  transition-transform duration-200 ${open ? "rotate-180" : ""}`}>
                        ▼
                    </span>
                </button>

                {open && (
                    <div className="flex flex-col gap-1 pl-4">
                        {subItems.map((item) => (
                            <Link
                                key={item.href}
                                href={item.href}
                                className={`${baseClass} justify-start  hover:translate-x-2 transition-all w-full text-xs ${path === item.href ? activeClass+"translate-x-2" : ""}`}
                            >
                                {item.label}
                            </Link>
                        ))}
                    </div>
                )}
            </div>
        );
    }

    return (
        <Link
            href={href}
            className={`${baseClass} hover:-translate-y-1 ${path === href ? activeClass : ""}`}
        >
            {children}
        </Link>
    );
};

export default SideBarLabel;