import {FC, ReactNode} from "react";


type LabelProps = {
    handleClick?: () => void;
    children: ReactNode;

}

const Label : FC<LabelProps> = ({handleClick, children}) => {
    return (
        <div
            style={{
                background: "var(--tag)",
                border: "0.5px solid var(--border)",
                color: "var(--tag-text)"
            }}
            className="
            rounded-3xl text-xs md:text-sm lg:text-base  py-2 px-4
            flex justify-center items-center whitespace-nowrap
            font-semibold  cursor-pointer
            hover:-translate-y-1 transition-all
            "
            onClick={handleClick}
        >
            {children}
        </div>
    );
};

export default Label;