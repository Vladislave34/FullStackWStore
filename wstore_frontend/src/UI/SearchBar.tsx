'use client'

import { useState, useRef } from "react";
import Icon from "@/UI/Icon";
import Image from "next/image";
import {ImSearch} from "react-icons/im";

const SearchBar = () => {
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [search, setSearch] = useState<string>("");
    const inputRef = useRef<HTMLInputElement>(null);

    const handleOpen = () => {
        setIsOpen(prev => {
            if (!prev) setTimeout(() => inputRef.current?.focus(), 50);
            return !prev;
        });
    };

    return (
        <div className="relative flex items-center">
            {/* Input — absolute, розкривається вліво */}
            <input
                ref={inputRef}
                type="text"
                value={search}

                onChange={(e) => setSearch(e.target.value)}
                onBlur={() => setIsOpen(false)}
                placeholder="Search..."
                style={{
                    background: "var(--surface)",
                    color: "var(--text)",
                    borderColor: "var(--border)",
                }}
                className={`
                    absolute right-12
                    transition-all duration-300
                    py-2 rounded-xl border 
                    focus:outline-none
                    ${isOpen
                    ? "w-36 md:w-44 lg:w-56 opacity-100 px-3 pointer-events-auto"
                    : "w-0 opacity-0 px-0 border-0 pointer-events-none"
                }
                `}
            />


            <div
                 onMouseDown={(e) => {
                     e.preventDefault();
                     handleOpen();
                 }}
            >
                <Icon isOpen={isOpen} height={40} width={40}>
                    {/*<Image src="/transparency.png" alt="Search" width={24} height={24} />*/}
                    <ImSearch size={22} color="var(--accent-mid)" />

                </Icon>
            </div>
        </div>
    );
};

export default SearchBar;