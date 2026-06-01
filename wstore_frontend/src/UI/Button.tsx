import {FC, ReactNode} from "react";


type ButtonProps = {
    handleClick?: () => void;
    children: ReactNode;
}

const Button : FC<ButtonProps> = ({handleClick, children}) => {
    return (
        <div
            // style={{
            //     border: "0.5px solid var(--border)",
            //     color: "var(--text)",
            //
            // }}
            // className="
            // rounded-xl text-base  py-2 px-4
            // flex justify-center items-center
            // font-semibold w-[24%] cursor-pointer hover:border-2"
            onClick={handleClick}
            className="rounded-xl text-xs md:text-sm lg:text-base py-2 px-4
                       flex justify-center items-center
                       font-semibold cursor-pointer
                       outline outline-[0.5px] outline-[var(--border)]
                       hover:outline-2 transition-all
                       text-[var(--text)]
                       flex-1 md:flex-none md:w-[24%]"
        >
            {children}
        </div>
    );
};

export default Button;