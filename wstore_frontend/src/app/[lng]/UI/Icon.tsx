
import {FC, ReactNode} from "react";

interface IconProps {
    height: number;
    width: number;
    border?: string;
    isOpen?: boolean;
    children: ReactNode
}

const Icon : FC<IconProps> = ({
                            height, width, border="", isOpen,  children
                              }) => {
    return (
        <div
            style={{
                background: "var(--accent-soft)",
                height: `${height}px`,
                width: `${width}px`,
                // border: border ? "1px solid var(--border) " : undefined,
            }}
            className={`flex items-center justify-center
             hover:cursor-pointer  rounded-xl
              transition hover:border
             ${isOpen && "hover:border-2 border-2" }
             `}

        >
            {children}
        </div>
    );
};

export default Icon;