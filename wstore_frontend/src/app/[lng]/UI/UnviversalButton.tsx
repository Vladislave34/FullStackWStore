import {FC, ReactNode} from "react";

interface UnviversalButtonProps {
    heigth: number;
    width: number;
    color: string;
    children: ReactNode;
    onClick?: () => void;
    text: "xs" | "sm" | "base" | "lg" | "xl" | "2xl" | "3xl";
}

const UnviversalButton : FC<UnviversalButtonProps> = (
    {
        onClick,
        children,
        width,
        color,
        heigth,
        text
    }
) => {


    return (
        <div

            onClick={onClick}
            className={`rounded-xl text-${text}  py-2 px-4 h-${heigth/4} w-${width/4}
                       flex justify-center items-center bg-[var(--${color})]
                       font-semibold cursor-pointer
                       outline outline-[0.5px] outline-[var(--border)]
                       hover:outline-2 transition-all
                       text-[var(--text)]
                       flex-1 md:flex-none md:w-[32%]`}
        >
            {children}
        </div>
    );
};

export default UnviversalButton;