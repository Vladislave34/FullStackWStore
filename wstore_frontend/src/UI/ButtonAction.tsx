import {FC, ReactNode} from "react";

type ButtonProps = {
    handleClick?: () => void;
    children: ReactNode;
}

const ButtonAction: FC<ButtonProps> = ({handleClick, children}) => {
    return (
        <div
            style={{
                background: "var(--btn)",

            }}
            className="
            rounded-xl text-xs md:text-sm lg:text-base py-2 px-4
            flex justify-center items-center
            font-semibold cursor-pointer
            hover:scale-105 transition-transform
            flex-1 md:flex-none md:w-[30%] text-[#f0f0f0]"
            onClick={handleClick}
        >
            {children}
        </div>
    );
};

export default ButtonAction;