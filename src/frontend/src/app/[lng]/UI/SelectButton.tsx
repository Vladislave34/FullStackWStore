"use client";

import { useEffect, useRef, useState } from "react";

interface CustomSelectProps {
    label?: string;
    name: string;
    options: string[];
    value: string | null;
    onChange: (value: string | null) => void;
    placeholder?: string;
}

export default function CustomSelect({
                                         label,
                                         name,
                                         options,
                                         value,
                                         onChange,
                                         placeholder = "Оберіть значення",
                                     }: CustomSelectProps) {
    const [open, setOpen] = useState(false);
    const [highlighted, setHighlighted] = useState(-1);
    const rootRef = useRef<HTMLDivElement>(null);
    const listRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        function handleClickOutside(e: MouseEvent) {
            if (rootRef.current && !rootRef.current.contains(e.target as Node)) {
                setOpen(false);
            }
        }
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    useEffect(() => {
        if (open) {
            const idx = options.findIndex((o) => o === value);
            setHighlighted(idx >= 0 ? idx : 0);
        }
    }, [open, options, value]);

    function handleSelect(option: string) {
        // Повторний клік по вже обраній опції знімає вибір
        if (option === value) {
            onChange(null);
        } else {
            onChange(option);
        }
        setOpen(false);
    }

    function handleClear(e: React.MouseEvent) {
        e.stopPropagation();
        onChange(null);
        setOpen(false);
    }

    function handleKeyDown(e: React.KeyboardEvent) {
        if (!open) {
            if (e.key === "Enter" || e.key === " " || e.key === "ArrowDown") {
                e.preventDefault();
                setOpen(true);
            } else if (e.key === "Backspace" || e.key === "Delete") {
                if (value) onChange(null);
            }
            return;
        }

        if (e.key === "ArrowDown") {
            e.preventDefault();
            setHighlighted((h) => Math.min(h + 1, options.length - 1));
        } else if (e.key === "ArrowUp") {
            e.preventDefault();
            setHighlighted((h) => Math.max(h - 1, 0));
        } else if (e.key === "Enter") {
            e.preventDefault();
            if (highlighted >= 0) handleSelect(options[highlighted]);
        } else if (e.key === "Escape") {
            setOpen(false);
        }
    }

    return (
        <div ref={rootRef} className="flex flex-col gap-1.5 relative">
            {/*{label && (*/}
            {/*    <label htmlFor={name} className="text-xs font-semibold text-[var(--muted)]">*/}
            {/*        {label}*/}
            {/*    </label>*/}
            {/*)}*/}

            <button
                type="button"
                id={name}
                onClick={() => setOpen((o) => !o)}
                onKeyDown={handleKeyDown}
                aria-haspopup="listbox"
                aria-expanded={open}
                className="
          w-full flex items-center justify-between
          bg-[var(--card)] border border-[var(--border)]
          text-[var(--text)] text-sm
          rounded-lg px-3 py-2.5
          outline-none cursor-pointer
          hover:border-[var(--accent-mid)]
          focus:border-[var(--accent-mid)]
          transition-colors
        "
            >
        <span className={value ? "text-[var(--text)]" : "text-[var(--muted)]"}>
          {value ?? label}
        </span>

                <span className="flex items-center gap-1">
                    {value && (
                        <span
                            role="button"
                            tabIndex={0}
                            onClick={handleClear}
                            onKeyDown={(e) => {
                                if (e.key === "Enter" || e.key === " ") {
                                    e.preventDefault();
                                    handleClear(e as unknown as React.MouseEvent);
                                }
                            }}
                            aria-label="Скинути вибір"
                            className="
                                p-0.5 rounded-full
                                text-[var(--muted)] hover:text-[var(--text)]
                                hover:bg-[var(--accent-soft)]
                                transition-colors cursor-pointer
                            "
                        >
                            <svg
                                className="w-3.5 h-3.5"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                strokeWidth="2"
                            >
                                <path d="M18 6L6 18M6 6l12 12" />
                            </svg>
                        </span>
                    )}
                    <svg
                        className={`w-4 h-4 text-[var(--muted)] transition-transform duration-150 ${
                            open ? "rotate-180" : ""
                        }`}
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        strokeWidth="2"
                    >
                        <path d="M6 9l6 6 6-6" />
                    </svg>
                </span>
            </button>

            {open && (
                <div
                    ref={listRef}
                    role="listbox"
                    className="
            absolute top-full left-0 right-0 mt-1.5 z-20
            bg-[var(--surface)] border border-[var(--border)]
            rounded-lg shadow-lg
            py-1 max-h-60 overflow-y-auto
          "
                >

                    {options.map((option, idx) => {
                        const active = option === value;
                        const isHighlighted = idx === highlighted;
                        return (
                            <div
                                key={option}
                                role="option"
                                aria-selected={active}
                                onClick={() => handleSelect(option)}
                                onMouseEnter={() => setHighlighted(idx)}
                                className={`
                                  flex items-center justify-between
                                  px-3 py-2 text-sm cursor-pointer
                                  ${
                                    active
                                        ? "bg-[var(--accent-soft)] text-[var(--text)]"
                                        : isHighlighted
                                            ? "bg-[var(--accent-soft)]/60 text-[var(--text)]"
                                            : "text-[var(--text)]"
                                }
                `}
                            >
                                <span>{option}</span>
                                {active && (
                                    <svg
                                        className="w-4 h-4 text-[var(--accent-mid)]"
                                        viewBox="0 0 24 24"
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="2"
                                    >
                                        <path d="M20 6L9 17l-5-5" />
                                    </svg>
                                )}
                            </div>
                        );
                    })}
                </div>
            )}

            <select
                name={name}
                value={value ?? ""}
                onChange={() => {}}
                className="hidden"
                tabIndex={-1}
                aria-hidden="true"
            >
                <option value="" disabled>
                    {placeholder}
                </option>
                {options.map((option) => (
                    <option key={option} value={option}>
                        {option}
                    </option>
                ))}
            </select>
        </div>
    );
}