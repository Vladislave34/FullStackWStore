"use client";

interface CustomCheckboxProps {
    id: string;
    name: string;
    value: string;
    label?: string;
    checked: boolean;
    onChange: (checked: boolean) => void;
}

export default function CustomCheckbox({
                                           id,
                                           name,
                                           value,
                                           label,
                                           checked,
                                           onChange,
                                       }: CustomCheckboxProps) {
    return (
        <label
            htmlFor={id}
            className="
        flex items-center gap-2.5
        cursor-pointer select-none
        group
      "
        >
      <span className="relative inline-flex items-center justify-center shrink-0">
        <input
            id={id}
            type="checkbox"
            name={name}
            value={value}
            checked={checked}
            onChange={(e) => onChange(e.target.checked)}
            className="peer sr-only"
        />

        <span
            className="
            w-5 h-5 rounded-md
            bg-[var(--card)] border-2 border-[var(--border)]
            transition-colors duration-150
            peer-hover:border-[var(--accent-mid)]
            peer-focus-visible:ring-2 peer-focus-visible:ring-[var(--accent-soft)]
            peer-checked:bg-[var(--accent-mid)]
            peer-checked:border-[var(--accent-mid)]
          "
        />

        <svg
            className="
            absolute w-3.5 h-3.5 text-white
            opacity-0 scale-75
            peer-checked:opacity-100 peer-checked:scale-100
            transition-all duration-150
            pointer-events-none
          "
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="3"
            strokeLinecap="round"
            strokeLinejoin="round"
        >
          <path d="M20 6L9 17l-5-5" />
        </svg>
      </span>

            {label && (
                <span className="text-sm font-medium text-[var(--text)]">
          {label}
        </span>
            )}
        </label>
    );
}