import {FC, type ReactNode} from "react";

interface NavBarProps {
    children: ReactNode;
}

const SideBar  : FC<NavBarProps> = ({children}) => {
    return (
        <div
            className="flex flex-col items-center
             bg-[var(--surface)] gap-8 border-r-2
             border-[var(--border)] pt-8 w-[15%]
             min-w-[15%] max-w-[15%]
             sticky top-0 h-screen self-start
             "
        >
            {children}
        </div>
    );
};

export default SideBar;